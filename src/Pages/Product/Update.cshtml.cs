using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class UpdateModel : PageModel
    {
        public UpdateModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        /// <summary>
        /// Backing service for product data operations.
        /// </summary>
        public JsonFileProductService ProductService { get; }

        /// <summary>
        /// Product being edited by the user.
        /// </summary>
        [BindProperty]
        public ProductModel Product { get; set; }

        /// <summary>
        /// Load the requested product for editing.
        /// </summary>
        public IActionResult OnGet(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectToPage("./Index");
            }

            Product = ProductService.GetProductById(id);

            if (Product == null)
            {
                return RedirectToPage("./Index");
            }

            return Page();
        }

        /// <summary>
        /// Persist the updated fighter details.
        /// </summary>
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = ProductService.UpdateData(Product);

            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Unable to update the fighter at this time. Please try again.");
                return Page();
            }

            return RedirectToPage("./Index", new { id = result.Id });
        }
    }
}
