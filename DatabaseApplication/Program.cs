using DatabaseApplication.Data;
using DatabaseApplication.Interfaces;
using DatabaseApplication.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), npgsqlOptions => npgsqlOptions.CommandTimeout(30)).EnableSensitiveDataLogging().LogTo(Console.WriteLine));   // Log to the console;

builder.Services.AddSingleton<UserServiceSession>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<QueryService>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseMiddleware<MiddlewareService>();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
