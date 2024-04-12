using Microsoft.AspNetCore.Mvc.Rendering;

namespace AuroraBricks.Models.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<AuroraBricks.Models.BrixProduct> Product { get; set; }
        
        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();
        public List<BrixProduct> Products { get; set; }
        public SelectList Categories { get; set; }
        public SelectList PrimaryColors { get; set; }
        public string SelectedCategory { get; set; }
        public string SelectedPrimaryColor { get; set; }
        

    }

}

