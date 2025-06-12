using Manager.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.BusinessEntities;
using Models.Entities;
using System.Text.Json;
using Utility.Helpers;
using WebApp.Controllers;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : BaseController
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
            Alert();

            var model = await _categoryManager.GetAll(page, pageSize);

            return View(model);
        }

        public async Task<IActionResult> View(int Id, int page = 1, int pageSize = 5)
        {
            var category = await _categoryManager.Get(Id);

            ViewData["Title"] = "View Category";

            category.page = page;
            category.pageSize = pageSize;

            return View(category);
        }

        public IActionResult Create()
        {
            Alert();

            ViewData["Title"] = "Create New Category";

            return View(new Category());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            if(await _categoryManager.Add(category))
            {
                TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
                {
                    NotificationStatus = NotficationStatus.Success.ToString(),
                    NotificationMessage = $"Category {category.Name} successfully added.",
                    showtoastMessage = true,
                });

                return RedirectToAction("Index");
            }

            TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
            {
                NotificationStatus = NotficationStatus.Error.ToString(),
                NotificationMessage = $"Failed to add category {category.Name}.",
                showtoastMessage = true,
            });

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            Alert();

            ViewData["Title"] = "Edit Category";

            var category = await _categoryManager.Get(Id);

            if (category == null)
            {
                TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
                {
                    NotificationStatus = NotficationStatus.Error.ToString(),
                    NotificationMessage = $"Category not found.",
                    showtoastMessage = true,
                });

                return RedirectToAction("Index");
            }
            
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category model)
        {
            var category = await _categoryManager.Get(model.Id);

            if (category == null)
            {
                TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
                {
                    NotificationStatus = NotficationStatus.Error.ToString(),
                    NotificationMessage = $"Category not found.",
                    showtoastMessage = true,
                });

                return RedirectToAction("Index");
            }

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

                return RedirectToAction("Index");
            }

            TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
            {
                NotificationStatus = NotficationStatus.Error.ToString(),
                NotificationMessage = $"Failed to update category {category.Name}.",
                showtoastMessage = true,
            });

            return View(model);
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

        [HttpPost]
        public JsonResult DisplayOrderAlreadyExist(int DisplayOrder, int? Id)
        {
            var isExist = _categoryManager.DisplayOrderAlreadyExist(DisplayOrder, Id);

            return Json(!isExist);
        }
    }
}
