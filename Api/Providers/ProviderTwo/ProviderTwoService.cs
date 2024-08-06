using System.Text;
using Api.Models;
using Newtonsoft.Json;

namespace Api.Providers.ProviderTwo;

public class ProviderTwoService: IProviderService
{
    private const int _timeoutSeconds = 15;
    private const string _providerTwoApi = "http://provider-two/api";
    private const string _searchUri = "/v1/search";
    private const string _pingUri = "/ping";
    private IHttpClientFactory _factory;

    public ProviderTwoService(IHttpClientFactory factory)
    {
        _factory = factory;
    }


    public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        ProviderTwoRequest providerRequest = MapToRequest(request);
        
        var response = await PostContent(providerRequest, _providerTwoApi, _searchUri, cancellationToken);
        
        var result = MapToResult(response);

        return result;
    }

    public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
    {
        // TODO: finish PostContent
        using var client = _factory.CreateClient("ProvTwo");
            
        var response = await client.PostAsync(_providerTwoApi + _searchUri, null, cancellationToken);

        return response.IsSuccessStatusCode;
    }

    private SearchResponse MapToResult(ProviderTwoResponse response)
    {
        return new SearchResponse
        {
            Routes = response.Routes.ToList().Select(x=>new Models.Route
            {
                Id = Guid.NewGuid(),
                Origin = x.Departure.Point,
                Destination = x.Arrival.Point,
                Price = x.Price,
                TimeLimit = x.TimeLimit,
                OriginDateTime = x.Departure.Date,
                DestinationDateTime = x.Arrival.Date
            }).ToArray(),
            MaxPrice = response.Routes.Max(x=>x.Price),
            MinPrice = response.Routes.Min(x=>x.Price),
            MaxMinutesRoute = (int) (response.Routes.Max(x => x.Arrival.Date - x.Departure.Date).TotalMinutes),
            MinMinutesRoute = (int) (response.Routes.Min(x => x.Arrival.Date - x.Departure.Date).TotalMinutes)
        };
    }

    // TODO: move to automapper
    private ProviderTwoRequest MapToRequest(SearchRequest request)
    {
        var result = new ProviderTwoRequest
        {
            Arrival = request.Destination,
            Departure = request.Origin,
            DepartureDate = request.OriginDateTime
        };

        if (request.Filters!.MinTimeLimit.HasValue)
            result.MinTimeLimit = request.Filters.MinTimeLimit;

        return result;
    }

    private async Task<ProviderTwoResponse> PostContent(Object requestData, String api, String endpoint, CancellationToken token)
    {
        var result = new ProviderTwoResponse();

        try
        {
            var json = JsonConvert.SerializeObject(requestData);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = _factory.CreateClient("ProvTwo");
            var response = await client.PostAsync(api + endpoint, data, token);

            string responseData = await response.Content.ReadAsStringAsync(token);
            result = JsonConvert.DeserializeObject<ProviderTwoResponse>(responseData);
        }
        // TODO: add error handling
        catch (Exception e)
        {
            throw;
        }
        
        return result;
    }
}