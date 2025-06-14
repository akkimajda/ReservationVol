namespace VolApp.Models
{
    public class Vol
    {
        public int Id { get; set; }
        public string? Depart { get; set; }
        public string? Destination { get; set; }
        public DateTime DateDepart { get; set; }
        public DateTime DateArrivee { get; set; }
        public decimal Prix { get; set; }
        public int PlacesDisponibles { get; set; }
    }
}
