using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class SearchRequest
{
    /// <summary>
    /// Start point of route, e.g. Moscow 
    /// </summary>
    [Required]
    public string Origin { get; set; }
    
    /// <summary>
    /// End point of route, e.g. Sochi
    /// </summary>
    [Required]
    public string Destination { get; set; }
    
    /// <summary>
    /// Start date of route
    /// </summary>
    [Required]
    public DateTime OriginDateTime { get; set; }
    
    // Optional
    public SearchFilters? Filters { get; set; }
}