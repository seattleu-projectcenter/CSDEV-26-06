using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class AddFighterModel : PageModel
    {
        public AddFighterModel(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        public JsonFileProductService ProductService { get; }


        [BindProperty]
        public ProductModel Product { get; set; }

        public IActionResult OnGet()
        {
            Product = new ProductModel();
            return Page();
        }

// On post. check fields and add to JSON, otherwise return error
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = ProductService.CreateData();
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Unable to add the fighter at this time. Please try again.");
                return Page();
            }

            Product.Id = result.Id;
            var finalResult = ProductService.UpdateData(Product);

            if (finalResult == null)
            {
                ModelState.AddModelError(string.Empty, "Unable to add the fighter at this time. Please try again.");
                return Page();
            }

            return RedirectToPage("./Index", new { id = finalResult.Id });
        }
    }
}


