using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AuroraBricks.Models;
[Keyless]
public class UserRecommendation
{
    [Required]
    public int CustomerId { get; }
    public string Recommendation1 { get; }
    public string Recommendation2 { get; }
    public string Recommendation3 { get; }
    public string Recommendation4 { get; }
    public string Recommendation5 { get; }
}