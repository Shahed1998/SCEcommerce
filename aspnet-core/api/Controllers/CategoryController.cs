using entity.business_entities;
using entity.Business_Entities;
using manager.Interfaces;
using Microsoft.AspNetCore.Mvc;
using repository.Interfaces.Helper;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryManager _categoryManager;
        private readonly IHelperFileHandler _fileHandler;

        public CategoryController(ICategoryManager categoryManager, IHelperFileHandler fileHandler) 
        {
            _categoryManager = categoryManager;
            _fileHandler = fileHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CategoryDTO dto)
        {
            dto.Image.Url = await _fileHandler.UploadImage(dto.Image);

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
            var category = await _categoryManager.GetById(Id);
            if(category == null) return NotFound();
            return Ok(category);
        }
    }
}
