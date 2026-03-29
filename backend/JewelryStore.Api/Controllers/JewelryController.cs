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
}
