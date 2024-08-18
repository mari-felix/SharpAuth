using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Context;
using System.Data.Entity;

namespace API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserContext _userContext;

        public UserController(UserContext userContext) {
            _userContext = userContext; 
        }
        
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CriarUsuario(User user) {
            try 
            {
                _userContext.Users.Add(user);
                await _userContext.SaveChangesAsync();
                return Ok(user);
            } 
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> ObterUsuario(int id) {
            try 
            {
                var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null) {
                    throw new Exception("Usuario não consta no distema");
                }

                return Ok(user);
            } 
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
    }
}