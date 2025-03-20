namespace twitterclone.DTOs;

public class UserSignInDots
{
    public required string FullName { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }
}


public class LoginDots
{
public required string Username{get;set;}

public required string Password{get;set;}

}

public class UserName{
    public string Username{get;set;}
}

public class UserProfileDto
{

    public bool Success { get; set; }

    public string? Message { get; set; }

    public ProfileDto? Profile{get;set;}

     public string? Weather{get;set;}

}

public class ProfileDto{
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
}