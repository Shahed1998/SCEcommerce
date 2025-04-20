using DataAccess.Data;
using DataAccess.Repository.Implementations;
using DataAccess.Repository.Interfaces;
using Manager.Implementations;
using Manager.Interfaces;
using Microsoft.EntityFrameworkCore;
using Utility.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<HelperEncryption>();


#region Services

builder.Services.AddScoped<ICategoryManager, CategoryManager>();

#endregion

#region Repository 

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
