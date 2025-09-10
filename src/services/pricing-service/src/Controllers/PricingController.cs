using Microsoft.AspNetCore.Mvc;
using MediatR;
using Pricing.Service.Application.Features.Pricing.Commands.CreatePricingRule;
using Pricing.Service.Application.Features.Pricing.Queries.CalculatePrice;
using Pricing.Service.Application.Common.Interfaces;

namespace Pricing.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PricingController : ControllerBase
{
    private readonly IMediator _mediator;

    public PricingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("rules")]
    public async Task<ActionResult<Guid>> CreatePricingRule(CreatePricingRuleCommand command)
    {
        try
        {
            var ruleId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPricingRule), new { id = ruleId }, ruleId);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("calculate")]
    public async Task<ActionResult<PriceCalculationResult>> CalculatePrice(CalculatePriceQuery query)
    {
        try
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("rules/{id}")]
    public ActionResult<object> GetPricingRule(Guid id)
    {
        // TODO: Implement GetPricingRuleById query
        return NotFound();
    }

    [HttpGet("hotels/{hotelId}/pricing")]
    public ActionResult<object> GetHotelPricing(Guid hotelId, [FromQuery] DateTime? date)
    {
        // TODO: Implement GetHotelPricing query
        return NotFound();
    }
}
