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

    public void AddProduct(BrixProduct product)
    {
        _context.Add(product);
        _context.SaveChanges();
    }

    public void EditProduct(BrixProduct product)
    {
        _context.BrixProducts.Update(product);
        _context.SaveChanges();
    }

    public void RemoveProduct(BrixProduct product)
    {
        _context.BrixProducts.Remove(product);
        _context.SaveChanges();
    }

    public void EditUser(BrixCustomer customer)
    {
        _context.Update(customer);
        _context.SaveChanges();
    }

    public void RemoveUser(BrixCustomer customer)
    {
        _context.BrixCustomers.Remove(customer);
        _context.SaveChanges();
    }
    public BrixCustomer GetLastCustomer()
    {
        
        return _context.BrixCustomers.OrderByDescending(c => c.CustomerId).FirstOrDefault();
    }
    public BrixProduct GetLastProduct()
    {
        
        return _context.BrixProducts.OrderByDescending(x => x.ProductId).FirstOrDefault();
    }
}

