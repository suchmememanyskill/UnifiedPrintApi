using Microsoft.AspNetCore.Mvc;
using UnifiedPrintApi.Service;
using UnifiedPrintApi.Service.MMF;

namespace UnifiedPrintApi.Controllers;

[ApiController]
[Route("mmf")]
public class MMF : ControllerBase
{
    private MMFApi _api;
    private Storage _storage;

    public MMF(MMFApi api, Storage storage)
    {
        _api = api;
        _storage = storage;
    }
    
    [HttpGet("{id}/download/{filename}")]
    public ActionResult GetMMFDownload(string id, string filename)
    {
        _storage.BaseUrl = $"{Request.Scheme}://{Request.Host.Value}"; // Hack
        try
        {
            return File(_api.GetDownloadFromPost(id, filename), "application/octet-stream", filename);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return BadRequest();
        }
    }
}