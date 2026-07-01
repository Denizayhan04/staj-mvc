using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Nodes;
using staj_mvc.Models;

namespace staj_mvc.Controllers;

public class AyarlarController : Controller
{
    private readonly IConfiguration _config;

    public AyarlarController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var model = new Ayarlar();
        _config.GetSection("Ayarlar").Bind(model);
        return View(model);
    }

    [HttpPost]
    public IActionResult Index(Ayarlar model)
    {
        string jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        string jsonText = System.IO.File.ReadAllText(jsonPath);
        JsonNode doc = JsonNode.Parse(jsonText)!;

        doc["Ayarlar"]!["IstasyonKodu"] = model.IstasyonKodu;
        doc["Ayarlar"]!["PortAdi"] = model.PortAdi;
        doc["Ayarlar"]!["SaatDilimi"] = model.SaatDilimi;
        doc["Ayarlar"]!["SonPatch"] = model.SonPatch;
        doc["Ayarlar"]!["DefaultEmail"] = model.DefaultEmail;

        System.IO.File.WriteAllText(jsonPath, doc.ToJsonString(new JsonSerializerOptions
        {
            WriteIndented = true
        }));

        return RedirectToAction("Index");
    }
}