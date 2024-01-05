using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_online_book_ticket.Models
{
    public class Customers 
    {
        [Key]
        public int CustomersId { get; set; }

		[StringLength(100)]
		[Column(TypeName = "nvarchar(100)")]
		public string Name { get; set; }

        [
            DataType(DataType.PhoneNumber),
            RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "The phone number is not in the correct format 03 or 09 and 10 number"),
        ]
        [StringLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        public string Phone { get; set; }

        [EmailAddress(ErrorMessage = "Email address is not in correct format!")]
        [DataType(DataType.EmailAddress)]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }


        public byte Age { get; set; }

        public bool Gender { get; set; }
    }
}
