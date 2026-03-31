using JewelryStore.Api.Models.DTOs;
using JewelryStore.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace JewelryStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public JewelryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<JewelryItemDto>>> GetAll()
    {
        var response = await _service.GetAllCategoryAsync();
        return Ok(response);
    }
}
