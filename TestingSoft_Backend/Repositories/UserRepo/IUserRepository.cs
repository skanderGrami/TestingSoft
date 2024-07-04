namespace TestingSoft_Backend.Repositories.UserRepo
{
    public interface IUserRepository
    {
       
        Task<User> GetUserById(int userId);
        Task<List<User>> GetAllUsers();
        Task<User> AddUser(User user);
        Task<User> UpdateUser(int userId ,User user);
        Task<bool> DeleteUser(int userId);
        Task<User> GetUserByEmail(string email);
        Task<LoginDto> Login(LoginDtoo loginDto);
        Task<User> Register(RegisterDto registerDto);
    }
}
