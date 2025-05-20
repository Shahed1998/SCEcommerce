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


        public async Task<IActionResult> Index(int page = 1, int pageSize = 30)
        {
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;

            var model = await _producManager.GetAll(page, pageSize);

            return View(model);
        }

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
    }
}
