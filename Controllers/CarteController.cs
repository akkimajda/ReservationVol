using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VolApp.Data;

namespace VolApp.Controllers
{
    public class CarteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Vue principale de la carte
        public IActionResult Index()
        {
            return View();
        }

        // 🔁 API : Renvoie les lignes de vols au format GeoJSON
        [HttpGet]
        public IActionResult GetVolsGeoJson()
        {
            var geoJson = _context.VolLignes
    .Select(v => new
    {
        geometry = v.Geom.ToString(),
        properties = new
        {
            id = v.Id,
            vol_id = v.Vol_Id,
        }
    })
    .ToList();



            var features = geoJson.Select(f => new
            {
                type = "Feature",
                geometry = new
                {
                    type = "LineString",
                    coordinates = WktToCoordinates(f.geometry)
                },
                properties = f.properties
            });

            return Json(new
            {
                type = "FeatureCollection",
                features = features
            });
        }

        // 🔁 Fonction pour transformer du WKT en tableau de coordonnées
        private static List<List<double>> WktToCoordinates(string wkt)
        {
            var coordPart = wkt.Replace("LINESTRING(", "")
                               .Replace(")", "")
                               .Trim();

            return coordPart.Split(',')
                .Select(pair =>
                {
                    var nums = pair.Trim().Split(' ');
                    return new List<double>
                    {
                        double.Parse(nums[0], System.Globalization.CultureInfo.InvariantCulture),
                        double.Parse(nums[1], System.Globalization.CultureInfo.InvariantCulture)
                    };
                }).ToList();
        }
    }
}
