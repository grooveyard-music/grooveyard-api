
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Grooveyard.Domain.Interfaces.Repositories.Media;
using Grooveyard.Domain.Interfaces.Repositories.Social;
using Grooveyard.Domain.Interfaces.Repositories.User;
using Grooveyard.Domain.Interfaces.Services.Media;
using Grooveyard.Domain.Interfaces.Services.Social;
using Grooveyard.Domain.Interfaces.Services.User;
using Grooveyard.Infrastructure.Data;
using Grooveyard.Infrastructure.Repositories;
using Grooveyard.Services;
using Grooveyard.Services.MediaService;
using Grooveyard.Services.SocialSercice;
using Grooveyard.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());


// Obtain the secret from the key vault
var connectionString = builder.Configuration["grooveyard-connectionstring"]; 

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<GrooveyardDbContext>();
builder.Services.AddDbContext<GrooveyardDbContext>(options =>
    options.UseSqlServer(connectionString.ToString(),
                                providerOptions => providerOptions.EnableRetryOnFailure()));
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDiscussionService, DiscussionService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<ICommunityService, CommunityService>();
builder.Services.AddScoped<IDiscussionRepository, DiscussionRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IUploadRepository, UploadRepository>();
builder.Services.AddScoped<IMediaRepository, MediaRepository>();
builder.Services.AddAutoMapper(typeof(UserProfile));

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.WithOrigins("https://localhost:3000")
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
            ValidIssuer = builder.Configuration["Jwt-Issuer"],
            ValidAudience = builder.Configuration["Jwt-Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt-Key"]))
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

builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["googleClientId"];
        googleOptions.ClientSecret = builder.Configuration["googleClientSecret"];
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
