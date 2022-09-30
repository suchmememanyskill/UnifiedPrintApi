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
    public Posts(Apis apis, Cache cache)
    {
        _apis = apis;
        _cache = cache;
    }

    [HttpGet("services")]
    public List<IApiDescription> GetApis() => _apis.GetApis();

    [HttpGet("list/{apiName}/search")]
    [ProducesResponseType(typeof(IApiPreviewPosts), StatusCodes.Status200OK)]
    public object? GetPostsSearch(string apiName, string query, int page = 1, int perPage = 20)
    {
        try
        {
            if (perPage > 50)
                throw new Exception("That's a little much don't you think");
            
            IApiDescription? desc = _apis.GetApi(apiName);

            if (desc == null)
            {
                Response.StatusCode = 404;
                return "Api not found";
            }

            string key = Cache.Hash($"{apiName}:q:{query}:{page}:{perPage}");
            return _cache.CacheValue(key, () => desc.GetPostsBySearch(query, page, perPage).Generic());
        }
        catch (Exception e)
        {
            Response.StatusCode = 400;
            return e.Message;
        }
    }
    
    // TODO: Limit
    [HttpGet("list/{apiName}/{sortType}")]
    [ProducesResponseType(typeof(IApiPreviewPosts), StatusCodes.Status200OK)]
    public object? GetPosts(string apiName, string sortType, int page = 1, int perPage = 20)
    {
        try
        {
            if (perPage > 50)
                throw new Exception("That's a little much don't you think");

            IApiDescription? desc = _apis.GetApi(apiName);

            if (desc == null)
            {
                Response.StatusCode = 404;
                return "Api not found";
            }

            SortType? type = desc.GetSortType(sortType);

            if (type == null)
            {
                Response.StatusCode = 404;
                return "Sort type not found";
            }

            string key = Cache.Hash($"{apiName}:s:{sortType}:{page}:{perPage}");
            return _cache.CacheValue(key, () => desc.GetPosts(type, page, perPage).Generic());
        }
        catch (Exception e)
        {
            Response.StatusCode = 400;
            return e.Message;
        }
    }
    
    [HttpGet("universal/{uid}")]
    [ProducesResponseType(typeof(IApiPost), StatusCodes.Status200OK)]
    public object? Post(string uid)
    {
        Storage.BaseUrl = $"{Request.Scheme}://{Request.Host.Value}";
        try
        {
            IApiPost? post = _apis.GetUID(uid);
            
            if (post == null)
            {
                Response.StatusCode = 404;
                return "Id not found";
            }

            return post.Generic();
        }
        catch (Exception e)
        {
            Response.StatusCode = 400;
            return e.Message;
        }
    }
}