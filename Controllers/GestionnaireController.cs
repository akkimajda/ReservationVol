using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using VolApp.Data;
using VolApp.Models;

namespace VolApp.Controllers
{
    public class GestionnaireController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly Dictionary<string, (double lat, double lon)> villesCoords = new()
        {
    { "Casablanca", (33.5731, -7.5898) },
    { "Fès", (34.0331, -5.0003) },
    { "Rabat", (34.0209, -6.8417) },
    { "Marrakech", (31.6295, -7.9811) },
    { "Madrid", (40.4168, -3.7038) },
    { "Paris", (48.8566, 2.3522) },
    { "New York", (40.7128, -74.0060) },
    { "Dubai", (25.2048, 55.2708) }
        };




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
[HttpPost]
public IActionResult AjouterVol(Vol vol)
{
    if (ModelState.IsValid)
    {
        vol.DateDepart = DateTime.SpecifyKind(vol.DateDepart, DateTimeKind.Utc);
        vol.DateArrivee = DateTime.SpecifyKind(vol.DateArrivee, DateTimeKind.Utc);

        _context.Vols.Add(vol);
        _context.SaveChanges(); // ID du vol généré ici

        // 🔴 Bloc ajouté ici pour insérer la géométrie si les villes sont connues
        if (villesCoords.TryGetValue(vol.Depart, out var coordDepart) &&
            villesCoords.TryGetValue(vol.Destination, out var coordDestination))
        {
            string wkt = $"LINESTRING({coordDepart.lon} {coordDepart.lat}, {coordDestination.lon} {coordDestination.lat})";

            try
            {
                string sql = @"
                    INSERT INTO vols_lignes (vol_id, geom)
                    VALUES ({0}, ST_GeomFromText({1}, 4326));
                ";

                _context.Database.ExecuteSqlRaw(sql, vol.Id, wkt);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur WKT: " + wkt);
                Console.WriteLine("Exception : " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Ville non trouvée : " + vol.Depart + " ou " + vol.Destination);
        }

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
