using Microsoft.AspNetCore.Mvc;
using UnifiedPrintApi.Service.Thingiverse;

namespace UnifiedPrintApi.Controllers;

[ApiController]
[Route("thingiverse")]
public class Thingiverse : ControllerBase
{
    private ThingiverseApi _api;

    public Thingiverse(ThingiverseApi api)
    {
        _api = api;
    }
    
    [HttpGet("download/{id}")]
    public ActionResult GetThingiverseDownload(string id, string filename)
    {
        try
        {
            return File(_api.GetPostDownload(id), "application/octet-stream", filename);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return BadRequest();
        }
    }
}