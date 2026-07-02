using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using staj_mvc.Data;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// MVC ve Yerelleştirme servisleri ekleniyor
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

builder.Services.AddHttpClient();

// Veritabanı bağlantısı (MySQL / Pomelo)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<StajDbContext>(opts =>
    opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Kaynak dosyalarının aranacağı kök klasör belirtiliyor
builder.Services.AddLocalization(opts => opts.ResourcesPath = "Resources");

// Dil seçenekleri ve varsayılan dil ayarları
builder.Services.Configure<RequestLocalizationOptions>(opts =>
{
    var desteklenenDiller = new[]
    {
        new CultureInfo("tr-TR"),
        new CultureInfo("en-US")
    };

    opts.DefaultRequestCulture = new RequestCulture("tr-TR");
    opts.SupportedCultures = desteklenenDiller;
    opts.SupportedUICultures = desteklenenDiller;
    
    // Çerez (Cookie) sağlayıcısını ilk sıraya alarak tarayıcı seçimlerini önceliklendiriyoruz
    opts.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
});

var app = builder.Build();

// Veritabanı ve tablolar yoksa modellere göre otomatik oluşturulur
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StajDbContext>();
    db.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// --- HTTP İSTEK ADIMLARI (MIDDLEWARE) SIRALAMASI ---

app.UseRouting(); // 1. Önce gelen isteğin rotası belirlenir

app.UseRequestLocalization(); // 2. Rota belirlendikten sonra dil çerezi okunup uygulanır

app.UseAuthorization(); // 3. Kimlik doğrulama/yetkilendirme yapılır

// Rota haritası eşleştirilir
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();