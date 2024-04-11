namespace AuroraBricks.Models;

public interface IBrixRepository
{
    public IEnumerable<BrixCustomer> Customers { get; }
    public IEnumerable<BrixOrder> Orders { get;  }
    public IEnumerable<BrixProduct> Products { get;  }
    public IEnumerable<BrixLineItem> LineItems { get; }
    
 
    Task<BrixCustomer> GetBrixCustomerByEmailAsync(string email);


    void AddCustomer(BrixCustomer customer);
    void AddProduct(BrixProduct product);
    void RemoveProduct(BrixProduct product);
    void EditProduct(BrixProduct product);

    void EditUser(BrixCustomer customer);
    void RemoveUser(BrixCustomer customer);
    BrixCustomer GetLastCustomer();

    BrixProduct GetLastProduct();
}