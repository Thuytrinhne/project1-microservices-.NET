var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();

                      });
});
builder.Services.AddHttpClient();


// add services to the container for dependency injection
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration); //  use extention method for service

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);

// configure the HTTP request pipeline
app.UseApiService();  // use extention method for web application
//if (app.Environment.IsDevelopment())
//{
//    await app.InitialiseDatabaseAsync();
//}
app.Run();
