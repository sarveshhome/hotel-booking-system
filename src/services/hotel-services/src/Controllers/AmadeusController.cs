using Microsoft.AspNetCore.Mvc;
using Hotel.Service.Infrastructure.Services;

namespace Hotel.Services.Controllers;

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

    [HttpGet("token/test")]
    public async Task<IActionResult> TestToken()
    {
        try
        {
            var token = await _amadeusService.GetAccessTokenAsync();
            return Ok(new { 
                success = true, 
                tokenLength = token.Length,
                tokenPreview = token.Length > 10 ? token.Substring(0, 10) + "..." : token,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { 
                success = false, 
                error = ex.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }
}