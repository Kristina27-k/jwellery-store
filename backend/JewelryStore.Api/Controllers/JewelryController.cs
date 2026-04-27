using JewelryStore.Api.Models.DTOs;
using JewelryStore.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace JewelryStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JewelryController : ControllerBase
{
    private readonly IJewelryService _service;

    public JewelryController(IJewelryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<JewelryItemDto>>> GetAll()
    {
        var response = await _service.GetAllJewelryAsync();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JewelryItemDto>> GetById(int id)
    {
        var response = await _service.GetJewelryByIdAsync(id);
        if (!response.IsSuccess) return NotFound(response);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] JewelryItemDto item)
    {
        var response = await _service.AddJewelryAsync(item);
        if (!response.IsSuccess) return BadRequest(response);
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] JewelryItemDto item)
    {
        var response = await _service.UpdateJewelryAsync(item);
        if (!response.IsSuccess) return BadRequest(response);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _service.DeleteJewelryAsync(id);
        if (!response.IsSuccess) return BadRequest(response);
        return Ok(response);
    }
}
