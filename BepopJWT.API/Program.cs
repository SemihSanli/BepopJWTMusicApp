using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.BusinessLayer.Concrete;
using BepopJWT.DataAccessLayer.Abstract;
using BepopJWT.DataAccessLayer.Context;
using BepopJWT.DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



//JWT Kaydým buraya

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey)),

            //Test ortamý olduðu için 0 yaptým. Yani api ile token üretimi arasýnda 5 dakikalýk fark olur, böylece token expire süresi 1 dakika da olsa o 5 dakikalýk farktan dolayý 6 dakikada expire olur.
            //Kafa karýþýklýðý olmamasý için 0'a çektim.
            ClockSkew = TimeSpan.Zero
        };
    });



builder.Services.AddScoped<IAuthService, AuthManager>();
builder.Services.AddScoped<IArtistDal, EfArtistDal>();
builder.Services.AddScoped<IArtistService, ArtistManager>();

builder.Services.AddScoped<IUserDal, EfUserDal>();
builder.Services.AddScoped<IUserService, UsersManager>();
builder.Services.AddScoped<ITokenService, TokenManager>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
