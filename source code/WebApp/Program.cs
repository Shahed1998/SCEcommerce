using DataAccess.Data;
using DataAccess.Repository.Implementations;
using DataAccess.Repository.Interfaces;
using Manager.Implementations;
using Manager.Interfaces;
using Microsoft.EntityFrameworkCore;
using Utility.Helpers;
using Utility.Mappings;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddSingleton<HelperEncryption>();


#region Services

builder.Services.AddScoped<ICategoryManager, CategoryManager>(); 
builder.Services.AddScoped<IProductManager, ProductManager>(); 

#endregion

#region Repository 

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

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
    name: "area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");


app.Run();
