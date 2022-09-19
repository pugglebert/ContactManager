using System.ComponentModel.DataAnnotations;

namespace ContactManager.Models;

public class Account
{
    public Account(string email, string password)
    {
        this.Email = email;
        this.Password = password;
    }
    
    public long Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Email { get; set; }

    [Required]
    [MaxLength(100)]
    public string Password { get; set; }
}