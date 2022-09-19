namespace ContactManager.DTO;

public class AuthRequest
{
    public AuthRequest (string email, string password)
    {
        this.Email = email;
        this.Password = password;
    }
    public string Email { get; set; }
    public string Password { get; set; }
}