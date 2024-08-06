using System.ComponentModel.DataAnnotations;

namespace Api.Providers.ProviderOne;

public class ProviderOneResponse
{
    [Required]
    public ProviderOneRoute[] Routes { get; set; }
}