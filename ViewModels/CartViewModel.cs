﻿using AuroraBricks.Models;

namespace AuroraBricks.ViewModels
{
    public class CartViewModel
    {
        public Cart? Cart { get; set; }
        public string? ReturnUrl { get; set; }
        public decimal Total => Cart?.ComputeTotalValue() ?? 0;
        public BrixOrder Order { get; set; } = new BrixOrder();
        public ICollection<BrixLineItem> LineItems { get; set; } = new List<BrixLineItem>();
    }
}

