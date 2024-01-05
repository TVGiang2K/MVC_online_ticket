namespace MVC_online_book_ticket.Models.DTOs
{
    public class ChangePasswordDTO
    {
        public int AccountsId { get; set; }
		public string NewPassword { get; set; }
		public string ConfirmPassword { get; set; }
	}
}
