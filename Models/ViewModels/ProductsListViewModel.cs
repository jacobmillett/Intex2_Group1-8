namespace AuroraBricks.Models.ViewModels
{
    public class ProductsListViewModel
    {
        public IEnumerable<AuroraBricks.Models.BrixProduct> Product { get; set; }
        
        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();
        public List<int> ItemsPerPageOptions { get; set; } = new List<int> { 10, 20, 50, 100 }; // Options for items per page.
        public int SelectedItemsPerPage { get; set; } // Current selection of items per page.


    }
}
