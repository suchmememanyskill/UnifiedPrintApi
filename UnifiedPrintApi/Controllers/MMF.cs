using Microsoft.AspNetCore.Mvc;
using UnifiedPrintApi.Service;
using UnifiedPrintApi.Service.MMF;

namespace UnifiedPrintApi.Controllers;

[ApiController]
[Route("mmf")]
public class MMF : ControllerBase
{
    private MMFApi _api;

    public MMF(MMFApi api)
    {
        _api = api;
    }
    
    [HttpGet("{id}/download/{filename}")]
    public ActionResult GetMMFDownload(string id, string filename)
    {
        Storage.BaseUrl = $"{Request.Scheme}://{Request.Host.Value}";
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