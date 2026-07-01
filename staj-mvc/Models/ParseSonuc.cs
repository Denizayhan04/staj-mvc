using staj_mvc.Dto;

namespace staj_mvc.Models;

public class ParseSonuc
{
    public string HamVeri { get; set; } = "";
    public List<SensorKayitDto> Gecerliler { get; set; } = new();
    public List<HataliSatir> Hataliler { get; set; } = new();
}