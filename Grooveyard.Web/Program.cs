
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
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

var secretClient = new SecretClient(new Uri("https://grooveyard.vault.azure.net/"), new DefaultAzureCredential());

// Obtain the secret from the key vault
KeyVaultSecret connectionString = secretClient.GetSecret("grooveyard-connectionstring").Value;
KeyVaultSecret googleClientId = secretClient.GetSecret("googleClientId").Value;
KeyVaultSecret googleClientSecret = secretClient.GetSecret("googleClientSecret").Value;
KeyVaultSecret facebookClientId = secretClient.GetSecret("facebookClientId").Value;
KeyVaultSecret facebookClientSecret = secretClient.GetSecret("facebookClientSecret").Value;


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
builder.Services.AddScoped<ICommunityService, CommunityService>();
builder.Services.AddScoped<IDiscussionRepository, DiscussionRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUploadService, UploadService>();
builder.Services.AddScoped<IUploadRepository, UploadRepository>();
builder.Services.AddAutoMapper(typeof(UserProfile));

builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = googleClientId.ToString();
        googleOptions.ClientSecret = googleClientSecret.ToString();
    })
    .AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = facebookClientId.ToString();
        facebookOptions.AppSecret = facebookClientSecret.ToString();
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
