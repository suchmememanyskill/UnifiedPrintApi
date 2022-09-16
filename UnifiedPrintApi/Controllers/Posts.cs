using Microsoft.AspNetCore.Mvc;
using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Interfaces.Generic;
using UnifiedPrintApi.Service;
using UnifiedPrintApi.Service.Thingiverse;

namespace UnifiedPrintApi.Controllers;

[ApiController]
[Route("[controller]")]
public class Posts : ControllerBase
{
    private Apis _apis;
    private Cache _cache;
    private Storage _storage;
    public Posts(Apis apis, Cache cache, Storage storage)
    {
        _apis = apis;
        _cache = cache;
        _storage = storage;
    }

    [HttpGet("services")]
    public List<IApiDescription> GetApis() => _apis.GetApis();

    [HttpGet("list/{apiName}/search")]
    public IApiPreviewPosts? GetPostsSearch(string apiName, string query, int page = 1, int perPage = 20)
    {
        IApiDescription? desc = _apis.GetApi(apiName);

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
        IApiDescription? desc = _apis.GetApi(apiName);

        if (desc == null)
        {
            Response.StatusCode = 404;
            return null;
        }

        SortType? type = desc.GetSortType(sortType);
        
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
        _storage.BaseUrl = $"{Request.Scheme}://{Request.Host.Value}"; // Hack
        IApiPost? post = _apis.GetUID(uid);

        if (post == null)
        {
            Response.StatusCode = 404;
            return null;
        }

        return post.Generic();
    }
}