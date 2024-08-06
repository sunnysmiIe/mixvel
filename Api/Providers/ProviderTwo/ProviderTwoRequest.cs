using System.ComponentModel.DataAnnotations;

namespace Api.Providers.ProviderTwo;

public class ProviderTwoRequest
{
    /// <summary>
    /// Start point of route, e.g. Moscow
    /// </summary>
    [Required]
    public string Departure { get; set; }
    
    /// <summary>
    /// End point of route, e.g. Sochi
    /// </summary>
    [Required]
    public string Arrival { get; set; }
    
    /// <summary>
    /// Start date of route
    /// </summary>
    [Required]
    public DateTime DepartureDate { get; set; }
    
    /// <summary>
    /// Minimum value of timelimit for route
    /// </summary>
    public DateTime? MinTimeLimit { get; set; }
}