using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Services;

namespace API.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userServices;
        public UserController() {
            _userServices = new UserServices(); 
        }
        
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CriarUsuario(User user) 
        {
            try 
            {
                await _userServices.CriarUsuario(user);
                return Ok(user);
            } 
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("LogInUser")]
        public async Task<IActionResult> LogarUsuario(string email, string password)
        {
            try 
            {
                var user = await _userServices.LogarUsuario(email, password);
                return Ok(user);  
            }
            catch (Exception ex)
            {
                return BadRequest("Não há usuário com esses dados: " + ex.Message);
            }
        }


        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> ObterUsuario(int id) 
        {
            try 
            {
                var user = await _userServices.ObterUsuario(id);
                return Ok(user);
            } 
            catch (Exception ex) 
            {
                return NotFound(ex.Message);
            }
        }
    }
}