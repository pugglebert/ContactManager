namespace ContactManager.DTO;

public class AuthResponse
{
    public AuthResponse (long id, string email, string token)
    {
        this.Id = id;
        this.Email = email;
        this.Token = token;
    }
    public long Id { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}