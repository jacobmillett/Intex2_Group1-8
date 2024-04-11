using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuroraBricks.Models;

public partial class BrixCustomer
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Specify that the column is auto-incremented

    public int CustomerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? BirthDate { get; set; }

    public string? CountryOfResidence { get; set; }

    public string? Gender { get; set; }

    public double? Age { get; set; }

    public string? Email { get; set; }
}