namespace staj_mvc.Models;

public class DovizResponse
{
    public string? Result { get; set; }
    public string? Base_Code { get; set; }
    public string? Time_Last_Update_Utc { get; set; }
    public Dictionary<string, double> Rates { get; set; } = new();
}

public class DovizSatir
{
    public string ParaBirimi { get; set; } = "";
    public double Kur { get; set; }
}