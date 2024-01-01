using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_online_book_ticket.Models
{
	public class Agencies
	{
		[Key]
		public int AgenciesId { get; set; }

		[StringLength(200)]
		[Column(TypeName = "nvarchar(200)")]
		public string Address { get; set; }

		[
			DataType(DataType.PhoneNumber),
			RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "The phone number is not in the correct format 03 or 09 and 10 number"),
		]
		[StringLength(100)]
		[Column(TypeName = "nvarchar(50)")]
		public string Phone { get; set; }
	}
}
