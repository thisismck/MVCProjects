using LocalizationFromDB.Data;
using LocalizationFromDB.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<MvcprojectsContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MvcprojectsContext"));
});

// Register the custom localizer factory as a singleton
builder.Services.AddSingleton<IStringLocalizerFactory, DbStringLocalizerFactory>();

// Add localization services
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Configure request localization
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MvcprojectsContext>();
    var supportedCultures = context.LocalizationCultures
        .Select(c => c.CultureCode)
        .ToList();

    var localizationOptions = new RequestLocalizationOptions
    {
        DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US"),
        SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
        SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
    };

    app.UseRequestLocalization(localizationOptions);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();