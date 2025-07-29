using UserDirectory.Models;

namespace UserDirectory.Interfaces
{
    public interface IUserService
    {
        void AddUser(User user);
        bool DeleteUser(int id);
        List<User> GetAllUsers();
        User? GetUserById(int id);
        bool UpdateUser(int id, User user);
    }
}
