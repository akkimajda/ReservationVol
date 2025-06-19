using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace VolApp.Models
{
    public class VolLigne
{
    public int Id { get; set; }
    public int Vol_Id { get; set; } // ✅ Ce champ existe
    public LineString? Geom { get; set; }
}
}
