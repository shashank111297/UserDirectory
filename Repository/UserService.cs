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
