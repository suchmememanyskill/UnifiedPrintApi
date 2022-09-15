using Microsoft.AspNetCore.Mvc;
using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Service;
using UnifiedPrintApi.Service.Thingiverse;

namespace UnifiedPrintApi.Controllers;

[ApiController]
[Route("[controller]")]
public class Posts : ControllerBase
{
    private ThingiverseApi _thingiverse;
    private Cache _cache;
    public Posts(ThingiverseApi thingiverseApi, Cache cache)
    {
        _thingiverse = thingiverseApi;
        _cache = cache;
    }

    [HttpGet("services")]
    public List<IApiDescription> GetApis() => new() {_thingiverse};

    [HttpGet("list/{apiName}/search")]
    public IApiPreviewPosts? GetPostsSearch(string apiName, string query, int page = 1, int perPage = 20)
    {
        IApiDescription? desc = GetApis().Find(x => x.Slug == apiName);

        if (desc == null)
        {
            Response.StatusCode = 404;
            return null;
        }
        
        string key = Cache.Hash($"{apiName}:q:{query}:{page}:{perPage}");
        return _cache.CacheValue(key, () => desc.GetPostsBySearch(query, page, perPage));
    }
    
    // TODO: Limit
    [HttpGet("list/{apiName}/{sortType}")]
    public IApiPreviewPosts? GetPosts(string apiName, string sortType, int page = 1, int perPage = 20)
    {
        IApiDescription? desc = GetApis().Find(x => x.Slug == apiName);

        if (desc == null)
        {
            Response.StatusCode = 404;
            return null;
        }

        SortType? type = desc.SortTypes.Find(x => x.UriName == sortType);
        
        if (type == null)
        {
            Response.StatusCode = 404;
            return null;
        }

        string key = Cache.Hash($"{apiName}:s:{sortType}:{page}:{perPage}");
        return _cache.CacheValue(key, () => desc.GetPosts(type, page, perPage));
    }

    [HttpGet("universal/{uid}")]
    public IApiPost? Post(string uid)
    {
        string service = uid.Split(":")[0];
        string id = uid.Substring(service.Length + 1);
        
        IApiDescription? api = GetApis().Find(x => x.Slug == service);

        if (api == null)
        {
            Response.StatusCode = 404;
            return null;
        }
        
        string key = Cache.Hash(uid);
        return _cache.CacheValue(key, () => api.GetPostById(id));
    }
}