using Manager.Implementations;
using Manager.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Utility.Helpers;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductManager _producManager;
        private readonly HelperEncryption _encryption;

        public ProductController(IProductManager categoryManager, HelperEncryption encryption)
        {
            _producManager = categoryManager;
            _encryption = encryption;
        }


        public async Task<IActionResult> Index(int page = 1, int pageSize = 30)
        {
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;

            var model = await _producManager.GetAll(page, pageSize);

            return View(model);
        }
    }
}
