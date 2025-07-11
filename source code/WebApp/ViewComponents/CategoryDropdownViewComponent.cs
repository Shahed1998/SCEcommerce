using Manager.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ViewComponents
{
    public class CategoryDropdownViewComponent : ViewComponent
    {
        private readonly ICategoryManager _categoryManager;

        public CategoryDropdownViewComponent(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryManager.All();
            return View("Default", categories);
        }
    }
}
