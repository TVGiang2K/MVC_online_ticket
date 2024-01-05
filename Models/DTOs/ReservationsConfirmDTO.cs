namespace MVC_online_book_ticket.Models.DTOs
{
    public class ReservationsConfirmDTO
    {
        public int TripsId { get; set; }
        public int CustomersId { get; set; }
        public int AccountsId { get; set; }
        public double Vat {  get; set; }
        public double TotalPrice { get; set;}
        public DateTime ReservationDateTime { get; set; }
        public byte ActiveReservation { get; set; }
    }
}
