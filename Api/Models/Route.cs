using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class Route
{
    /// <summary>
    /// Identifier of the whole route
    /// </summary>
    [Required]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Start point of route
    /// </summary>
    [Required]
    public string Origin { get; set; }
    
    /// <summary>
    /// End point of route
    /// </summary>
    [Required]
    public string Destination { get; set; }
    
    /// <summary>
    /// Start date of route
    /// </summary>
    [Required]
    public DateTime OriginDateTime { get; set; }
    
    /// <summary>
    /// End date of route
    /// </summary>
    [Required]
    public DateTime DestinationDateTime { get; set; }
    
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

    public override int GetHashCode()
    {
        return (Origin + 
                Destination + 
                OriginDateTime.ToLongDateString() +
                DestinationDateTime.ToLongDateString() +
                Price).GetHashCode();
    }
}