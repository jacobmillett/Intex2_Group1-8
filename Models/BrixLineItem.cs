using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuroraBricks.Models;

public partial class BrixLineItem
{
    [Key]
    public int TransactionId { get; set; }

    [Key]
    public int ProductId { get; set; }

    public int? Qty { get; set; }

    public int? Rating { get; set; }
}
