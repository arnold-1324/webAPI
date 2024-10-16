using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;
using twitterclone.Interfaces;
using twitterclone.Services;
using twitterclone.Repositories;
using twitterclone.HelperClass;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddSingleton<IMongoClient>(new MongoClient(builder.Configuration["MongoDBSettings:ConnectionString"]));

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(builder.Configuration["MongoDBSettings:DatabaseName"]);
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddTransient<IAuthService, EmailSender>();

builder.Services.AddSingleton<EmailTemplate>();
builder.Services.AddSingleton(sp =>
{
    var secretKey = builder.Configuration["JwtSettings:Secret"];
    var subject = builder.Configuration["JwtSettings:Subject"];
    var issuer = builder.Configuration["JwtSettings:Issuer"];
    var audience = builder.Configuration["JwtSettings:Audience"];
    
    
    if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(subject) || 
        string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
    {
        throw new InvalidOperationException("One or more JWT settings are missing from configuration.");
    }

    return new TokenHelper(secretKey, subject, issuer, audience);
});

builder.Services.AddControllers();

var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Secret"] ?? "default-secret");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = true,
        ValidateLifetime = true
    };
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
