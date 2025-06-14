using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VolApp.Models
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string? NomClient { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        public int VolId { get; set; }

        [ForeignKey("VolId")]
        public Vol? Vol { get; set; }


        public DateTime DateReservation { get; set; } = DateTime.UtcNow;
        public bool EstConfirmee { get; set; } = false;

        public string StatutMessage { get; set; } = "En attente de confirmation";


    }
}
