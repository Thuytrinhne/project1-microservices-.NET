using BuildingBlocks.PhotoCloudinary;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the dependency container

builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
// Config from app settings
builder.Services.Configure<CloudinaryConfig>(builder.Configuration.GetSection("CloudinaryConfig"));


var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    // add behavior into mediator request lifecycle 
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter(); // minimal api


builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("DefaultConnectionString")!);
    opts.AutoCreateSchemaObjects = AutoCreate.All;

}).UseLightweightSessions();


if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks() // for catalog microservices
    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnectionString")!);
var app = builder.Build();

// Configure the HTTP request pipeline to mapping the incoming request 
app.MapCarter();
app.UseExceptionHandler(opts => { });
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
app.Run();
