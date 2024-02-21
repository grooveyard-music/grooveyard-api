
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Grooveyard.Domain.Entities;
using Grooveyard.Domain.Events;
using Grooveyard.Infrastructure.Data;
using Grooveyard.Infrastructure.Data.Repositories;
using Grooveyard.Infrastructure.Data.Repositories.Interfaces;
using Grooveyard.Infrastructure.DomainEvents.Dispatchers;
using Grooveyard.Infrastructure.DomainEvents.Handlers;
using Grooveyard.Infrastructure.Services;
using Grooveyard.Services;
using Grooveyard.Services.Implementations;
using Grooveyard.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

//var keyVaultEndpoint = new Uri("https://grooveyard.vault.azure.net/");
//builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
//// Obtain the secret from the key vault
//var connectionString = builder.Configuration["grooveyard-connectionstring"]; 

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<GrooveyardDbContext>();
builder.Services.AddDbContext<GrooveyardDbContext>(options =>
    options.UseSqlServer("Server=DESKTOP-9U8OMF1;Database=grooveyard-db;Trusted_Connection=True;TrustServerCertificate=true;",
                                providerOptions => providerOptions.EnableRetryOnFailure()));
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDiscussionService, DiscussionService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IDiscussionRepository, DiscussionRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IUploadRepository, UploadRepository>();
builder.Services.AddScoped<IMediaRepository, MediaRepository>();
builder.Services.AddHttpClient(); 
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
builder.Services.AddTransient<IDomainEventHandler<LikedEvent>, LikedEventHandler>();
builder.Services.AddTransient<IDomainEventHandler<PostCreatedEvent>, PostCreatedEventHandler>();


builder.Services.AddAutoMapper(typeof(UserConfig));
builder.Services.AddAutoMapper(typeof(SocialConfig));
builder.Services.AddAutoMapper(typeof(MediaConfig));

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.WithOrigins("https://localhost:3000", "https://main--resplendent-tulumba-5ce9fb.netlify.app", "https://localhost:5173", "https://localhost:443", "http://localhost/")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials());
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["JWT"];
                return Task.CompletedTask;
            },
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserPolicy", policy => policy.RequireAuthenticatedUser());
});

//builder.Services.AddAuthentication()
//    .AddGoogle(googleOptions =>
//    {
//        googleOptions.ClientId = builder.Configuration["googleClientId"];
//        googleOptions.ClientSecret = builder.Configuration["googleClientSecret"];
//    });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.None;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


//dotnet ef migrations add InitialCreate --startup-project ./Grooveyard.Web --project ./Grooveyard.Infrastructure
//dotnet ef database update --startup-project ./Grooveyard.Web --project ./Grooveyard.Infrastructure
