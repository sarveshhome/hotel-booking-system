using Microsoft.AspNetCore.Mvc;
using Hotel.Service.Infrastructure.Services;

namespace Hotel.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AmadeusController : ControllerBase
{
    private readonly IAmadeusApiService _amadeusService;

    public AmadeusController(IAmadeusApiService amadeusService)
    {
        _amadeusService = amadeusService;
    }

    [HttpGet("hotels/city/{cityCode}")]
    public async Task<IActionResult> SearchHotelsByCity(string cityCode)
    {
        var hotels = await _amadeusService.SearchHotelsByCityAsync(cityCode);
        return Ok(hotels);
    }
}