using entity.business_entities;
using manager.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryManager _categoryManager;
        public CategoryController(ICategoryManager categoryManager) 
        {
            _categoryManager = categoryManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTO dto)
        {
            var isSaved = await _categoryManager.Insert(dto);
            if (isSaved == true)
            {
                return Ok(new { isSaved });
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _categoryManager.Get());
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            return Ok(await _categoryManager.GetById(Id));
        }
    }
}
