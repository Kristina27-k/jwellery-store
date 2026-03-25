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
    public async Task<ActionResult> Create(JewelryItemDto itemDto)
    {
        var response = await _service.AddJewelryAsync(itemDto);
        if (!response.IsSuccess) return BadRequest(response);
        return CreatedAtAction(nameof(GetById), new { id = itemDto.Data?.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, JewelryItemDto itemDto)
    {
        if (id != itemDto.Id) return BadRequest();
        var response = await _service.UpdateJewelryAsync(itemDto);
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
