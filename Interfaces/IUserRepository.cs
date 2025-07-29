using UserDirectory.Models;
using System.Collections.Generic;

namespace UserDirectory.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
    }
}
