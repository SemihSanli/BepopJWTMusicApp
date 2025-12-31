using BepopJWT.BusinessLayer.Abstract;
using BepopJWT.BusinessLayer.Concrete;
using BepopJWT.BusinessLayer.Options.CloudinaryOptions;
using BepopJWT.BusinessLayer.Options.IyzicoOptions;
using BepopJWT.BusinessLayer.Options.OpenAIOptions;
using BepopJWT.DataAccessLayer.Abstract;
using BepopJWT.DataAccessLayer.Context;
using BepopJWT.DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<IyzicoSettings>(builder.Configuration.GetSection("Iyzipay"));
builder.Services.Configure<PaymentSettings>(builder.Configuration.GetSection("PaymentSettings"));
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.Configure<OpenAISettings>(builder.Configuration.GetSection(OpenAISettings.OpenAI));

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
builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IPackageDal,EfPackageDal>();
builder.Services.AddScoped<IPackageService, PackageManager>();
builder.Services.AddScoped<IPaymentDal, EfPaymentDal>();
builder.Services.AddScoped<IPaymentService, PaymentManager>();
builder.Services.AddScoped<IIyzicoService, IyzicoManager>();
builder.Services.AddScoped<IOrderDal, EfOrderDal>();
builder.Services.AddScoped<IOrderService, OrderManager>();
builder.Services.AddScoped<IUserDal, EfUserDal>();
builder.Services.AddScoped<IUserService, UsersManager>();
builder.Services.AddScoped<ITokenService, TokenManager>();
builder.Services.AddScoped<ISongDal, EfSongDal>();
builder.Services.AddScoped<ISongService, SongManager>();
builder.Services.AddScoped<IFileUploadService, FileUploadManager>();
builder.Services.AddScoped<IPlaylistDal, EfPlaylistDal>();
builder.Services.AddScoped<IPlaylistSongDal, EfPlaylistSongDal>();
builder.Services.AddScoped<IPlayListService, PlaylistManager>();
builder.Services.AddScoped<IMLRecommendationService,MLRecommendationManager>();
builder.Services.AddHttpClient<IOpenAIService,OpenAIManager>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "BepopJWT API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Bearer {token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
            .AllowAnyOrigin()  
            .AllowAnyMethod()  
            .AllowAnyHeader(); 
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
