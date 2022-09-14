using Microsoft.AspNetCore.Mvc;
using UnifiedPrintApi.Model.Interfaces;

namespace UnifiedPrintApi.Controllers;

[ApiController]
[Route("[controller]")]
public class Posts : ControllerBase
{
    [HttpGet]
    public List<IApiDescription> GetApis() => new();

    [HttpGet("list/{apiName}/search")]
    public List<IApiPreviewPost> GetPostsSearch(string apiName, string query, int page = 1, int perPage = 20)
    {
        return new();
    }
    
    [HttpGet("list/{apiName}/{sortType}")]
    public List<IApiPreviewPost> GetPosts(string apiName, int page = 1, int perPage = 20)
    {
        return new();
    }

    [HttpGet("id/{id}")]
    public IApiPost Post(string id)
    {
        return null!;
    }
}