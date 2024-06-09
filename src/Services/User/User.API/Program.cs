
using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Microsoft.Extensions.Configuration;
using User.API.Models;
using User.API.Service;
using User.API.Service.IService;

var builder = WebApplication.CreateBuilder(args);

// Application services
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));

});
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddSingleton<EmailVerificationService>();



//Configuration from AppSettings
builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("JWT"));
builder.Services.Configure<MailConfig>(builder.Configuration.GetSection("Mail"));
builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection("MongoDbConfig"));


// Database services
var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>
        (
            mongoDbSettings.ConnectionString, mongoDbSettings.Name
        );

// Cross-cutting services
builder.Services.AddExceptionHandler<CustomExceptionHandler>();


var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();
app.UseExceptionHandler(opts => { });

app.Run();
