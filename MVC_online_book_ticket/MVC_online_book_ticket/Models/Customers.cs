using System.ComponentModel.DataAnnotations;

namespace MVC_online_book_ticket.Models
{
    public class Customers : General
    {
        [Key]
        public int CustomersId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public byte Age { get; set; }
        public bool Gender { get; set; }
    }
}
