using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MVC_online_book_ticket.Models
{
	public class Reservations
	{
		[Key]
		public int ReservationsId { get; set; }

		[ForeignKey("CustomersId")]
		public int CustomersId { get; set; }

		[ForeignKey("TripsId")]
		public int TripsId { get; set; }

		[ForeignKey("AccountsId")]
		public string AccountsId { get; set; }

		public double TotalPrice { get; set; }

		public DateTime ReservationDateTime { get; set; }

		public byte ActiveReservation { get; set; }

		public double? RefundAmount { get; set; }

		public DateTime? CancellationDateTime { get; set; }

		public virtual Customers Customers { get; set; }

		public virtual Trips Trips { get; set; }

		public double Vat { get; set; }

		public virtual Accounts Accounts { get; set; }
	}
}
