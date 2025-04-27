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
        public async Task<IActionResult> Index(NotificationViewModel category, int page=1)
        {

            ViewBag.Page = page;

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

            var model = await _categoryManager.GetAll();

            var pagedList = new PagedList<Category>(model, 1, 30, model.Count());

            return View(pagedList);
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
                return Ok(new { success = true, redirectToAction = Url.Action("Index", "Category") });
            }

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
                return Ok(new { success = true, redirectToAction = Url.Action("Index", "Category") });
            }

            return PartialView();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {

            var category = await _categoryManager.Get(id);
            string? serializedParams, encryptedParams;

            if (category == null)
            {
                return NotFound(new { success = false, redirectToAction = Url.Action("Index", "Category") });
            }

            if(await _categoryManager.Remove(category))
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
