using Manager.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductManager _productManager;

        public HomeController(ILogger<HomeController> logger, IProductManager productManager)
        {
            _logger = logger;
            _productManager = productManager;
        }

        public async Task<IActionResult> Index(int page=1, int pageSize=30)
        {
            ViewData["Title"] = "Home Page";

            var model = await _productManager.GetAll(page, pageSize);

            return View(model);
        }
        
    }
}
