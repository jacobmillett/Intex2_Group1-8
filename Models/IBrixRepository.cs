namespace AuroraBricks.Models;

public interface IBrixRepository
{
    public List<BrixCustomer> Customers { get; }
    public List<BrixOrder> Orders { get;  }
    public List<BrixProduct> Products { get;  }
    public List<BrixLineItem> LineItems { get; }
}