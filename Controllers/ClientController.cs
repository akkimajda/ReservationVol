using Microsoft.AspNetCore.Mvc;
using VolApp.Data;
using VolApp.Models;

namespace VolApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Accueil()
        {
            var vols = _context.Vols.ToList();
            return View(vols);
        }

        // GET: formulaire de réservation
        public IActionResult Reserver(int id)
        {
            var vol = _context.Vols.Find(id);
            if (vol == null) return NotFound();

            ViewBag.Vol = vol;
            return View();
        }

        // POST: enregistrement
        [HttpPost]
        public IActionResult Reserver(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                // ✅ Ne jamais forcer reservation.Id — PostgreSQL va le générer
                reservation.DateReservation = DateTime.UtcNow;

                _context.Reservations.Add(reservation);
                _context.SaveChanges();

                return RedirectToAction("Accueil");
            }

            ViewBag.Vol = _context.Vols.Find(reservation.VolId);
            return View(reservation);
        }
    }
}
