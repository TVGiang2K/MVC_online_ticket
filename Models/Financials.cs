using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_online_book_ticket.Models
{
    public enum Category
    {
        Discount,
        Reimbursement
    } 
    public class Financials 
    {
        [Key]
        public int FinancialsId { get; set; }

        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string Title { get; set; }

        public int FinancialFrom { get; set; }

        public int FinancialTo { get; set; }


        public double Percentage { get; set; }

        public Category Category { get; set; }
    }
}
