using Newtonsoft.Json;
using Service.Thingiverse.Models;
using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Interfaces.Generic;
using Utils;

namespace UnifiedPrintApi.Service.Thingiverse;

public record ThingiverseSortType(string Name, string UrlPart)
{
    public string InternalName => Name.ToLower().Replace(' ', '-');
}

public class ThingiverseApi : IApiDescription
{
    public string Name => "Thingiverse";
    public string Color => "#0359B5";
    public static string apiKey = "Bearer 56edfc79ecf25922b98202dd79a291aa";
    
    public static readonly List<ThingiverseSortType> ActualSortTypes = new()
    {
        new("Popular Last 7 Days", "sort=popular&posted_after=now-7d"),
        new("Popular Last 30 Days", "sort=popular&posted_after=now-30d"),
        new("Popular This Year", "sort=popular&posted_after=now-365d"),
        new("Popular All Time", "sort=popular"),
        new("Newest", "sort=newest"),
        new("Most Makes", "sort=makes")
    };

    public List<SortType> SortTypes => ActualSortTypes.Select(x => new SortType(x.Name, x.InternalName)).ToList();
    public Uri Site => new("https://www.thingiverse.com/");
    public string Description => "Provides access to the Thingiverse API";
    private Cache _cache;

    public ThingiverseApi(Cache cache)
    {
        _cache = cache;
    }

    public string? MakeRequest(string url)
    {
        string hash = Cache.Hash(url);
        return _cache.CacheValue<string>(hash,
            () => Request.GetString(new Uri(url), new() {{"Authorization", apiKey}}), TimeSpan.FromHours(2));
    }

    public IApiPreviewPosts GetPosts(SortType type, int page, int perPage)
        => GetPostsBySearchOrSortType(page, perPage, type);

    public IApiPreviewPosts GetPostsBySearch(string search, int page, int perPage)
        => GetPostsBySearchOrSortType(page, perPage, null, search);
    public IApiPost GetPostById(string id)
    {
        string url = $"https://api.thingiverse.com/things/{id}";
        string? response = MakeRequest(url);
        
        if (response == null)
            throw new Exception("Request failed");
        
        RequestSpecificThing parsedResponse = JsonConvert.DeserializeObject<RequestSpecificThing>(response);
        ThingiversePost post = new(this, parsedResponse);
        post.GetDownloads();
        post.GetImages();
        return post.Generic();
    }

    private IApiPreviewPosts GetPostsBySearchOrSortType(int page, int perPage, SortType? sortType = null, string? search = null)
    {
        int min = (page - 1) * perPage;
        int max = min + perPage;
        int apiLimit = 20;
        int current = min / apiLimit * apiLimit;
        int diff = min % apiLimit;
        ThingiverseSortType? ttype = ActualSortTypes.Find(x => x.Name == sortType?.DisplayName);

        if ((sortType != null && search != null) || (sortType == null && search == null))
            throw new ArgumentException("Sort and search was provided, or neither were provided");

        long total = -1;
        List<ThingiversePreviewPost> posts = new();

        if (sortType != null && ttype == null)
            throw new Exception("Invalid sort type");

        for (; max > current; current += apiLimit)
        {
            string url;

            if (ttype != null)
                url =
                    $"https://api.thingiverse.com/search/?page={current / apiLimit + 1}&per_page={apiLimit}&{ttype.UrlPart}&type=things";
            else
                url =
                    $"https://api.thingiverse.com/search/{search}?page={current / apiLimit + 1}&per_page={apiLimit}&sort=relevant&type=things";
            
            string? response = MakeRequest(url);

            if (response == null)
                throw new Exception("Request failed");
            
            RequestThings parsedResponse = JsonConvert.DeserializeObject<RequestThings>(response);
            if (total <= -1)
                total = parsedResponse.Total;

            if (parsedResponse.Hits.Count <= 0)
                break;
            
            posts.AddRange(parsedResponse.Hits.Select(x => new ThingiversePreviewPost(this, x)));
        }

        return new GenericApiPreviewPosts(posts.Skip(diff).Take(max - min), total);
    }
}