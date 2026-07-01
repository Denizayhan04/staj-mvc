using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using staj_mvc.Models;

namespace staj_mvc.Controllers;

public class DovizController : Controller
{
    private readonly HttpClient _http;

    public DovizController(IHttpClientFactory factory)
    {
        _http = factory.CreateClient();
    }

    private static readonly string[] SecilenKurlar = 
        { "TRY", "EUR", "GBP", "JPY", "CAD", "AUD", "CHF", "CNY" };

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var json = await _http.GetStringAsync("https://open.er-api.com/v6/latest/USD");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<DovizResponse>(json, options);

            if (data == null || data.Result != "success")
            {
                ViewBag.Hata = "API'den geçerli veri alınamadı.";
                return View(new List<DovizSatir>());
            }

            var liste = data.Rates
                .Where(r => SecilenKurlar.Contains(r.Key))
                .Select(r => new DovizSatir { ParaBirimi = r.Key, Kur = r.Value })
                .OrderBy(r => r.ParaBirimi)
                .ToList();

            ViewBag.GuncellemeSaati = data.Time_Last_Update_Utc;
            return View(liste);
        }
        catch (HttpRequestException)
        {
            ViewBag.Hata = "API'ye bağlanılamadı. İnternet bağlantınızı kontrol edin.";
            return View(new List<DovizSatir>());
        }
        catch (JsonException)
        {
            ViewBag.Hata = "API'den gelen veri okunamadı.";
            return View(new List<DovizSatir>());
        }
    }
}