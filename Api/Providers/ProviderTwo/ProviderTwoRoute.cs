using System.ComponentModel.DataAnnotations;

namespace Api.Providers.ProviderTwo;


public class ProviderTwoRoute
{
    /// <summary>
    /// Start point of route
    /// </summary>
    [Required]
    public ProviderTwoPoint Departure { get; set; }
    
    
    /// <summary>
    /// End point of route
    /// </summary>
    [Required]
    public ProviderTwoPoint Arrival { get; set; }
    
    /// <summary>
    /// Price of route
    /// </summary>
    [Required]
    public decimal Price { get; set; }
    
    /// <summary>
    /// Timelimit. After it expires, route became not actual
    /// </summary>
    [Required]
    public DateTime TimeLimit { get; set; }
}

public class ProviderTwoPoint
{
    /// <summary>
    /// Name of point, e.g. Moscow\Sochi
    /// </summary>
    [Required]
    public string Point { get; set; }
    
    /// <summary>
    /// Date for point in Route, e.g. Point = Moscow, Date = 2023-01-01 15-00-00
    /// </summary>
    [Required]
    public DateTime Date {get; set; }
}
