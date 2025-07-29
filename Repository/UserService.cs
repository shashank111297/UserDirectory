using System.Text.Json;
using UserDirectory.Interfaces;
using UserDirectory.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace UserDirectory.Repository
{
    public class UserService : IUserService
    {
        private readonly string _filePath;
        private readonly ILogger<UserService> _logger;
        private List<User> _users;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public UserService(IWebHostEnvironment env, ILogger<UserService> logger)
        {
            _logger = logger;
            _filePath = Path.Combine(env.ContentRootPath, "Data", "User.json");
            _users = new List<User>();
        }

        private async Task LoadDataAsync()
        {
            if (_users.Any()) return; // already loaded

            if (File.Exists(_filePath))
            {
                try
                {
                    var json = await File.ReadAllTextAsync(_filePath);
                    _users = JsonSerializer.Deserialize<List<User>>(json, _jsonOptions) ?? new List<User>();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to load user data.");
                    _users = new List<User>();
                }
            }
            else
            {
                await SaveChangesAsync(); // create file if not exists
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            await LoadDataAsync();
            return _users.AsReadOnly();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            await LoadDataAsync();
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", id);
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            return user;
        }

        public async Task AddUserAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            await LoadDataAsync();
            user.Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
            _users.Add(user);
            await SaveChangesAsync();
            _logger.LogInformation("User with ID {UserId} added.", user.Id);
        }

        public async Task<bool> UpdateUserAsync(int id, User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            await LoadDataAsync();
            var existingUser = _users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
            {
                _logger.LogWarning("Update failed: User with ID {UserId} not found.", id);
                return false;
            }

            existingUser.Name = user.Name;
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.Address = user.Address;
            existingUser.Phone = user.Phone;
            existingUser.Website = user.Website;
            existingUser.Company = user.Company;

            await SaveChangesAsync();
            _logger.LogInformation("User with ID {UserId} updated.", id);
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            await LoadDataAsync();
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                _logger.LogWarning("Delete failed: User with ID {UserId} not found.", id);
                return false;
            }

            _users.Remove(user);
            await SaveChangesAsync();
            _logger.LogInformation("User with ID {UserId} deleted.", id);
            return true;
        }

        private async Task SaveChangesAsync()
        {
            try
            {
                var dir = Path.GetDirectoryName(_filePath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var json = JsonSerializer.Serialize(_users, _jsonOptions);
                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save user data.");
                throw;
            }
        }
    }
}
