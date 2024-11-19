using MongoDB.Driver;
using twitterclone.Interfaces;
using twitterclone.Models;
using twitterclone.DTOs;

namespace twitterclone.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("users");
        }

        public async Task<User> GetUserByUsernameOrEmailAsync(string username, string email)
        {
            
            return await _users.Find(u => u.Username == username || u.Email == email).FirstOrDefaultAsync();
            
        }

        public async Task<User>GetUserByUsername(string Username){
            return await _users.Find(u=>u.Username==Username).FirstOrDefaultAsync();
        }

        public async Task CreateUserAsync(User user){
             await _users.InsertOneAsync(user);
        }

        
    
    }
}
