using Microsoft.AspNetCore.Mvc;
using Pricing.Service.Infrastructure.Services;

namespace Pricing.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AmadeusController : ControllerBase
{
    private readonly IAmadeusApiService _amadeusService;

    public AmadeusController(IAmadeusApiService amadeusService)
    {
        _amadeusService = amadeusService;
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetToken()
    {
        var token = await _amadeusService.GetAccessTokenAsync();
        return Ok(new { AccessToken = token });
    }

    [HttpGet("hotels")]
    public async Task<IActionResult> SearchHotels(
        [FromQuery] string cityCode,
        [FromQuery] string checkIn,
        [FromQuery] string checkOut)
    {
        var hotels = await _amadeusService.SearchHotelsAsync(cityCode, checkIn, checkOut);
        return Ok(hotels);
    }

    [HttpGet("flights")]
    public async Task<IActionResult> SearchFlights(
        [FromQuery] string origin,
        [FromQuery] string destination,
        [FromQuery] string departureDate,
        [FromQuery] string returnDate,
        [FromQuery] int adults,
        [FromQuery] int max = 5)
    {
        var flights = await _amadeusService.SearchFlightsAsync(origin, destination, departureDate, returnDate, adults, max);
        return Ok(flights);
    }

    [HttpGet("hotels/city/{cityCode}")]
    public async Task<IActionResult> SearchHotelsByCity(string cityCode)
    {
        var hotels = await _amadeusService.SearchHotelsByCityAsync(cityCode);
        return Ok(hotels);
    }
}