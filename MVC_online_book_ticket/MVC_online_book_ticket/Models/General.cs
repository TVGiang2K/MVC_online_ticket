namespace MVC_online_book_ticket.Models
{
    public class General
    {
        public DateTime Create_date { get; set; }
        public DateTime? Update_date { get; set; }
        public DateTime? Delete_date { get; set; }
        public StatusState Status { get; set; }
    }
    public enum StatusState
    {
        InActive,
        Active
    }
}
