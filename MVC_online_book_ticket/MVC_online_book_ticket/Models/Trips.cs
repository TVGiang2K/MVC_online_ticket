using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_online_book_ticket.Models
{
    public class Trips : General
    {
        [Key]
        public int TripsId { get; set; }

        [ForeignKey("BusesId")]
        public int BusesId { get; set; }

        public string DepartureLocation { get; set; }

        public string DestinationLocation { get; set; }

        public DateTime DepartureDateTime { get; set; }

        public DateTime DestinationDateTime { get; set; }

        public string Distance { get; set; }
        public string RouteTrip { get; set; }
        public double BasePrice { get; set; }

        public virtual Buses Buses { get; set; }
    }
}
