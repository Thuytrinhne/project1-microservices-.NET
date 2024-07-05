
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
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

builder.Services.AddSingleton<EmailVerificationService>();
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
    options.TokenLifespan = TimeSpan.FromMinutes(Authorization.ResetPasswordExpiredTimeInMinutes));


//Configuration from AppSettings
builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("JWT"));
builder.Services.Configure<MailConfig>(builder.Configuration.GetSection("Mail"));
builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection("MongoDbConfig"));
builder.Services.Configure<CloudinaryConfig>(builder.Configuration.GetSection("CloudinaryConfig"));


// Database services
var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>
        (
            mongoDbSettings.ConnectionString, mongoDbSettings.Name
        ).AddDefaultTokenProviders();

// Cross-cutting services
builder.Services.AddExceptionHandler<CustomExceptionHandler>();


var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);
// Configure the HTTP request pipeline
app.MapCarter();
app.UseExceptionHandler(opts => { });

app.Run();
