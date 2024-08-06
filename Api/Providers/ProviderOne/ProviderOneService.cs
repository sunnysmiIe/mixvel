using System.Runtime.InteropServices.JavaScript;
using System.Text;
using Api.Models;
using Newtonsoft.Json;

namespace Api.Providers.ProviderOne;

public class ProviderOneService: IProviderService
{
    private const int _timeoutSeconds = 15;
    private const string _providerOneApi = "http://provider-one/api";
    private const string _searchUri = "/v1/search";
    private const string _pingUri = "/ping";
    private IHttpClientFactory _factory;

    public ProviderOneService(IHttpClientFactory factory)
    {
        _factory = factory;
    }


    public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        ProviderOneRequest providerRequest = MapToRequest(request);
        
        var response = await PostContent(providerRequest, _providerOneApi, _searchUri, cancellationToken);
        
        var result = MapToResult(response);

        return result;
    }

    public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
    {
        // TODO: finish PostContent
        using var client = _factory.CreateClient("ProvOne");
            
        var response = await client.PostAsync(_providerOneApi + _searchUri, null, cancellationToken);

        return response.IsSuccessStatusCode;
    }

    private SearchResponse MapToResult(ProviderOneResponse response)
    {
        return new SearchResponse
        {
            Routes = response.Routes.ToList().Select(x=>new Models.Route
            {
                Id = Guid.NewGuid(),
                Origin = x.From,
                Destination = x.To,
                Price = x.Price,
                TimeLimit = x.TimeLimit,
                OriginDateTime = x.DateFrom,
                DestinationDateTime = x.DateTo
            }).ToArray(),
            MaxPrice = response.Routes.Max(x=>x.Price),
            MinPrice = response.Routes.Min(x=>x.Price),
            MaxMinutesRoute = (int) (response.Routes.Max(x => x.DateTo - x.DateFrom).TotalMinutes),
            MinMinutesRoute = (int) (response.Routes.Min(x => x.DateTo - x.DateFrom).TotalMinutes)
        };
    }

    // TODO: move to automapper
    private ProviderOneRequest MapToRequest(SearchRequest request)
    {
        var result = new ProviderOneRequest
        {
            To = request.Destination,
            From = request.Origin,
            DateFrom = request.OriginDateTime
        };


        if (request.Filters!.DestinationDateTime.HasValue)
            result.DateTo = request.Filters.DestinationDateTime;
        
        if (request.Filters!.MaxPrice.HasValue)
            result.MaxPrice = request.Filters.MaxPrice;;

        return result;
    }

    private async Task<ProviderOneResponse> PostContent(Object requestData, String api, String endpoint, CancellationToken token)
    {
        var result = new ProviderOneResponse();

        try
        {
            var json = JsonConvert.SerializeObject(requestData);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = _factory.CreateClient("ProvOne");
            var response = await client.PostAsync(api + endpoint, data, token);

            string responseData = await response.Content.ReadAsStringAsync(token);
            result = JsonConvert.DeserializeObject<ProviderOneResponse>(responseData);
        }
        // TODO: add error handling
        catch (Exception e)
        {
            throw;
        }
        
        return result;
    }
}