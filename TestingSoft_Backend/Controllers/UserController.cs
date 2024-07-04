using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestingSoft_Backend.Dtos;
using TestingSoft_Backend.Models;
using TestingSoft_Backend.Repositories.UserRepo;

namespace TestingSoft_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;


        public UserController(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return Ok(users);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            await _userRepository.UpdateUser(id,user);

            return NoContent();
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            await _userRepository.AddUser(user);

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userRepository.DeleteUser(id);

            return NoContent();
        }

        // POST: api/User/Register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (registerDto == null)
            {
                return BadRequest("Les données fournies pour l'enregistrement sont invalides.");
            }

            try
            {
                // Utilisez le dépôt d'utilisateurs pour enregistrer un nouvel utilisateur
                var user = await _userRepository.Register(registerDto);

                // Retournez une réponse CreatedAtAction avec le nouvel utilisateur
                return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
            }
            catch (Exception ex)
            {
                // Gérez les erreurs d'enregistrement
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST: api/User/Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDtoo loginDto)
        {
            // Vérifiez que les données fournies sont valides
            if (loginDto == null)
            {
                return BadRequest("Les données fournies pour l'authentification sont invalides.");
            }

            try
            {
                // Utilisez le dépôt d'utilisateurs pour authentifier un utilisateur
                var user = await _userRepository.Login(loginDto);

                // Si l'authentification est réussie, retournez les détails de l'utilisateur
               // var token = _tokenService.GenerateToken(user);
                // générer un token d'authentification ici Ki nest7a9ou
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Gérez les erreurs d'authentification
                return Unauthorized(new { error = ex.Message });
            }
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string providedPassword, string storedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, storedPassword);
        }
    }
}
