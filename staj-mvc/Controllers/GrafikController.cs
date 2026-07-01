using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using staj_mvc.Models;

namespace staj_mvc.Controllers;

public class GrafikController : Controller
{
    private readonly HttpClient _http;

    public GrafikController(IHttpClientFactory factory)
    {
        _http = factory.CreateClient();
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Veri()
    {
        try
        {
            var json = await _http.GetStringAsync("https://open.er-api.com/v6/latest/USD");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<DovizResponse>(json, options);

            if (data == null || data.Result != "success")
                return Json(new { hata = "API'den veri alınamadı." });

            var secilenKurlar = new[] { "TRY", "EUR", "GBP", "CAD", "AUD", "CHF", "CNY" };

            var kurlar = secilenKurlar
                .Where(k => data.Rates.ContainsKey(k))
                .Select(k => new { paraBirimi = k, kur = data.Rates[k] })
                .ToList();

            return Json(new
            {
                labels = kurlar.Select(k => k.paraBirimi),
                values = kurlar.Select(k => k.kur),
                guncelleme = data.Time_Last_Update_Utc
            });
        }
        catch (Exception ex)
        {
            return Json(new { hata = ex.Message });
        }
    }
}