using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Interfaces.Generic;
using UnifiedPrintApi.Service.MMF.Models;
using Utils;

namespace UnifiedPrintApi.Service.MMF;

public record MMFSortType(string Name, string UrlPart)
{
    public string InternalName => Name.ToLower().Replace(' ', '-');
}

public class MMFApi : IApiDescription
{
    public static string apiKey = "d7c64faa-aa6e-4645-b47a-95cf3ddc991a"; // This is a personal API key. Please do not use this
    public string Name => "MyMiniFactory";
    public string Color => "#00C4A6";

    public static readonly List<MMFSortType> ActualSortTypes = new()
    {
        new("Featured Popular", "&featured=1&sortBy=popularity"),
        new("Featured Recently Published", "&featured=1&sortBy=date"),
        new("Featured Most Viewed", "&featured=1&sortBy=visits"),
        new("Relevance", ""),
        new("Popularity", "&sortBy=popularity"),
        new("Latest Published", "&sortBy=date"),
        new("Most Viewed", "&sortBy=visits")
    };

    public List<SortType> SortTypes => ActualSortTypes.Select(x => new SortType(x.Name, x.InternalName)).ToList();
    public Uri Site => new("https://www.myminifactory.com/");
    public string Description => "Provides access to the MMF Api";
    private Cache _cache;
    private Storage _storage;

    public MMFApi(Cache cache, Storage storage)
    {
        _cache = cache;
        _storage = storage;
    }

    public IApiPreviewPosts GetPosts(SortType type, int page, int perPage)
        => GetPostsBySearchOrSortType(page, perPage, type);

    public IApiPreviewPosts GetPostsBySearch(string search, int page, int perPage)
        => GetPostsBySearchOrSortType(page, perPage, null, search);

    public IApiPost? GetPostById(string id)
    {
        string url = $"https://www.myminifactory.com/api/v2/objects/{id}?key={apiKey}";
        string? response = _cache.CacheValue(Cache.Hash(url), () =>
        {
            try
            {
                return Request.GetString(new(url));
            }
            catch
            {
                return null;
            }
        });

        if (response == null)
            return null;

        FetchSpecificObject result = JsonConvert.DeserializeObject<FetchSpecificObject>(response);
        return new MMFPost(this, result);
    }

    public Stream GetDownloadFromPost(string id, string fileName)
    {
        MMFPost post = (MMFPost)GetPostById(id);
        return post.Download(fileName);
    }

    private IApiPreviewPosts GetPostsBySearchOrSortType(int page, int perPage, SortType? sortType = null,
        string? search = null)
    {
        int min = (page - 1) * perPage;
        int max = min + perPage;
        int apiLimit = 20;
        int current = min / apiLimit * apiLimit;
        int diff = min % apiLimit;
        MMFSortType? ttype = ActualSortTypes.Find(x => x.Name == sortType?.DisplayName);

        if ((sortType != null && search != null) || (sortType == null && search == null))
            throw new ArgumentException("Sort and search was provided, or neither were provided");

        long total = -1;
        List<MMFPreviewPost> posts = new();

        if (sortType != null && ttype == null)
            throw new Exception("Invalid sort type");

        for (; max > current; current += apiLimit)
        {
            string url;

            if (ttype != null)
                url =
                    $"https://www.myminifactory.com/search/fetch_search/?object=1&page={current / apiLimit + 1}&store=0{ttype.UrlPart}";
            else
                url =
                    $"https://www.myminifactory.com/search/fetch_search/?object=1&page={current / apiLimit + 1}&store=0&query={search}";

            string? response = Request.GetString(new(url));

            if (response == null)
                throw new Exception("Request failed");

            FetchResultsResult parsedResponse = JsonConvert.DeserializeObject<FetchResultsResult>(response);
            if (total <= -1)
                total = parsedResponse.Total;

            if (parsedResponse.ObjectResults.Count <= 0)
                break;
            
            posts.AddRange(parsedResponse.ObjectResults.Select(x => new MMFPreviewPost(this, x)));
        }
        
        return new GenericApiPreviewPosts(posts.Skip(diff).Take(max - min), total);
    }
}