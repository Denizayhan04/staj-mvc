using Microsoft.AspNetCore.Mvc;
using staj_mvc.Dto;
using staj_mvc.Models;

namespace staj_mvc.Controllers;

public class ParseController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new ParseSonuc());
    }

    [HttpPost]
    public IActionResult Index(ParseSonuc form)
    {
        var sonuc = new ParseSonuc { HamVeri = form.HamVeri };

        var satirlar = form.HamVeri
            .Split('\n')
            .Select(s => s.Trim())
            .Where(s => !string.IsNullOrEmpty(s))
            .ToList();

        for (int i = 0; i < satirlar.Count; i++)
        {
            var satir = satirlar[i];
            var p = satir.Split(',');

            if (p.Length != 19)
            {
                sonuc.Hataliler.Add(new HataliSatir
                {
                    SatirNo = i + 1,
                    Icerik = satir,
                    HataMesaji = $"19 sütun olmalı, {p.Length} sütun bulundu"
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
                    Enlem               = double.Parse(p[5].Trim()),
                    Boylam              = double.Parse(p[6].Trim()),
                    Rakim               = int.Parse(p[7].Trim()),
                    RuzgarYonu          = double.Parse(p[8].Trim()),
                    RuzgarSiddeti       = double.Parse(p[9].Trim()),
                    UyduSayisi          = int.Parse(p[10].Trim()),
                    Direnc1             = double.Parse(p[11].Trim()),
                    Direnc2             = double.Parse(p[12].Trim()),
                    Pikofarad           = double.Parse(p[13].Trim()),
                    Gps                 = int.Parse(p[14].Trim()),
                    Glonass             = int.Parse(p[15].Trim()),
                    Galileo             = int.Parse(p[16].Trim()),
                    CipSicaklik         = double.Parse(p[17].Trim()),
                    AliciSinyalSeviyesi = int.Parse(p[18].Trim())
                });
            }
            catch (FormatException ex)
            {
                sonuc.Hataliler.Add(new HataliSatir
                {
                    SatirNo = i + 1,
                    Icerik = satir,
                    HataMesaji = $"Dönüştürme hatası: {ex.Message}"
                });
            }
        }

        return View(sonuc);
    }
}