using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_online_book_ticket.Models
{
    public class Buses 
    {
        [Key]
        public int BusesId { get; set; }

        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string BusNumber { get; set; }

        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string BusTypes { get; set; }

        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string LicensePlate { get; set; }


        public int SeatCapacity { get; set; }
    }
}
