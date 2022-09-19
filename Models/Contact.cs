using System.ComponentModel.DataAnnotations;

namespace ContactManager.Models;

public class Contact
{
    public long Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string? FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string? LastName { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Email { get; set; }

}