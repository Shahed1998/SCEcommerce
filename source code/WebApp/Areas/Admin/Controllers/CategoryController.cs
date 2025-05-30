using Manager.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.BusinessEntities;
using Models.Entities;
using System.Text.Json;
using Utility.Helpers;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryManager _categoryManager;
        private readonly HelperEncryption _encryption;
        public CategoryController(ICategoryManager categoryManager, HelperEncryption encryption)
        {
            _categoryManager = categoryManager;
            _encryption = encryption;
        }
        public async Task<IActionResult> Index(int page=1, int pageSize=30)
        {

            if (TempData["Notification"] is string json)
            {
                ViewBag.ToastrNotification = JsonSerializer.Deserialize<NotificationViewModel>(json);
            }

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;

            var model = await _categoryManager.GetAll(page, pageSize);

            return View(model);
        }

        public async Task<IActionResult> View(int Id)
        {
            var category = await _categoryManager.Get(Id);
            return PartialView(category);
        }

        public IActionResult Create()
        {
            return PartialView(new Category());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {

            if (!ModelState.IsValid)
            {
                return PartialView(category);
            }

            if(await _categoryManager.Add(category))
            {
                TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
                {
                    NotificationStatus = NotficationStatus.Success.ToString(),
                    NotificationMessage = $"Category {category.Name} successfully added.",
                    showtoastMessage = true,
                });

                return Ok(new { success = true, redirectToAction = Url.Action("Index", "Category") });
            }

            TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
            {
                NotificationStatus = NotficationStatus.Error.ToString(),
                NotificationMessage = $"Failed to add category {category.Name}.",
                showtoastMessage = true,
            });

            return StatusCode(500, new { success = true, redirectToAction = Url.Action("Index", "Category") });

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var category = await _categoryManager.Get(Id);

            if (category == null) return NotFound($"Category not found");

            return PartialView(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category model)
        {
            var category = await _categoryManager.Get(model.Id);

            if (category == null) return NotFound($"Category not found");

            category.Id = model.Id;
            category.Name = model.Name;
            category.DisplayOrder = model.DisplayOrder;

            if (await _categoryManager.Update(category))
            {
                TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
                {
                    NotificationStatus = NotficationStatus.Success.ToString(),
                    NotificationMessage = $"Category {category.Name} successfully updated.",
                    showtoastMessage = true,
                });

                return Ok(new { success = true, redirectToAction = Url.Action("Index", "Category") });
            }

            TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
            {
                NotificationStatus = NotficationStatus.Error.ToString(),
                NotificationMessage = $"Failed to update category {category.Name}.",
                showtoastMessage = true,

            });

            return PartialView();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {

            var category = await _categoryManager.Get(id);

            if (category == null)
            {
                TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
                {
                    NotificationStatus = NotficationStatus.Error.ToString(),
                    NotificationMessage = $"Failed to delete.",
                    showtoastMessage = true,
                });

                return NotFound(new { success = false, redirectToAction = Url.Action("Index", "Category") });
            }

            if(await _categoryManager.Remove(category))
            {
                TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
                {
                    NotificationStatus = NotficationStatus.Success.ToString(),
                    NotificationMessage = $"Category {category.Name} successfully deleted.",
                    showtoastMessage = true,
                });

                return Ok(new { success = true, redirectToAction = Url.Action("Index", "Category") });
            }

            TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
            {
                NotificationStatus = NotficationStatus.Error.ToString(),
                NotificationMessage = "Failed to delete product.",
                showtoastMessage = true,
            });

            return StatusCode(500, new { success = false, redirectToAction = Url.Action("Index", "Category") });
        }
    }
}
