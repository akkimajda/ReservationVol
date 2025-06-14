using System.Collections.Generic;

namespace VolApp.Models
{
    public class DashboardViewModel
    {
        public List<Vol> Vols { get; set; } = new();  // Initialise avec une liste vide
        public List<Reservation> ReservationsConfirmees { get; set; } = new();  // Idem
    }
}
