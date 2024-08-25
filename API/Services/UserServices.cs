using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using API.Context;
using API.Models;
using System.Net.Mail;

namespace API.Services
{
    public class UserServices
    {
        private readonly UserContext _userContext;
        public UserServices()
        {
            _userContext = new UserContext();
        }

        private bool IsValidEmail(string email)
        {
            try 
            {
                var endereco = new MailAddress(email);
                return endereco.Address == email;
            }
            catch 
            {
                return false;
            }
        }

        private bool IsValidPassword(string senha, string senhaDB) 
        {
           try 
            {
                return senha == senhaDB; 
            }
            catch 
            {
                return false;
            } 
        }

        
        private string HashPassword(string password) 
        {
            SHA256 sha256hash = SHA256.Create();

            byte[] data = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            string hashPassword = sb.ToString();
            return hashPassword;

        }

        private async Task<bool> CompararLog(string email, string password) 
        {
            try 
            {
                var usersDB = _userContext.Users;
                if (IsValidEmail(email)) 
                {
                   var user = await usersDB.FirstOrDefaultAsync(x => x.Email == email);
                   if (user != null) 
                   {
                        return IsValidPassword(password, user.Password);
                   } 
                   else
                   {
                        return false;
                   }
                }
                else 
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        } 

        public async Task<User> LogarUsuario(string email, string password) 
        {
            try
            {
                var usersDB = _userContext.Users;
                password = HashPassword(password);
                var log = await CompararLog(email, password);
                if (log == true)
                {
                    var user = await usersDB.FirstOrDefaultAsync(x => x.Email == email);
                    return user;
                } else {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task CriarUsuario(User user)
        {
            Random random = new Random();
            user.Id = random.Next(10000, 99999);

            if (IsValidEmail(user.Email))
            {
                
                user.Password = HashPassword(user.Password);
                user.CreatedAt = DateTime.Now;

                _userContext.Users.Add(user);
                await _userContext.SaveChangesAsync();
            }

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