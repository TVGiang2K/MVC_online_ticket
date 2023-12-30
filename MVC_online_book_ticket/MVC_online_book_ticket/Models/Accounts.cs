using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MVC_online_book_ticket.Models
{
    public enum Role
    {
        Administrator,
        Employees
    }
    public class Accounts 
    {
        [Key]
        public int AccountsId { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public byte Age { get; set; }
        public string Phone { get; set; }
        public bool Gender { get; set; }
        public string? Avatar { get; set; }
        public string Qualification { get; set; }
        public string Position { get; set; }
        public Role Role { get; set; }
    }
}
