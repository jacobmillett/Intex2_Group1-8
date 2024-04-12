namespace AuroraBricks.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuroraBricks.Data;
using AuroraBricks.Models;
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
    public void AddOrder(BrixOrder order)
    {
        _context.Add(order);
        _context.SaveChanges();
    }
    public void AddLineItem(BrixLineItem lineItem)
    {
        _context.Add(lineItem);
        _context.SaveChanges();
    }

    public void EditProduct(BrixProduct product)
    {
        _context.BrixProducts.Update(product);
        _context.SaveChanges();
    }

    public void EditOrder(BrixOrder order)
    {
        _context.BrixOrders.Update(order);
        _context.SaveChanges();
    }

    public void RemoveOrder(BrixOrder order)
    {
        _context.BrixOrders.Remove(order);
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
    
    public BrixOrder GetLastOrder()
    {
        
        return _context.BrixOrders.OrderByDescending(x => x.TransactionId).FirstOrDefault();
    }
    
    public async Task<BrixCustomer> GetBrixCustomerByEmailAsync(string email)
    {
        return await _context.BrixCustomers.FirstOrDefaultAsync(c => c.Email == email);
    }

}

