using DataAccess.Data;
using DataAccess.Repository.Implementations;
using DataAccess.Repository.Interfaces;
using Manager.Implementations;
using Manager.Interfaces;
using Microsoft.EntityFrameworkCore;
using Utility.Helpers;
using Utility.Mappings;
using Microsoft.AspNetCore.Identity;
using Utility.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// With Email confirmation
//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

// Adds default identity customizations, good for quick setup
//builder.Services.AddDefaultIdentity<IdentityUser>()
//                .AddEntityFrameworkStores<ApplicationDbContext>();

// Allows customization and roles
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddSingleton<HelperEncryption>();

#region Services

builder.Services.AddScoped<IEmailSender, EmailSender>();
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

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");


app.Run();
