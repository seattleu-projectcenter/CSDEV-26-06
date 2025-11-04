using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class DeleteModel : PageModel
    {
        private readonly JsonFileProductService _productService;

        public DeleteModel(JsonFileProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public ProductModel Product { get; set; }

        public IActionResult OnGet(string id)
        {
            Product = _productService.GetProductById(id);
            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            _productService.DeleteProduct(Product.Id);
            return RedirectToPage("/Product/Index");
        }
    }
}
