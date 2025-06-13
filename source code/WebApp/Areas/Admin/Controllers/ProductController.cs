using Manager.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.BusinessEntities;
using System.Text.Json;
using Utility;
using Utility.Helpers;
using WebApp.Controllers;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{StaticDetails.Role_Admin},{StaticDetails.Role_SuperAdmin}")]
    public class ProductController : BaseController
    {
        private readonly IProductManager _producManager;
        private readonly ICategoryManager _categoryManager;
        private readonly HelperEncryption _encryption;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductManager productManager, HelperEncryption encryption,
            ICategoryManager categoryManager, IWebHostEnvironment webHostEnvironment)
        {
            _producManager = productManager;
            _encryption = encryption;
            _categoryManager = categoryManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 30)
        {

            Alert();

            var model = await _producManager.GetAll(page, pageSize);

            return View(model);
        }

        public async Task<IActionResult> View(int productId)
        {

            var model = await _producManager.GetWithCategory(productId);

            ViewData["Title"] = $"Product Details";

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ProductVM vm = new ProductVM();

            vm.CategoryList = (await _categoryManager.All()).Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                 {
                    model.CategoryList = (await _categoryManager.All()).Select(x => new SelectListItem()
                    {
                        Value = x.Id.ToString(),
                        Text = x.Name
                    }).ToList();

                    return View(model);
                }

                #region Product Image Upload
                if (model.FormFile != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;

                    string fileName = Guid.NewGuid().ToString() + "-" + DateTime.Now.ToString("yyyyMMdd-hhmmss")
                        + Path.GetExtension(model.FormFile.FileName);

                    string productPath = Path.Combine(wwwRootPath, "uploads", "product");

                    if(!Directory.Exists(productPath))
                    {
                        Directory.CreateDirectory(productPath);
                    }

                    using (var filestream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        model.FormFile.CopyTo(filestream);
                    }

                    model.ImageUrl = Path.Combine("uploads", "product", fileName);
                }
                #endregion

                HelperHtmlSanitizer.Sanitize(model);

                if (await _producManager.Add(model))
                {
                    TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
                    {
                        NotificationStatus = NotficationStatus.Success.ToString(),
                        NotificationMessage = $"Product {model.Title} successfully added.",
                        showtoastMessage = true,
                    });

                    return RedirectToAction("Index");
                }

                TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
                {
                    NotificationStatus = NotficationStatus.Error.ToString(),
                    NotificationMessage = $"Failed to add product {model.Title}.",
                    showtoastMessage = true,
                });

                return RedirectToAction("Index");

            }
            catch(Exception ex)
            {
                HelperSerilog.LogError(ex.Message, ex);

                TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
                {
                    NotificationStatus = NotficationStatus.Error.ToString(),
                    NotificationMessage = $"An internal server error occured.",
                    showtoastMessage = true,
                });

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var product = await _producManager.GetWithCategory(Id);

            if (product is null)
            {
                TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
                {
                    NotificationStatus = NotficationStatus.Success.ToString(),
                    NotificationMessage = $"Product not found!",
                    showtoastMessage = true,
                });

                return RedirectToAction("Index");
            }

            product.CategoryList = (await _categoryManager.All()).Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToList();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductVM model)
        {
            if (!ModelState.IsValid)
            {
                model.CategoryList = (await _categoryManager.All()).Select(x => new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }).ToList();

                return View(model);
            }

            #region Product Image Upload
            if (model.FormFile != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (!string.IsNullOrEmpty(model.ImageUrl))
                {
                    var oldImgPath = Path.Combine(wwwRootPath, model.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImgPath))
                    {
                        System.IO.File.Delete(oldImgPath);
                    }
                }

                string fileName = Guid.NewGuid().ToString() + "-" + DateTime.Now.ToString("yyyyMMdd-hhmmss")
                    + Path.GetExtension(model.FormFile.FileName);

                string productPath = Path.Combine(wwwRootPath, "uploads", "product");

                if (!Directory.Exists(productPath))
                {
                    Directory.CreateDirectory(productPath);
                }

                using (var filestream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    model.FormFile.CopyTo(filestream);
                }

                model.ImageUrl = Path.Combine("uploads", "product", fileName);
            }
            #endregion

            HelperHtmlSanitizer.Sanitize(model);

            if (await _producManager.Update(model))
            {
                TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
                {
                    NotificationStatus = NotficationStatus.Success.ToString(),
                    NotificationMessage = $"Product {model.Title} successfully updated.",
                    showtoastMessage = true,
                });

                return RedirectToAction("Index");

            }

            TempData["Notification"] = JsonSerializer.Serialize(new NotificationViewModel
            {
                NotificationStatus = NotficationStatus.Error.ToString(),
                NotificationMessage = $"Failed to update product {model.Title}.",
                showtoastMessage = true,

            });

            return RedirectToAction("Index");
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
