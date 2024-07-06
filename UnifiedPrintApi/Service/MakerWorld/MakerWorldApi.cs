using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Service.MakerWorld.Models;
using Utils;

namespace UnifiedPrintApi.Service.MakerWorld;

public record MakerWorldSortType(string Name, string UrlPart)
{
    public string InternalName => Name.ToLower().Replace(' ', '-');

    public static List<MakerWorldSortType> Types = new()
    {
        new("Trending last 7 days", "orderBy=hotScore&designCreateSince=7"),
        new("Trending last 30 days", "orderBy=hotScore&designCreateSince=30"),
        new("Trending all time", "orderBy=hotScore&designCreateSince=0"),
        new("Most downloads last 30 days", "orderBy=downloadCount&designCreateSince=30"),
        new("Most downloads all time", "orderBy=downloadCount&designCreateSince=0"),
        new("Most liked last 30 days", "orderBy=likeCount&designCreateSince=30"),
        new("Most liked all time", "orderBy=likeCount&designCreateSince=0"),
        new ("Featured", "FEATURED")
    };
}

public class MakerWorldApi : IApiDescription
{
    public string Name => "MakerWorld";
    public string Color => "#00AE42";
    public List<SortType> SortTypes => MakerWorldSortType.Types.Select(x => new SortType(x.Name, x.InternalName, null)).ToList();
    public Uri Site => new("https://makerworld.com");
    public string Description => "Provides access to the MakerWorld API";

    private Cache _cache;

    public MakerWorldApi(Cache cache)
    {
        _cache = cache;
    }

    public string? GetMakerWorldKey()
    {
        return _cache.CacheValue("__makerworld_build_key__", () =>
        {
            try
            {
                var mainWebpage = Request.GetStringWithFlareSolver(new("https://makerworld.com/en"));
                var pattern = @"\/_next\/static\/[^/]*\/_buildManifest\.js";
                var rg = new Regex(pattern);
                return rg.Matches(mainWebpage).Single().Value[14..^18];
            }
            catch (Exception ex)
            {
                return null;
            }
        }, TimeSpan.FromDays(1));
    }
    
    public IApiPreviewPosts GetPosts(SortType type, int page, int perPage)
    {
        MakerWorldSortType sortType = MakerWorldSortType.Types.Find(x => x.Name == type.DisplayName)!;

        if (sortType.UrlPart == "FEATURED")
        {
            var key = GetMakerWorldKey();
            if (key == null)
            {
                throw new Exception("Failed to get makerworld key");
            }
            
            string featuredResponse = Request.GetStringWithFlareSolver(
                new($"https://makerworld.com/_next/data/{key}/en.json"));

            MWRootFeatured props = JsonConvert.DeserializeObject<MWRootFeatured>(featuredResponse)!;
            props.PageProps?.Designs.ForEach(x => x.Design.Api = this);
            return props.PageProps;
        }
        
        string response = Request.GetStringWithFlareSolver(
            new($"https://makerworld.com/api/v1/search-service/select/design?{sortType.UrlPart}&keyword=&limit={perPage}&offset={(page - 1) * perPage}"));

        MWSearch search = JsonConvert.DeserializeObject<MWSearch>(response)!;
        search.Hits.ForEach(x => x.Api = this);
        return search;
    }

    public IApiPreviewPosts GetPostsBySearch(string search, int page, int perPage)
    {
        string response = Request.GetStringWithFlareSolver(
            new($"https://makerworld.com/api/v1/search-service/select/design?orderBy=score&keyword={search}&limit={perPage}&offset={(page - 1) * perPage}"));
        
        MWSearch mwSearch = JsonConvert.DeserializeObject<MWSearch>(response)!;
        mwSearch.Hits.ForEach(x => x.Api = this);
        return mwSearch;
    }

    public IApiPost? GetPostById(string id)
    {
        var key = GetMakerWorldKey();

        if (key == null)
        {
            return null;
        }
        
        Console.WriteLine($"Got MakerWorld key: {key}");
        
        string response = Request.GetStringWithFlareSolver(
            new($"https://makerworld.com/_next/data/{key}/en/models/{id}.json?designId={id}"));

        MWRootModel props = JsonConvert.DeserializeObject<MWRootModel>(response)!;
        props.PageProps.Design.Api = this;

        return props.PageProps.Design;
    }
}