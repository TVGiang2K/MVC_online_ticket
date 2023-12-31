using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_online_book_ticket.Models
{
    public class Trips 
    {
        [Key]
        public int TripsId { get; set; }

        [Display(Name = "Buses")]
        [ForeignKey("BusesId")]
        public int BusesId { get; set; }

        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string DepartureLocation { get; set; }

        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string DestinationLocation { get; set; }

        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string? Image {  get; set; }

        [DataType(DataType.Date)]
        public DateTime DepartureDateTime { get; set; }

        [DataType(DataType.Date)]
        public DateTime DestinationDateTime { get; set; }

        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string Distance { get; set; }

        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string RouteTrip { get; set; }

        public double BasePrice { get; set; }

        public virtual Buses? Buses { get; set; }
    }
}
