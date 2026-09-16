using Basket;
using Carter;
using Catalog;
using FluentValidation;
using FluentValidation.AspNetCore;
using Ordering;
using Serilog;
using Shared.Exceptions.Handler;
using Shared.Extensions;

var builder = WebApplication.CreateBuilder(args);

// add services to the container
// common services: carter , mediator , fluentvalidation, logging, exception handling
var catalogAssembly = typeof(CatalogModule).Assembly;
 var basketAssembly = typeof(BasketModule).Assembly;

builder.Services.AddCarterWithAssemblies(
    catalogAssembly,
    basketAssembly
    );


builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(
        catalogAssembly,
        basketAssembly
        );
    cfg.AddOpenBehavior(typeof(Shared.Behaviors.ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(Shared.Behaviors.LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssemblies(
    [catalogAssembly,
    basketAssembly]
);

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

builder.Services.
    AddExceptionHandler<CustomExceptionHandler>();
builder.Host.UseSerilog((context, config) =>
 config.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseSerilogRequestLogging();

app.UseCatalogModule();
app.UseBasketModule();  
app.UseOrderingModule();

app.Run();