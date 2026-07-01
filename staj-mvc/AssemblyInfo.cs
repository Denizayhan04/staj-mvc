using Microsoft.Extensions.Localization;

// Proje/assembly adı "staj-mvc" (tire), kök namespace ise "staj_mvc" (alt çizgi).
// Lokalizör, attribute yoksa kaynak yolunu assembly adından türetir ve resx'leri bulamaz.
// Bu attribute ile doğru kök namespace bildirilir: staj_mvc.Resources.SharedResource.*
[assembly: RootNamespace("staj_mvc")]
