using Manager.Implementations;
using Manager.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.BusinessEntities;
using Models.Entities;
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


        public async Task<IActionResult> Index(NotificationViewModel nvm, int page = 1, int pageSize = 30)
        {
           
            var model = await _producManager.GetAll(page, pageSize);

            ViewBag.ToastrNotification = nvm;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;

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

            if(!ModelState.IsValid)
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
                return Ok(new { success = true, redirectToAction = Url.Action("Index", "Product") });
            }

            return StatusCode(500, new { success = true, redirectToAction = Url.Action("Index", "Product") });
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

            if(!ModelState.IsValid)
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
                return Ok(new { success = true, redirectToAction = Url.Action("Index", "Product") });
            }

            return PartialView();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if(await _producManager.Remove(id))
            {
                return Ok(new { success = true, redirectToAction = Url.Action("Index", "Product", new { IsDeleted=true }) });
            }

            return StatusCode(500, new { success = false, redirectToAction = Url.Action("Index", "Product", new { IsDeleted = false }) });
        }
    }
}
