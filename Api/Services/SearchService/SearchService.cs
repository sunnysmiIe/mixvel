using System.Collections.Concurrent;
using Api.Models;
using Api.Providers;
using Route = Api.Models.Route;

namespace Api.Services.SearchService;

public class SearchService : ISearchService
{
    private static ConcurrentDictionary<int, Route> _cache = new ();
    private IProviderService _providerOne;
    private IProviderService _providerTwo;

    public SearchService(
        [FromKeyedServices("ProviderOne")] IProviderService providerOne,
        [FromKeyedServices("ProviderTwo")] IProviderService providerTwo)
    {
        _providerOne = providerOne;
        _providerTwo = providerTwo;
    }

    public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
    {
        var cached = GetCachedResults(request);
        
        if (request.Filters!.OnlyCached.GetValueOrDefault())
            return cached;

        var joinedRoutes = new List<Route>();
        try
        {
            var providerOneTask = _providerOne.SearchAsync(request, cancellationToken);
            var providerTwoTask = _providerTwo.SearchAsync(request, cancellationToken);

            await Task.WhenAll(new[] {providerOneTask, providerTwoTask});
            
            if (providerOneTask.IsCompletedSuccessfully)
                joinedRoutes.AddRange(providerOneTask.Result.Routes);
            
            if(providerTwoTask.IsCompletedSuccessfully)
                joinedRoutes.AddRange(providerTwoTask.Result.Routes);

        }
        catch (Exception e)
        {
            return cached;
        }
        
        RepopulateCache(joinedRoutes);

        return new SearchResponse
        {
            Routes = joinedRoutes.ToArray(),
            MaxPrice = joinedRoutes.Max(x => x.Price),
            MinPrice = joinedRoutes.Min(x => x.Price),
            MaxMinutesRoute = (int) (joinedRoutes.Max(x => x.DestinationDateTime - x.OriginDateTime).TotalMinutes),
            MinMinutesRoute = (int) (joinedRoutes.Min(x => x.DestinationDateTime - x.OriginDateTime).TotalMinutes)
        };
    }

    private void RepopulateCache(List<Route> joinedRoutes)
    {
        var expired = _cache.Where(x => x.Value.TimeLimit < DateTime.UtcNow).ToList();
        foreach (var item in expired)
            _cache.TryRemove(item);
        

        foreach (var route in joinedRoutes)
            _cache.AddOrUpdate(route.GetHashCode(), route, (key, oldvalue) => route);
    }

    private SearchResponse GetCachedResults(SearchRequest request)
    {
        var query = _cache.Values.Where(x=>
            x.Destination.Equals(request.Destination) && 
            x.Origin.Equals(request.Origin) &&
            x.OriginDateTime.Equals(request.OriginDateTime));

        if (request.Filters!.DestinationDateTime.HasValue)
            query.Where(x => x.DestinationDateTime < request.Filters.DestinationDateTime);
        
        if (request.Filters!.MaxPrice.HasValue)
            query.Where(x => x.Price < request.Filters.MaxPrice);
        
        // TODO: write a test about it - usage is unclear, need more specifics
        if (request.Filters!.MinTimeLimit.HasValue)
            query.Where(x => x.TimeLimit > request.Filters.MinTimeLimit);

        // TODO: collection matearilized? filters?
        var routes = query.ToList();
        return new SearchResponse
        {
            Routes = routes.ToArray(),
            MaxPrice = routes.Max(x=>x.Price),
            MinPrice = routes.Min(x=>x.Price),
            MaxMinutesRoute = (int)routes.Max(x=>x.DestinationDateTime-x.OriginDateTime).TotalMinutes,
            MinMinutesRoute = (int)routes.Min(x=>x.DestinationDateTime-x.OriginDateTime).TotalMinutes
        };
    }

    public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
    {
        var isProvOneAvailableTask = _providerOne.IsAvailableAsync(cancellationToken);
        var isProvTwoAvailableTask = _providerTwo.IsAvailableAsync(cancellationToken);

        await Task.WhenAll(new [] {isProvOneAvailableTask, isProvTwoAvailableTask});
        
        return isProvOneAvailableTask.Result || isProvTwoAvailableTask.Result;
    }
}