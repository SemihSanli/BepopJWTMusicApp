

using BepopJWT.Consume.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();



builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ApiClientHelper>();

builder.Services.AddDistributedMemoryCache(); 
builder.Services.AddSession(options =>        
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/SignIn";
        options.LogoutPath = "/Auth/SignOut";
        options.AccessDeniedPath = "/Auth/AccessDenied";

        options.Cookie.Name = "Bepop.Consume.Auth";
        options.Cookie.HttpOnly = true;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Error/Page/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
