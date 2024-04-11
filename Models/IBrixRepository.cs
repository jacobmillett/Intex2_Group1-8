namespace AuroraBricks.Models;

public interface IBrixRepository
{
    public IEnumerable<BrixCustomer> Customers { get; }
    public IEnumerable<BrixOrder> Orders { get;  }
    public IEnumerable<BrixProduct> Products { get;  }
    public IEnumerable<BrixLineItem> LineItems { get; }
    
 


    void AddCustomer(BrixCustomer customer);

    BrixCustomer GetLastCustomer();
}