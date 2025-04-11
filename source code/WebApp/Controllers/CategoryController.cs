using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

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

        public IActionResult Create()
        {
            return PartialView(new Category());
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {

            if(!ModelState.IsValid)
            {
                return PartialView(category);
            }
            return PartialView();
        }
    }
}
