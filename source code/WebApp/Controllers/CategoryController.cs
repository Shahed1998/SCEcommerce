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
            var model = _context.categories.OrderByDescending(x => x.Id).ToList();
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

            _context.categories.Add(category);
            _context.SaveChanges();

            return Ok(new { success = true, redirectToAction = Url.Action("Index", "Category") });
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var category = _context.categories.FirstOrDefault(x => x.Id == Id);

            if (category == null) return NotFound($"Category not found");

            return PartialView(category);
        }

        [HttpPost]
        public IActionResult Edit(Category model)
        {
            var category = _context.categories.FirstOrDefault(x => x.Id == model.Id);

            if (category == null) return NotFound($"Category not found");

            category.Id = model.Id;
            category.Name = model.Name;
            category.DisplayOrder = model.DisplayOrder;

            _context.categories.Update(category);

            if(_context.SaveChanges() > 0)
            {
                return Ok(new { success = true, redirectToAction = Url.Action("Index", "Category") });
            }

            return PartialView();
        }
    }
}
