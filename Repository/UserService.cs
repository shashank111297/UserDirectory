using System.Text.Json;
using UserDirectory.Interfaces;
using UserDirectory.Models;

namespace UserDirectory.Repository
{
    public class UserService : IUserRepository
    {

        private readonly string _filePath = "Data/users.json";
        private List<User> _users;

        public UserService()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            else
            {
                _users = new List<User>();
                SaveChanges();
            }
        }
        public List<User> GetAllUsers() => _users;

        public User? GetUserById(int id) => _users.FirstOrDefault(u => u.Id == id);

        public void AddUser(User user)
        {
            user.Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
            _users.Add(user);
            SaveChanges();
        }

        public bool UpdateUser(int id, User user)
        {
            var existingUser = GetUserById(id);
            if (existingUser == null) return false;

            existingUser.Name = user.Name;
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.Address = user.Address;
            existingUser.Phone = user.Phone;
            existingUser.Website = user.Website;
            existingUser.Company = user.Company;

            SaveChanges();
            return true;
        }

        public bool DeleteUser(int id)
        {
            var user = GetUserById(id);
            if (user == null) return false;

            _users.Remove(user);
            SaveChanges();
            return true;
        }

        private void SaveChanges()
        {
            var json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
            var dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.WriteAllText(_filePath, json);
        }
    }
}
