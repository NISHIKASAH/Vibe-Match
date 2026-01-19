using System.Text;
using API.Data;
using API.Interfaces;
using API.Middleware;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) //to validate token key , to allow access to loggged in user only  
.AddJwtBearer(options =>
{
    var tokenKey = builder.Configuration["TokenKey"] ?? throw new Exception("Token key not found - program.cs");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true, 
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)) , //using same key for decrypting the token
        ValidateIssuer = false, 
        ValidateAudience = false 
    };
});



var app = builder.Build();



app.MapControllers();
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(x=> x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200","https://localhost:4200"));

//middleware for authenticate and authorization  //this 2 middleware should be after cors (order matter)
app.UseAuthentication(); //who user is
app.UseAuthorization(); // should  user be provided access 
app.Run();
