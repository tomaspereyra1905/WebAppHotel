using Api.Dependencies;
using Serilog;
using Infrastructure;
using Infrastructure.Data;
using Core;
using Core.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
builder.Host.UseSerilog(((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration)));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructure();
builder.Services.AgregarCore();
builder.Services.AgregarAutoMapper();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));


builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    opt.UseSqlite(builder.Configuration.GetConnectionString("ConnectionDb"));
});

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
    pattern: "{controller=Home}/{action=ListHotels}/{id?}");

app.Run();
