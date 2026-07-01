using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace staj_mvc.Controllers;

public class DilController : Controller
{
    [HttpGet]
    public IActionResult Degistir(string? culture, string? geriDon)
    {
        if (!string.IsNullOrEmpty(culture))
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
        }

        return Redirect(geriDon ?? "/");
    }
    
}