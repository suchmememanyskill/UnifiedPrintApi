using Newtonsoft.Json;
using UnifiedPrintApi.Model;
using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Model.Interfaces.Generic;

namespace UnifiedPrintApi.Service;

public class Storage
{
    private Cache _cache;
    private Dictionary<string, GenericApiPost> _permaCachedPosts;

    public Storage(Cache cache)
    {
        _cache = cache;

        _permaCachedPosts = Load<Dictionary<string, GenericApiPost>>("cachedPosts.json") ?? new();
        foreach (var (key, value) in _permaCachedPosts)
        {
            _cache.AddCacheValue(key, value, null);
        }
    }

    public void AddPostToCache(IApiPost post)
    {
        if (_permaCachedPosts.ContainsKey(post.UniversalId))
            return;
        
        _permaCachedPosts.Add(post.UniversalId, post.Generic());
        _cache.AddCacheValue(post.UniversalId, post.Generic(), null);
        Save("cachedPosts.json", _permaCachedPosts);
    }

    public string CreateSaveStorage(string name)
    {
        string id = Guid.NewGuid().ToString();
        string filename = $"{id}.json";
        Save(filename, new SaveStorage(){Name = name});
        return id;
    }

    public SaveStorage GetSaveStorage(string id)
    {
        string filename = $"{id}.json";
        SaveStorage? storage = Load<SaveStorage>(filename);
        if (storage == null)
            throw new Exception("Invalid id");

        return storage;
    }

    public void AddToSaveStorage(string id, IApiPost post)
    {
        string filename = $"{id}.json";
        SaveStorage? storage = Load<SaveStorage>(filename);
        if (storage == null)
            throw new Exception("Invalid id");

        if (storage.UIDs.Contains(post.UniversalId))
            throw new Exception("Collection already contains post");
        
        storage.UIDs.Add(post.UniversalId);
        Save(filename, storage);
        AddPostToCache(post);
    }

    public void RemoveFromSaveStorage(string id, string uid)
    {
        string filename = $"{id}.json";
        SaveStorage? storage = Load<SaveStorage>(filename);
        if (storage == null)
            throw new Exception("Invalid id");

        storage.UIDs.Remove(uid);
        Save(filename, storage);
    }
    
    public static T? Load<T>(string name)
    {
        string dir = "storage";
        string path = Path.Join(dir, name);

        if (!File.Exists(path))
            return default;

        return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
    }

    public static void Save(string name, object data)
    {
        string dir = "storage";
        string path = Path.Join(dir, name);

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        File.WriteAllText(path, JsonConvert.SerializeObject(data));
    }
    
    public string? BaseUrl { get; set; }
}