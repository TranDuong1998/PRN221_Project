using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PRN211_Project.Entities;
using PRN211_Project.Models;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<Prn211ProjectContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MyDB"))
    );

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddAuthentication()
        .AddGoogle(googleOptions =>
        {
            IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");

            googleOptions.ClientId = googleAuthNSection["ClientId"];
            googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
            googleOptions.CallbackPath = "/GoogleLogin";

        });


builder.Services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<Prn211ProjectContext>()
        .AddDefaultTokenProviders();

builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.UseSession();

app.Run();
