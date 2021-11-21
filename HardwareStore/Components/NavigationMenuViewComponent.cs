using Microsoft.AspNetCore.Mvc;
using System.Linq;
using HardwareStore.Models;

namespace HardwareStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IStoreRepository repository;

        public NavigationMenuViewComponent(IStoreRepository repository)
        {
            this.repository = repository;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repository.Products.Select(p => p.Category).Distinct().OrderBy(n => n));
        }
    }
}
