using JewelryStore.Api.Models.DTO;
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
         
        var items = await _service.GetAllJewelryAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JewelryItemDto>> GetById(int id)
    {
        var item = await _service.GetJewelryByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult> Create(JewelryItemDto itemDto)
    {
        await _service.AddJewelryAsync(itemDto);
        return CreatedAtAction(nameof(GetById), new { id = itemDto.Id }, itemDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, JewelryItemDto itemDto)
    {
        if (id != itemDto.Id) return BadRequest();
        await _service.UpdateJewelryAsync(itemDto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteJewelryAsync(id);
        return NoContent();
    }
}
