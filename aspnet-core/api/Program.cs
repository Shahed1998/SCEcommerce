using api.Middlewares;
using entity.DataContext;
using manager.Implementations;
using manager.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using repository.Helpers;
using repository.Implementations;
using repository.Interfaces;
using repository.Interfaces.Helper;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Creates web root directory at the start of the application, if it doesnot exist
CreateWwwRootDirectory();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region DatabaseContext Register
// Register DatabaseContext
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion

#region Services
builder.Services.AddScoped<ICategoryManager, CategoryManager>();
builder.Services.AddScoped<IAccountManager, AccountManager>();
#endregion

#region Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); // registering generic repository
#endregion

#region Helpers
builder.Services.AddScoped<IHelperFileHandler, HelperFileHandler>();
#endregion

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetSection("Settings:JWTSecretKey").Value!)),

        // Set token expiration
        ValidateLifetime = true,
        RequireExpirationTime = true,

        // Set the clock skew to account for any server-client time synchronization issues
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build(); // Need fix

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// for testing purpose
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


static void CreateWwwRootDirectory()
{
    try
    {
        string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        if (!Directory.Exists(wwwRootPath))
        {
            Directory.CreateDirectory(wwwRootPath);
        }
    }
    catch (Exception ex)
    {
        throw new Exception($"{ex}");
    }
}
