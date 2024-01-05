namespace MVC_online_book_ticket.Models.DTOs
{
    public class ReservationsCancleDTO
    {
        public int TripsId { get; set; }
        public int CustomersId { get; set; }
        public int AccountsId { get; set; }
        public double TotalPrice { get; set;}
        public double RefundAmount { get; set;}
        public DateTime CancellationDateTime { get; set; }
        public byte ActiveReservation { get; set; }
    }
}
