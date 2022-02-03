using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using HardwareStore.Infrastructure;
using HardwareStore.Models;
using System.Linq;

namespace HardwareStore.Pages
{
    public class CartModel : PageModel
    {
        private IStoreRepository repository;
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }

        public CartModel(IStoreRepository repository, Cart cartService)
        {
            this.repository = repository;
            this.Cart = cartService;
        }

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
        }

        public IActionResult OnPost(long productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            Cart.AddLine(product, 1);
            
            return RedirectToPage(new { returnUrl });
        }
    }
}
