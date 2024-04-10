namespace AuroraBricks.Models;

public class EfBrixRepository : IBrixRepository
{
    private AbrixContext _context;
    
    public EfBrixRepository(AbrixContext temp)
    {
        _context = temp;
    }
    
    public List<BrixCustomer> Customers => _context.BrixCustomers.ToList();
    public List<BrixOrder> Orders => _context.BrixOrders.ToList();
    public List<BrixProduct> Products => _context.BrixProducts.ToList();
    public List<BrixLineItem> LineItems => _context.BrixLineItems.ToList();
}