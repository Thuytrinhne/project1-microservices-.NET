var builder = WebApplication.CreateBuilder(args);
// add services to the container for dependency injection
builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration); //  use extention method for service

var app = builder.Build();

// configure the HTTP request pipeline
app.UseApiService();  // use extention method for web application
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
app.Run();
