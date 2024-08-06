using Api.Providers;
using Api.Providers.ProviderOne;
using Api.Providers.ProviderTwo;
using Api.Services;
using Api.Services.SearchService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddKeyedScoped<IProviderService, ProviderOneService>("ProviderOne");
builder.Services.AddKeyedScoped<IProviderService, ProviderTwoService>("ProviderTwo");
builder.Services.AddHttpClient();
builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
