using Manager.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.BusinessEntities;
using System.Text.Json;
using Utility.Helpers;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductManager _producManager;
        private readonly ICategoryManager _categoryManager;
        private readonly HelperEncryption _encryption;

        public ProductController(IProductManager productManager, HelperEncryption encryption, ICategoryManager categoryManager)
        {
            _producManager = productManager;
            _encryption = encryption;
            _categoryManager = categoryManager;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 30)
        {

            if (TempData["Notification"] is string json)
            {
                ViewBag.ToastrNotification = JsonSerializer.Deserialize<NotificationViewModel>(json);
            }

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;

            var model = await _producManager.GetAll(page, pageSize);

            return View(model);
        }

        public async Task<IActionResult> View(int productId)
        {
            var model = await _producManager.GetWithCategory(productId);

            return PartialView(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categoryList = (await _categoryManager.All()).Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();

            ViewBag.CategoryList = categoryList;

            return PartialView(new ProductDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO model)
        {

            if (!ModelState.IsValid)
            {
                var categoryList = (await _categoryManager.All()).Select(x => new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();

                ViewBag.CategoryList = categoryList;

                return PartialView(new ProductDTO());
            }

            if (await _producManager.Add(model))
            {
                TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
                {
                    NotificationStatus = NotficationStatus.Success.ToString(),
                    NotificationMessage = $"Product {model.Title} successfully added.",
                    showtoastMessage = true,
                });

                return Ok(new
                {
                    success = true,
                    redirectToAction = Url.Action("Index", "Product")
                });
            }

            TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
            {
                NotificationStatus = NotficationStatus.Error.ToString(),
                NotificationMessage = $"Failed to add product {model.Title}.",
                showtoastMessage = true,
            });

            return StatusCode(500, new
            {
                success = false,
                redirectToAction = Url.Action("Index", "Product")
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var categoryList = (await _categoryManager.All()).Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();

            ViewBag.CategoryList = categoryList;

            var product = await _producManager.GetWithCategory(Id);

            if (product is null) return NotFound($"Product not found");

            return PartialView(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductDTO model)
        {

            if (!ModelState.IsValid)
            {
                var categoryList = (await _categoryManager.All()).Select(x => new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();

                ViewBag.CategoryList = categoryList;

                return PartialView(model);
            }

            if (await _producManager.Update(model))
            {
                TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
                {
                    NotificationStatus = NotficationStatus.Success.ToString(),
                    NotificationMessage = $"Product {model.Title} successfully updated.",
                    showtoastMessage = true,
                });

                return Ok(new 
                {
                    success = true,
                    redirectToAction = Url.Action("Index", "Product")
                });
            }

            TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
            {
                NotificationStatus = NotficationStatus.Error.ToString(),
                NotificationMessage = $"Failed to update product {model.Title}.",
                showtoastMessage = true,
                
            });

            return StatusCode(500, new 
            {
                success = false,
                redirectToAction = Url.Action("Index", "Product")
            });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _producManager.Remove(id))
            {
                TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
                {
                    NotificationStatus = NotficationStatus.Success.ToString(),
                    NotificationMessage = $"Product successfully deleted.",
                    showtoastMessage = true,
                });

                return Ok(new
                {
                    success = true,
                    redirectToAction = Url.Action("Index", "Product")
                });
            }

            TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
            {
                NotificationStatus = NotficationStatus.Error.ToString(),
                NotificationMessage = "Failed to delete product.",
                showtoastMessage = true,
            });

            return StatusCode(500, new
            {
                success = false,
                redirectToAction = Url.Action("Index", "Product")
            });
        }
    }
}
