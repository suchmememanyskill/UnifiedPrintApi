using Microsoft.AspNetCore.Mvc;
using UnifiedPrintApi.Service.Printables;

namespace UnifiedPrintApi.Controllers;

[ApiController]
[Route("printables")]
public class Printables : Controller
{
    private PrintablesApi _api;

    public Printables(PrintablesApi api)
    {
        _api = api;
    }

    [HttpGet("download")]
    public ActionResult Download(string fileId, string fileType, string postId)
    {
        string? link = _api.GetDownloadLink(fileType, fileId, postId);

        if (link == null)
        {
            return NotFound();
        }

        try
        {
            HttpClient client = new();
            Stream stream = client.GetStreamAsync(new Uri(link)).GetAwaiter().GetResult();
            return File(stream, "application/octet-stream", Path.GetFileName(link));
        }
        catch (Exception ex)
        {
            return BadRequest();
        }
    }
}