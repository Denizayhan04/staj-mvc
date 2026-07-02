
using System.ComponentModel.DataAnnotations;

namespace staj_mvc.Models;

public class Kayit
{
    [Key]
    public int Id { get; set; } //primary
    
    [Required]
    [DataType(DataType.Date)]
    public DateTime Tarih { get; set; }
    
    [Required]
    [Range(0,100000,ErrorMessage="0 ile 100000 bir sayı girin ")]
    public int Deger { get; set; }
    
    
    [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
    public string? Aciklama { get; set; }
}