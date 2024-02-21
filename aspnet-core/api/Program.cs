using api.Middlewares;
using entity.DataContext;
using manager.Implementations;
using manager.Interfaces;
using Microsoft.EntityFrameworkCore;
using repository.Helpers;
using repository.Implementations;
using repository.Interfaces;
using repository.Interfaces.Helper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
#endregion

#region Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); // registering generic repository
#endregion

#region Helpers
builder.Services.AddScoped<IHelperFileHandler, HelperFileHandler>();
#endregion


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

app.UseAuthorization();

app.MapControllers();

app.Run();
