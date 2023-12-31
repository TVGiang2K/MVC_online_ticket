using System.ComponentModel.DataAnnotations;

namespace MVC_online_book_ticket.Models.DTOs
{
	public class ForgetPasswordDTO
	{
		[EmailAddress(ErrorMessage = "Email address is not in correct format!")]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }
	}
}
