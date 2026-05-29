using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ApprenticeshipManagement.Models;

namespace ApprenticeshipManagement.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>System workflow diagram for internship report (opens in browser).</summary>
    public IActionResult Diagram()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "diagram.html");
        return PhysicalFile(path, "text/html");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
