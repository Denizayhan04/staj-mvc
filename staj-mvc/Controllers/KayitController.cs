using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using staj_mvc.Models;

namespace staj_mvc.Controllers;

public class KayitController : Controller
{
    private readonly string _connStr;

    public KayitController(IConfiguration config)
    {
        _connStr = config.GetConnectionString("DefaultConnection")!;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var liste = new List<Kayit>();

        using var conn = new MySqlConnection(_connStr);
        conn.Open();

        var cmd = new MySqlCommand("SELECT * FROM Kayitlar", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            liste.Add(new Kayit
            {
                Id       = reader.GetInt32("Id"),
                Tarih    = reader.GetDateTime("Tarih"),
                Deger    = reader.GetInt32("Deger"),
                Aciklama = reader.IsDBNull(reader.GetOrdinal("Aciklama")) ? null : reader.GetString("Aciklama")
            });
        }

        return View(liste);
    }

    // EKLE - Form göster
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
// DÜZENLE - Formu göster
    [HttpGet]
    public IActionResult Edit(int id)
    {
        Kayit? kayit = null;

        using var conn = new MySqlConnection(_connStr);
        conn.Open();

        var cmd = new MySqlCommand("SELECT * FROM Kayitlar WHERE Id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            kayit = new Kayit
            {
                Id       = reader.GetInt32("Id"),
                Tarih    = reader.GetDateTime("Tarih"),
                Deger    = reader.GetInt32("Deger"),
                Aciklama = reader.IsDBNull(reader.GetOrdinal("Aciklama")) ? null : reader.GetString("Aciklama")
            };
        }

        if (kayit == null)
            return NotFound();

        return View(kayit);
    }

// DÜZENLE - Kaydet
    [HttpPost]
    public IActionResult Edit(Kayit model)
    {
        if (!ModelState.IsValid)
            return View(model);

        using var conn = new MySqlConnection(_connStr);
        conn.Open();

        var cmd = new MySqlCommand(
            "UPDATE Kayitlar SET Tarih=@tarih, Deger=@deger, Aciklama=@aciklama WHERE Id=@id", conn);

        cmd.Parameters.AddWithValue("@tarih", model.Tarih);
        cmd.Parameters.AddWithValue("@deger", model.Deger);
        cmd.Parameters.AddWithValue("@aciklama", (object?)model.Aciklama ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@id", model.Id);

        cmd.ExecuteNonQuery();

        return RedirectToAction("Index");
    }
    // EKLE - Formu kaydet
    [HttpPost]
    public IActionResult Create(Kayit model)
    {
        if (!ModelState.IsValid)
            return View(model);

        using var conn = new MySqlConnection(_connStr);
        conn.Open();

        var cmd = new MySqlCommand(
            "INSERT INTO Kayitlar (Tarih, Deger, Aciklama) VALUES (@tarih, @deger, @aciklama)", conn);

        cmd.Parameters.AddWithValue("@tarih", model.Tarih);
        cmd.Parameters.AddWithValue("@deger", model.Deger);
        cmd.Parameters.AddWithValue("@aciklama", (object?)model.Aciklama ?? DBNull.Value);

        cmd.ExecuteNonQuery();

        return RedirectToAction("Index");
    }

    // SİL
    [HttpPost]
    public IActionResult Delete(int id)
    {
        using var conn = new MySqlConnection(_connStr);
        conn.Open();

        var cmd = new MySqlCommand("DELETE FROM Kayitlar WHERE Id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();

        return RedirectToAction("Index");
    }
}