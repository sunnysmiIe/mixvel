using System.ComponentModel.DataAnnotations;

namespace Api.Providers.ProviderTwo;

public class ProviderTwoResponse
{
    /// <summary>
    /// Array of routes
    /// </summary>
    [Required]
    public ProviderTwoRoute[] Routes { get; set; }
}