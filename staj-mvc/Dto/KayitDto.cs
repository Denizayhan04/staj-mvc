namespace staj_mvc.Dto;

public class SensorKayitDto
{
    public string? CihazId { get; set; }
    public int Versiyon { get; set; }
    public int PaketSayisi { get; set; }
    public string? GpsTarihi { get; set; }
    public int PilGucu { get; set; }
    public double Enlem { get; set; }
    public double Boylam { get; set; }
    public int Rakim { get; set; }
    public double RuzgarYonu { get; set; }
    public double RuzgarSiddeti { get; set; }
    public int UyduSayisi { get; set; }
    public double Direnc1 { get; set; }
    public double Direnc2 { get; set; }
    public double Pikofarad { get; set; }
    public int Gps { get; set; }
    public int Glonass { get; set; }
    public int Galileo { get; set; }
    public double CipSicaklik { get; set; }
    public int AliciSinyalSeviyesi { get; set; }
}