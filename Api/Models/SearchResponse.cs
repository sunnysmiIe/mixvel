using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class SearchResponse
{
    /// <summary>
    /// Array of routes
    /// </summary>
    [Required]
    public Route[] Routes { get; set; }
    
    /// <summary>
    /// The cheapest route
    /// </summary>
    [Required]
    public decimal MinPrice { get; set; }
    
    /// <summary>
    /// Most expensive route
    /// </summary>
    [Required]
    public decimal MaxPrice { get; set; }
    
    /// <summary>
    /// The fastest route
    /// </summary>
    [Required]
    public int MinMinutesRoute { get; set; }
    
    /// <summary>
    /// The longest route
    /// </summary>
    [Required]
    public int MaxMinutesRoute { get; set; }
}

