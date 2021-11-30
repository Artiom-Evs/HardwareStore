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

        public CartModel(IStoreRepository repository)
        {
            this.repository = repository;
        }

        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        }

        public IActionResult OnPost(long productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            Cart.AddLine(product, 1);
            HttpContext.Session.SetJson("cart", Cart);

            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}
