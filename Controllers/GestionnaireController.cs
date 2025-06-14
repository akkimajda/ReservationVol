using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using VolApp.Data;
using VolApp.Models;

namespace VolApp.Controllers
{
    public class GestionnaireController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GestionnaireController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var vols = _context.Vols.ToList(); // 🔁 lire depuis la base réelle
            return View(vols);
        }

        [HttpPost]
        public IActionResult AjouterVol(Vol vol)
        {
            if (ModelState.IsValid)
            {
                // ✅ Forcer les dates à UTC
                vol.DateDepart = DateTime.SpecifyKind(vol.DateDepart, DateTimeKind.Utc);
                vol.DateArrivee = DateTime.SpecifyKind(vol.DateArrivee, DateTimeKind.Utc);

                _context.Vols.Add(vol);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            return View("Dashboard", _context.Vols.ToList());
        }




        public IActionResult SupprimerVol(int id)
        {
            var vol = _context.Vols.Find(id);
            if (vol != null)
            {
                _context.Vols.Remove(vol);
                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard");
        }


        public IActionResult EditVol(int id)
        {
            var vol = _context.Vols.FirstOrDefault(v => v.Id == id);
            if (vol == null)
                return NotFound();

            return View(vol); // renvoie à la vue EditVol.cshtml
        }


        [HttpPost]
        public IActionResult EditVol(Vol vol)
        {
            if (ModelState.IsValid)
            {
                // ✅ Forcer les dates en UTC
                vol.DateDepart = DateTime.SpecifyKind(vol.DateDepart, DateTimeKind.Utc);
                vol.DateArrivee = DateTime.SpecifyKind(vol.DateArrivee, DateTimeKind.Utc);

                _context.Vols.Update(vol);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            return View(vol);
        }



        public IActionResult DetailsVol(int id)
        {
            var vol = _context.Vols.FirstOrDefault(v => v.Id == id);
            if (vol == null)
                return NotFound();

            return View(vol); // envoie vers la vue DetailsVol.cshtml
        }

        public IActionResult ListeReservations()
        {
            var reservations = _context.Reservations.Include(r => r.Vol).ToList();
            return View(reservations);
        }

        [HttpPost]
        public IActionResult Confirmer(int id)
        {
            var res = _context.Reservations.Find(id);
            if (res != null)
            {
                res.EstConfirmee = true;
                _context.SaveChanges();
            }
            return RedirectToAction("ListeReservations");
        }

        [HttpPost]
        public IActionResult Supprimer(int id)
        {
            var res = _context.Reservations.Find(id);
            if (res != null)
            {
                // facultatif : stocker une info d’annulation pour le client
                TempData["AnnuleeEmail"] = res.Email;
                _context.Reservations.Remove(res);
                _context.SaveChanges();
            }
            return RedirectToAction("ListeReservations");
        }













    }
}
