using Microsoft.AspNetCore.Mvc;
using UnifiedPrintApi.Model;
using UnifiedPrintApi.Model.Get;
using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Interfaces.Generic;
using UnifiedPrintApi.Model.Post;
using UnifiedPrintApi.Service;

namespace UnifiedPrintApi.Controllers;

[ApiController]
[Route("[controller]")]
public class Saved : ControllerBase
{
    private Apis _apis;
    private Storage _storage;

    public Saved(Apis apis, Storage storage)
    {
        _apis = apis;
        _storage = storage;
    }
    
    [HttpGet("{token}")]
    public SavedToken GetSavedPosts(string token)
    {
        SaveStorage storage = _storage.GetSaveStorage(token);
        List<string> uids = new(storage.UIDs);
        uids.Reverse();
        
        return new()
        {
            CollectionName = storage.Name,
            Posts = uids.Select(x => _apis.GetUID(x, TimeSpan.FromDays(7))?.Generic() ?? null).Where(x => x != null).ToList()!
        };
    }
    
    [HttpGet("{token}/uids")]
    public SaveStorage GetSavedPostsUids(string token)
    {
        return _storage.GetSaveStorage(token);
    }

    [HttpPost]
    public string GetNewSaveToken(SavedNew data)
    {
        return _storage.CreateSaveStorage(data.CollectionName);
    }

    [HttpPost("{token}/add")]
    public string AddPostToSaves(string token, SavedTokenAdd data)
    {
        try
        {
            IApiPost? post = _apis.GetUID(data.UID, TimeSpan.FromDays(7));

            if (post == null)
            {
                Response.StatusCode = 404;
                return "Post not found";
            }

            _storage.AddToSaveStorage(token, post);
        }
        catch (Exception e)
        {
            Response.StatusCode = 400;
            return e.Message;
        }

        return "OK";
    }
    
    [HttpDelete("{token}/remove")]
    public string RemovePostFromSaves(string token, SavedTokenAdd data)
    {
        try
        {
            _storage.RemoveFromSaveStorage(token, data.UID);
        }
        catch (Exception e)
        {
            Response.StatusCode = 400;
            return e.Message;
        }
        
        return "OK";
    }
}