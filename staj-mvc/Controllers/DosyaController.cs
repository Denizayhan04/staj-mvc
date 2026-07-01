using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using staj_mvc.Dto;
using staj_mvc.Models;

namespace staj_mvc.Controllers;

public class DosyaController : Controller
{
    private readonly string _logPath;

    public DosyaController(IWebHostEnvironment env)
    {
        var logKlasor = Path.Combine(env.ContentRootPath, "logs");
        Directory.CreateDirectory(logKlasor);
        _logPath = Path.Combine(logKlasor, "app.log");
    }

    private void Log(string mesaj, string seviye = "INFO")
    {
        var satir = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{seviye}] {mesaj}";
        System.IO.File.AppendAllText(_logPath, satir + Environment.NewLine);
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new ParseSonuc());
    }

    [HttpPost]
    public async Task<IActionResult> Index(IFormFile? dosya)
    {
        var sonuc = new ParseSonuc();

        if (dosya == null || dosya.Length == 0)
        {
            ViewBag.Hata = "Lütfen bir dosya seçin.";
            return View(sonuc);
        }

        if (!dosya.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
        {
            ViewBag.Hata = "Sadece .txt dosyası yüklenebilir.";
            return View(sonuc);
        }

        var satirlar = new List<string>();

        using (var reader = new StreamReader(dosya.OpenReadStream()))
        {
            string? satir;
            while ((satir = await reader.ReadLineAsync()) != null)
            {
                if (!string.IsNullOrWhiteSpace(satir))
                    satirlar.Add(satir.Trim());
            }
        }

        Log($"Dosya yüklendi: {dosya.FileName}, {satirlar.Count} satır", "INFO");

        for (int i = 0; i < satirlar.Count; i++)
        {
            var satir = satirlar[i];
            var p = satir.Split(',');

            if (p.Length != 19)
            {
                var hata = $"Alan sayısı 19 değil, {p.Length} bulundu";
                Log($"Satır {i + 1}: hata - {hata}");
                sonuc.Hataliler.Add(new HataliSatir
                {
                    SatirNo = i + 1,
                    Icerik = satir,
                    HataMesaji = hata
                });
                continue;
            }

            try
            {
                sonuc.Gecerliler.Add(new SensorKayitDto
                {
                    CihazId             = p[0].Trim(),
                    Versiyon            = int.Parse(p[1].Trim()),
                    PaketSayisi         = int.Parse(p[2].Trim()),
                    GpsTarihi           = p[3].Trim(),
                    PilGucu             = int.Parse(p[4].Trim()),
                    Enlem               = double.Parse(p[5].Trim(), CultureInfo.InvariantCulture),
                    Boylam              = double.Parse(p[6].Trim(), CultureInfo.InvariantCulture),
                    Rakim               = int.Parse(p[7].Trim()),
                    RuzgarYonu          = double.Parse(p[8].Trim(), CultureInfo.InvariantCulture),
                    RuzgarSiddeti       = double.Parse(p[9].Trim(), CultureInfo.InvariantCulture),
                    UyduSayisi          = int.Parse(p[10].Trim()),
                    Direnc1             = double.Parse(p[11].Trim(), CultureInfo.InvariantCulture),
                    Direnc2             = double.Parse(p[12].Trim(), CultureInfo.InvariantCulture),
                    Pikofarad           = double.Parse(p[13].Trim(), CultureInfo.InvariantCulture),
                    Gps                 = int.Parse(p[14].Trim()),
                    Glonass             = int.Parse(p[15].Trim()),
                    Galileo             = int.Parse(p[16].Trim()),
                    CipSicaklik         = double.Parse(p[17].Trim(), CultureInfo.InvariantCulture),
                    AliciSinyalSeviyesi = int.Parse(p[18].Trim())
                });
                Log($"Satır {i + 1}: parse başarılı", "OK");
            }
            catch (FormatException ex)
            {
                Log($"Satır {i + 1}: hata - {ex.Message}", "ERROR");
                sonuc.Hataliler.Add(new HataliSatir
                {
                    SatirNo = i + 1,
                    Icerik = satir,
                    HataMesaji = ex.Message
                });
            }
        }

        Log($"İşlem tamamlandı: {sonuc.Gecerliler.Count} geçerli, {sonuc.Hataliler.Count} hatalı", "INFO");
        Log("---");

        ViewBag.DosyaAdi = dosya.FileName;
        return View(sonuc);
    }

    [HttpGet]
    public IActionResult LogGoster(string? filtre)
    {
        if (!System.IO.File.Exists(_logPath))
        {
            ViewBag.LogIcerik = "Henüz log kaydı yok.";
            ViewBag.Filtre = filtre;
            return View();
        }

        var satirlar = System.IO.File.ReadAllLines(_logPath).ToList();

        if (filtre == "ERROR")
            satirlar = satirlar.Where(s => s.Contains("[ERROR]")).ToList();
        else if (filtre == "OK")
            satirlar = satirlar.Where(s => s.Contains("[OK]")).ToList();
        else if (filtre == "INFO")
            satirlar = satirlar.Where(s => s.Contains("[INFO]")).ToList();

        ViewBag.LogIcerik = string.Join(Environment.NewLine, satirlar);
        ViewBag.Filtre = filtre ?? "TUMU";
        return View();
    }
}