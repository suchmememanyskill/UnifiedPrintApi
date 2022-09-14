using Microsoft.AspNetCore.Mvc;
using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Post;

namespace UnifiedPrintApi.Controllers;

[ApiController]
[Route("[controller]")]
public class Saved : ControllerBase
{
    [HttpGet("{token}")]
    public List<IApiPost> GetSavedPosts(string token)
    {
        return new();
    }

    [HttpPost]
    public string GetNewSaveToken(SavedNew data)
    {
        return "";
    }

    [HttpPost("{token}/add")]
    public string AddPostToSaves(string token, SavedTokenAdd data)
    {
        return "";
    }
    
    [HttpDelete("{token}/remove")]
    public string RemovePostFromSaves(string token, SavedTokenAdd data)
    {
        return "";
    }
    
    
}