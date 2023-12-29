namespace MVC_online_book_ticket.Models
{
    public enum Category
    {
        Discount,
        Reimbursement
    } 
    public class Financial : General
    {
        public int FinancialId { get; set; }

        public string Title { get; set; }

        public int FinancialFrom { get; set; }

        public int FinancialTo { get; set; }

        public double Percentage { get; set; }

        public Category Category { get; set; }
    }
}
