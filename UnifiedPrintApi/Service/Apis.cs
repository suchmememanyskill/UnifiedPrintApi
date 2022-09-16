using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Service.MMF;
using UnifiedPrintApi.Service.Thingiverse;

namespace UnifiedPrintApi.Service;

public class Apis
{
    private ThingiverseApi _thingiverse;
    private MMFApi _mmf;
    private Cache _cache;
    private List<IApiDescription> _apis;

    public Apis(ThingiverseApi thingiverse, MMFApi mmf, Cache cache)
    {
        _thingiverse = thingiverse;
        _cache = cache;
        _mmf = mmf;
        _apis = new()
        {
            _thingiverse,
            _mmf
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