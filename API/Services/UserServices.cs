using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using API.Context;
using API.Models;

namespace API.Services
{
    public class UserServices
    {
        private readonly UserContext _userContext;
        public UserServices()
        {
            _userContext = new UserContext();
        }
        public async Task CriarUsuario(User user)
        {
            Random random = new Random();
            user.Id = random.Next(10000, 99999);

            using (SHA256 sha256hash = SHA256.Create()) 
            {
                byte[] data = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sb.Append(data[i].ToString("x2"));
                }
                string hashPassword = sb.ToString();
                user.Password = hashPassword;
            }

            _userContext.Users.Add(user);
            await _userContext.SaveChangesAsync();
        }

        public async Task<User> ObterUsuario(int id) 
        {
            var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) 
            {
                throw new Exception("Usuario não consta no sistema");
            }

            return user;
        } 
    }
}