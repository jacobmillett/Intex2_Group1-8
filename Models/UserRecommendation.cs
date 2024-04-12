using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AuroraBricks.Models;
[Keyless]
public class UserRecommendation
{
    [Required]
    public int CustomerId { get; set; }
    public string Product1 { get; set; }
    public string Product2 { get; set; }
    public string Product3 { get; set; }
    public string Product4 { get; set; }
    public string Product5 { get; set; }
}