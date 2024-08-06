using System.ComponentModel.DataAnnotations;

namespace Api.Providers.ProviderOne;

public class ProviderOneRoute
{
    /// <summary>
    /// Start point of route 
    /// </summary>
    [Required]
    public string From { get; set; }
    
    /// <summary>
    /// End point of route 
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
    [Required]
    public DateTime DateTo { get; set; }
    
    /// <summary>
    /// Price of route 
    /// </summary>
    [Required]
    public decimal Price { get; set; }
    
    /// <summary>
    /// imelimit. After it expires, route became not actual 
    /// </summary>
    [Required]
    public DateTime TimeLimit { get; set; }
}