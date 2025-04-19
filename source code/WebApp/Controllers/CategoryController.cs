 using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Models.BusinessEntities;
using Models.Entities;
using System.Text.Json;
using System.Text.Json.Nodes;
using Utility.Helpers;

namespace WebApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly HelperEncryption _encryption;
        public CategoryController(ApplicationDbContext context, HelperEncryption encryption)
        {
            _context = context;
            _encryption = encryption;
        }
        public IActionResult Index(NotificationViewModel category)
        {

            if(!string.IsNullOrWhiteSpace(category.p))
            {
                var decryptedValue = _encryption.Decrypt(category.p);
                NotificationViewModel? response;

                if(string.IsNullOrWhiteSpace(decryptedValue))
                {
                    response = new NotificationViewModel();
                }
                else
                {
                    response = JsonSerializer.Deserialize<NotificationViewModel>(decryptedValue);
                }

                if (response != null) 
                {
                    // use automapper for this mapping in the future
                    category.IsEdited = response.IsEdited;
                    category.IsCreated = response.IsCreated;
                    category.IsDeleted = response.IsDeleted;
                    category.success = response.success;
                    category.showtoastMessage = response.showtoastMessage;
                }
            }

            ViewBag.ToastrNotification = category;

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

            if (!ModelState.IsValid)
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

            if (_context.SaveChanges() > 0)
            {
                return Ok(new { success = true, redirectToAction = Url.Action("Index", "Category") });
            }

            return PartialView();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {

            var category = _context.categories.FirstOrDefault(x => x.Id.Equals(id));
            string? serializedParams, encryptedParams;

            if (category == null)
            {
                return NotFound(new { success = false, redirectToAction = Url.Action("Index", "Category") });
            }

            _context.categories.Remove(category);

            if(_context.SaveChanges() > 0)
            {
                serializedParams = JsonSerializer.Serialize(new { showtoastMessage = true, success = true, IsDeleted = true });
                encryptedParams = _encryption.Encrypt(serializedParams);

                return Ok(new { success = true, redirectToAction = Url.Action("Index", "Category", new { p = encryptedParams }) });
            }

            serializedParams = JsonSerializer.Serialize(new { showtoastMessage = true, success = false, IsDeleted = true });
            encryptedParams = _encryption.Encrypt(serializedParams);

            return StatusCode(500, new { success = false, redirectToAction = Url.Action("Index", "Category", new { p = encryptedParams }) });
        }
    }
}
