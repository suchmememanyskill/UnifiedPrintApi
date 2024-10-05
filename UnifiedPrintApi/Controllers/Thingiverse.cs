using Microsoft.AspNetCore.Mvc;
using UnifiedPrintApi.Service.Thingiverse;

namespace UnifiedPrintApi.Controllers;

[ApiController]
[Route("thingiverse")]
public class Thingiverse : ControllerBase
{
    private ThingiverseApi _api;
    private HttpClient _httpClient;

    public Thingiverse(ThingiverseApi api)
    {
        _api = api;
        _httpClient = new();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:131.0) Gecko/20100101 Firefox/131.0");
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

    [HttpGet("download_img/{*img}")]
    public async Task<IActionResult> ThingiverseImage(string img)
    {
        var response = await _httpClient.GetAsync($"https://cdn.thingiverse.com/assets/{img}");
        var content = await response.Content.ReadAsStreamAsync();
    
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        {
            return BadRequest();
        }
        
        return File(content, response.Content.Headers.ContentType.ToString(), img.Split("/").Last());
    }
}