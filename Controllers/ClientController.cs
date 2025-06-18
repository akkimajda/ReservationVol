using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using VolApp.Data;
using VolApp.Models;
using System;
using Microsoft.EntityFrameworkCore; // tout en haut


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

    // Créer un modèle vide sans ID
    var reservation = new Reservation
    {
        VolId = vol.Id
    };

    return View(reservation);
}



        // POST: enregistrement
        [HttpPost]
        public IActionResult Reserver(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                reservation.DateReservation = DateTime.UtcNow;
                _context.Reservations.Add(reservation);
                _context.SaveChanges();

                return RedirectToAction("Reserver", new { id = reservation.VolId });
            }

            ViewBag.Vol = _context.Vols.Find(reservation.VolId);
            return View(reservation);
        }

        public IActionResult MesReservations()
{
    var reservations = _context.Reservations
        .Include(r => r.Vol)
        .ToList();

    return View(reservations);
}



        [HttpPost]
        public IActionResult AnnulerReservation(int id)
        {
            var reservation = _context.Reservations.Find(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
                _context.SaveChanges();
            }
            return RedirectToAction("MesReservations");
        }
    }
}
