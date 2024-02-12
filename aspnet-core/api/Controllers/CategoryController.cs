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
            try
            {
                var isSaved = await _categoryManager.Insert(dto);
                if (isSaved == true)
                {
                    return Ok(new { isSaved });
                }
                return BadRequest();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _categoryManager.Get());
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            try
            {
                return Ok(await _categoryManager.GetById(Id));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
