using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations.Schema;

namespace VolApp.Models
{
    public class VolLigne
    {
        public int Id { get; set; }

        public int VolId { get; set; }

        public LineString Geom { get; set; }

        [ForeignKey("VolId")]
        public Vol Vol { get; set; }
    }
}
