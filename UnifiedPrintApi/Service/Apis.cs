using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Service.Thingiverse;

namespace UnifiedPrintApi.Service;

public class Apis
{
    private ThingiverseApi _thingiverse;
    private Cache _cache;
    private List<IApiDescription> _apis;

    public Apis(ThingiverseApi thingiverse, Cache cache)
    {
        _thingiverse = thingiverse;
        _cache = cache;
        _apis = new()
        {
            _thingiverse
        };
    }

    public List<IApiDescription> GetApis() => _apis;
    public IApiDescription? GetApi(string slug) => GetApis().Find(x => x.Slug == slug);
    
    public IApiPost? GetUID(string uid)
    {
        string service = uid.Split(":")[0];
        string id = uid.Substring(service.Length + 1);
        
        IApiDescription? api = GetApis().Find(x => x.Slug == service);

        if (api == null)
            return null;

        string key = uid;
        return _cache.CacheValue(key, () => api.GetPostById(id), TimeSpan.FromHours(3));
    }
}