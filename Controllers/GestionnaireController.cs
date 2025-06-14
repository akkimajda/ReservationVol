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

        // PAGE DASHBOARD
        public IActionResult Dashboard()
        {
            var model = new DashboardViewModel
            {
                Vols = _context.Vols.ToList(),
                ReservationsConfirmees = _context.Reservations
                    .Include(r => r.Vol)
                    .Where(r => r.EstConfirmee)
                    .ToList()
            };

            return View(model);
        }

        // AJOUT VOL
        [HttpPost]
        public IActionResult AjouterVol(Vol vol)
        {
            if (ModelState.IsValid)
            {
                vol.DateDepart = DateTime.SpecifyKind(vol.DateDepart, DateTimeKind.Utc);
                vol.DateArrivee = DateTime.SpecifyKind(vol.DateArrivee, DateTimeKind.Utc);

                _context.Vols.Add(vol);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            return RedirectToAction("Dashboard");
        }

        // SUPPRIMER VOL
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

        // MODIFIER VOL (GET)
        public IActionResult EditVol(int id)
        {
            var vol = _context.Vols.FirstOrDefault(v => v.Id == id);
            return vol == null ? NotFound() : View(vol);
        }

        // MODIFIER VOL (POST)
        [HttpPost]
        public IActionResult EditVol(Vol vol)
        {
            if (ModelState.IsValid)
            {
                vol.DateDepart = DateTime.SpecifyKind(vol.DateDepart, DateTimeKind.Utc);
                vol.DateArrivee = DateTime.SpecifyKind(vol.DateArrivee, DateTimeKind.Utc);

                _context.Vols.Update(vol);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }

            return View(vol);
        }

        // DÉTAILS
        public IActionResult DetailsVol(int id)
        {
            var vol = _context.Vols.FirstOrDefault(v => v.Id == id);
            return vol == null ? NotFound() : View(vol);
        }

        // LISTE DES RÉSERVATIONS EN ATTENTE
        public IActionResult ListeReservations()
        {
            var reservations = _context.Reservations.Include(r => r.Vol).ToList();
            return View(reservations);
        }

        // CONFIRMER UNE RÉSERVATION
        [HttpPost]
        public IActionResult Confirmer(int id)
        {
            var res = _context.Reservations.Find(id);
            if (res != null)
            {
                res.EstConfirmee = true;
                res.StatutMessage = "Votre réservation a été confirmée.";
                _context.SaveChanges();
            }
            return RedirectToAction("ListeReservations");
        }



        // SUPPRIMER UNE RÉSERVATION
        [HttpPost]
        public IActionResult Supprimer(int id)
        {
            var res = _context.Reservations.Find(id);
            if (res != null)
            {
                TempData["AnnuleeEmail"] = res.Email;
                TempData["StatutAnnule"] = "Votre réservation a été annulée. Veuillez en effectuer une nouvelle.";
                _context.Reservations.Remove(res);
                _context.SaveChanges();
            }
            return RedirectToAction("ListeReservations");
        }


    }
}
