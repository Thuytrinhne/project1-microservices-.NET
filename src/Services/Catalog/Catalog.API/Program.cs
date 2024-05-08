var builder = WebApplication.CreateBuilder(args);
// Add services to the dependency container
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("DefaultConnectionString")!);
}).UseLightweightSessions();
var app = builder.Build();

// Configure the HTTP request pipeline to mapping the incoming request 
app.MapCarter();
app.Run();
