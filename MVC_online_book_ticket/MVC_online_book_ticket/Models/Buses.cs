using System.ComponentModel.DataAnnotations;

namespace MVC_online_book_ticket.Models
{
    public class Buses 
    {
        [Key]
        public int BusesId { get; set; }
        public string BusNumber { get; set; }
        public string BusTypes { get; set; }
        public string LicensePlate { get; set; }
        public int SeatCapacity { get; set; }
    }
}
