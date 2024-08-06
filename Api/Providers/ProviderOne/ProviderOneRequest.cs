using System.ComponentModel.DataAnnotations;

namespace Api.Providers.ProviderOne;

public class ProviderOneRequest
{
    /// <summary>
    /// Start point of route, e.g. Moscow 
    /// </summary>
    [Required]
    public string From { get; set; }
    
    /// <summary>
    /// End point of route, e.g. Sochi
    /// </summary>
    [Required]
    public string To { get; set; }
    
    /// <summary>
    /// Start date of route
    /// </summary>
    [Required]
    public DateTime DateFrom { get; set; }
    
    /// <summary>
    /// End date of route
    /// </summary>
    public DateTime? DateTo { get; set; }
    
    /// <summary>
    /// Maximum price of route
    /// </summary>
    public decimal? MaxPrice { get; set; }
}