using JewelryStore.Api.Models.DTOs;
using JewelryStore.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JewelryStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        var response = await _service.GetAllCategoriesAsync();
        if (!response.IsSuccess)
            return BadRequest(response);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        var response = await _service.GetCategoryByIdAsync(id);
        if (!response.IsSuccess)
            return NotFound(response);
        return Ok(response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CategoryDto categoryDto)
    {
        var response = await _service.CreateCategoryAsync(categoryDto);
        if (!response.IsSuccess)
            return BadRequest(response);
        return CreatedAtAction(nameof(GetById), new { id = response.Data.Id }, response);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] CategoryDto categoryDto)
    {
        var response = await _service.UpdateCategoryAsync(categoryDto);
        if (!response.IsSuccess)
            return BadRequest(response);
        return Ok(response);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _service.DeleteCategoryAsync(id);
        if (!response.IsSuccess)
            return BadRequest(response);
        return Ok(response);
    }
}
