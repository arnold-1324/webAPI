

namespace twitterclone.DTOs;

public class UserDTO
    {
        public required string Id { get; set; } 
        public required string Username { get; set; }
        public required string FullName { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsVerified { get; set; }
        public required string Email { get; set; }
         public List<string> Followers { get; set; } = new List<string>();
        public List<string> Following { get; set; } = new List<string>();
         public bool IsFrozen { get; set; }
        public string ProfileImg { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

public  class AuthResultDots
{
  public bool Success { get; set; }
    public string? Token { get; set; }
    public string? Message { get; set; }
    public UserDTO? User { get; set; }

}

