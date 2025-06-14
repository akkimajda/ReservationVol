using Microsoft.AspNetCore.Mvc;

namespace VolApp.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
public IActionResult Login(string email, string password)
{
    if (email.ToLower().Contains("@rarabia.com"))
    {
        // Rediriger vers la page du gestionnaire
        return RedirectToAction("Dashboard", "Gestionnaire");
    }
    else
    {
        // Rediriger vers la page du client
        return RedirectToAction("Accueil", "Client");
    }
}


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}
