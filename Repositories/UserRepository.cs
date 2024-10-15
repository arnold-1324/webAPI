using MongoDB.Driver;
using twitterclone.Interfaces;
using twitterclone.Models;

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
    }
}
