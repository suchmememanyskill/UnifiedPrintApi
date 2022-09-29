using UnifiedPrintApi.Model.Interfaces;
using UnifiedPrintApi.Service.MMF;
using UnifiedPrintApi.Service.Printables;
using UnifiedPrintApi.Service.Thingiverse;

namespace UnifiedPrintApi.Service;

public class Apis
{
    private ThingiverseApi _thingiverse;
    private MMFApi _mmf;
    private PrintablesApi _printables;
    private Cache _cache;
    private List<IApiDescription> _apis;

    public Apis(ThingiverseApi thingiverse, MMFApi mmf, PrintablesApi printables, Cache cache)
    {
        _thingiverse = thingiverse;
        _cache = cache;
        _mmf = mmf;
        _printables = printables;
        _apis = new()
        {
            _thingiverse,
            _mmf,
            _printables
        };
    }

    public List<IApiDescription> GetApis() => _apis;
    public IApiDescription? GetApi(string slug) => GetApis().Find(x => x.Slug == slug);
    
    public IApiPost? GetUID(string uid, TimeSpan? cacheTime = null)
    {
        string service = uid.Split(":")[0];
        string id = uid.Substring(service.Length + 1);
        
        IApiDescription? api = GetApis().Find(x => x.Slug == service);

        if (api == null)
            return null;

        string key = uid;
        return _cache.CacheValue(key, () =>
        {
            try
            {
                return api.GetPostById(id);
            }
            catch
            {
                return null;
            }
        }, cacheTime ?? TimeSpan.FromHours(3));
    }
}