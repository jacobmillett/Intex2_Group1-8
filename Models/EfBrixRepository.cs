namespace AuroraBricks.Models;

public class EfBrixRepository : IBrixRepository
{
    private AbrixContext _context;
    
    public EfBrixRepository(AbrixContext temp)
    {
        _context = temp;
    }
    
    public IEnumerable<BrixCustomer> Customers => _context.BrixCustomers.ToList();
    public IEnumerable<BrixOrder> Orders => _context.BrixOrders.ToList();
    public IEnumerable<BrixProduct> Products => _context.BrixProducts.ToList();
    public IEnumerable<BrixLineItem> LineItems => _context.BrixLineItems.ToList();

    public void AddCustomer(BrixCustomer customer)
    {
        _context.Add(customer);
        _context.SaveChanges();
    }
    
    
    public BrixCustomer GetLastCustomer()
    {
        
        return _context.BrixCustomers.OrderByDescending(c => c.CustomerId).FirstOrDefault();
    }
}

