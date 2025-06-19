using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using VolApp.Data;
using VolApp.Models;

namespace VolApp.Controllers
{
    public class CarteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarteController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(); // Affiche la carte
        }

        [HttpGet]
        public IActionResult GetVolsGeoJson()
        {
            var lignes = _context.VolLignes
                .Include(vl => vl.Vol)
                .ToList();

            var geojson = new
            {
                type = "FeatureCollection",
                features = lignes.Select(vl => new
                {
                    type = "Feature",
                    geometry = new
                    {
                        type = "LineString",
                        coordinates = vl.Geom.Coordinates.Select(c => new[] { c.X, c.Y })
                    },
                    properties = new
                    {
                        depart = vl.Vol.Depart,
                        destination = vl.Vol.Destination,
                        date_vol = vl.Vol.DateDepart.ToString("yyyy-MM-dd HH:mm"),
                        places = vl.Vol.PlacesDisponibles
                    }
                })
            };

            return Json(geojson);
        }
    }
}
