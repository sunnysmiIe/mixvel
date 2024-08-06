using Api.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("route")]
public class RouteController : ControllerBase
{
    private readonly ISearchService _searchService;
    
    public RouteController(ISearchService service)
    {
        _searchService = service;
    }

    [HttpGet]
    public async Task<ActionResult> GetRoutesAsync([FromBody] SearchRequest request, CancellationToken cancellationToken)
    {
        // TODO: handle exceptions
        try
        {
            var result = await _searchService.SearchAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpGet]
    [Route("availability")]
    public async Task<ActionResult> GetAvailability(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _searchService.IsAvailableAsync(cancellationToken);
            return Ok(result);
        }
        catch (Exception e)
        {
            throw;
        }
    }
}