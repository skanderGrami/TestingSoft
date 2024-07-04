using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using TestingSoft_Backend.Dtos;
using TestingSoft_Backend.Models;

namespace TestingSoft_Backend.Repositories.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly TestingSoftContext _context;
        private readonly ITokenService _tokenService;
        public UserRepository(TestingSoftContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        public async Task<User> AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User> UpdateUser(int userId, User user)
        {
            if (userId != user.UserId)
            {
                throw new ArgumentException("Id mismatch");
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(userId))
                {
                    throw new KeyNotFoundException($"User with ID {userId} not found");
                }
                else
                {
                    throw;
                }
            }

            return user;
        }

        
        public async Task<User> Register(RegisterDto registerDto)
        {
            // Vérifiez si l'utilisateur existe déjà
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException($"A user with the email '{registerDto.Email}' already exists.");
            }

            // Vérifiez que toutes les valeurs de RegisterDto sont présentes
            if (string.IsNullOrWhiteSpace(registerDto.FirstName) ||
                string.IsNullOrWhiteSpace(registerDto.LastName) ||
                string.IsNullOrWhiteSpace(registerDto.Email) ||
                string.IsNullOrWhiteSpace(registerDto.Password) ||
                string.IsNullOrWhiteSpace(registerDto.Role))
            {
                throw new ArgumentException("All fields in Register must be provided.");
            }

            Console.WriteLine($"RegisterDto - FirstName: {registerDto.FirstName}, LastName: {registerDto.LastName}, Email: {registerDto.Email}, Password: {registerDto.Password}, Role: {registerDto.Role}");

            // Créez un nouvel utilisateur avec les données fournies
            var newUser = new User
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                Password = HashPassword(registerDto.Password),
                Role = registerDto.Role
            };

            // Ajoutez l'utilisateur à la base de données
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            
            Console.WriteLine($"NewUser - FirstName: {newUser.FirstName}, LastName: {newUser.LastName}, Email: {newUser.Email}, Password: {newUser.Password}, Role: {newUser.Role}");

            return newUser;
        }

        public async Task<LoginDto> Login(LoginDtoo loginDto)
        {
            User getuser = await GetUserByEmail(loginDto.Email);
            if (getuser == null)
            {
                throw new Exception("Invalid credentials");
            }

            // Vérifiez le mot de passe haché
           /* if (!VerifyPassword(loginDto.Password, getuser.Password))
            {
                throw new Exception("Invalid credentials");
            }*/

            // Créez un token JWT pour l'utilisateur
            string token = _tokenService.GenerateToken(getuser);
           
            return new LoginDto
            {
                user_Id = getuser.UserId,
                Email = loginDto.Email,
                Password = loginDto.Password,
                Token = token

            };
        }

       
        // Méthodes pour hacher et vérifier les mots de passe
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(u => u.UserId == id);
        }

       
    }

       
}
