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