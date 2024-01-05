using System.Security.Cryptography;
using System.Text;

namespace MVC_online_book_ticket.Common
{
    public class HashPassword
    {
        public string SetPassword(string plainTextPassword)
        {
            // Tạo chuỗi (salt) ngẫu nhiên để thêm vào mật khẩu trước khi băm
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            // Kết hợp mật khẩu với chuỗi
            byte[] passwordBytes = Encoding.UTF8.GetBytes(plainTextPassword);
            byte[] saltedPassword = salt.Concat(passwordBytes).ToArray();
            // Băm mật khẩu
            using (var sha256 = new SHA256Managed())
            {
                byte[] hashedPassword = sha256.ComputeHash(saltedPassword);

                // Lưu trữ mật khẩu đã băm và chuỗi
                byte[] storedPassword = salt.Concat(hashedPassword).ToArray();
                return Convert.ToBase64String(storedPassword);
            }
        }

        public bool VerifyPassword(string plainTextPassword, string storedPassword)
        {
            

            // Chuyển đổi mật khẩu lưu trữ thành mảng byte
            byte[] checkStoredPassword = Convert.FromBase64String(storedPassword);

            // Tách chuỗi từ mật khẩu lưu trữ
            byte[] salt = checkStoredPassword.Take(16).ToArray();
            byte[] hashedPassword = checkStoredPassword.Skip(16).ToArray();

            // Kết hợp mật khẩu nhập vào với chuỗi
            byte[] passwordBytes = Encoding.UTF8.GetBytes(plainTextPassword);
            byte[] saltedPassword = salt.Concat(passwordBytes).ToArray();
            

            // Băm mật khẩu nhập vào
            using (var sha256 = new SHA256Managed())
            {
                byte[] hashedInputPassword = sha256.ComputeHash(saltedPassword);

                // So sánh mật khẩu đã băm với mật khẩu lưu trữ
                return hashedPassword.SequenceEqual(hashedInputPassword);
            }
        }
    }
}
