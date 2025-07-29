using UserDirectory.Models;
using System.Collections.Generic;

namespace UserDirectory.Interfaces
{
    public interface IUserRepository
    {
        void AddUser(User user);
        bool DeleteUser(int id);
        List<User> GetAllUsers();
        User? GetUserById(int id);
        bool UpdateUser(int id, User user);
    }
}
