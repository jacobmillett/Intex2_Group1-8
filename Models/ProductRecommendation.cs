using System.ComponentModel.DataAnnotations;

namespace AuroraBricks.Models;

public class ProductRecommendation
{
    [Key]
    public string? ProductName { get; set; }
    public string? Recommendation1 { get; set; }
    public string? Recommendation2 { get; set; }
    public string? Recommendation3 { get; set; }
    public string? Recommendation4 { get; set; }
    public string? Recommendation5 { get; set; }
}
