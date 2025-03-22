using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context) 
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = _context.categories.ToList();
            return View(model);
        }
    }
}
