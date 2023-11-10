using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace UnifiedPrintApi.Controllers;

[ApiController]
[Route("[controller]")]
public class Hacks : Controller
{
    [HttpGet("prusa")]
    public IActionResult OpenInPrusa(string url)
    {
        string redirect = $"prusaslicer://open?file={HttpUtility.UrlEncode(url)}";
        Console.WriteLine(redirect);
        return Redirect(redirect);
    }
}