using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ContactManager.Models;

public enum DegreeYear {
    First,
    Second,
    Third,
    Fourth,
    PostGrad
}

public class Member
{
    public long Id { get; set; }
    
    [Required]
    public Contact? Contact { get; set; }

    [MaxLength(100)]
    public string? Major { get; set; }

    [Required]
    public DateTime? SignUp { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DegreeYear? DegreeYear { get; set; }
}