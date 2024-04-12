namespace AuroraBricks.Models;

public interface IBrixRepository
{
    public IEnumerable<BrixCustomer> Customers { get; }
    public IEnumerable<BrixOrder> Orders { get;  }
    public IEnumerable<BrixProduct> Products { get;  }
    public IEnumerable<BrixLineItem> LineItems { get; }
    public IEnumerable<ProductRecommendation> ProductRecommendations { get; }
    public IEnumerable<UserRecommendation> UserRecommendations { get; }
 
    Task<BrixCustomer> GetBrixCustomerByEmailAsync(string email);
    Task<UserRecommendation> GetCustomerRecommendationByCustomerIdAsync(int id);
    Task<BrixProduct> GetRecommendation1Async(string name);
    Task<BrixProduct> GetRecommendation2Async(string name);
    Task<BrixProduct> GetRecommendation3Async(string name);
    Task<BrixProduct> GetRecommendation4Async(string name);
    Task<BrixProduct> GetRecommendation5Async(string name);
    
    void AddCustomer(BrixCustomer customer);
    void AddProduct(BrixProduct product);
    void AddOrder(BrixOrder order);

    void AddLineItem(BrixLineItem lineItem);
    void RemoveProduct(BrixProduct product);
    void EditProduct(BrixProduct product);
    
    void EditUser(BrixCustomer customer);
    void RemoveUser(BrixCustomer customer);

    void RemoveOrder(BrixOrder order);
    BrixCustomer GetLastCustomer();

    BrixProduct GetLastProduct();
    
    BrixOrder GetLastOrder();
}