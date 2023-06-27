using BookingManagement___Projekt_zaliczeniowy;
using BookingManagement___Projekt_zaliczeniowy.Abstract;
using BookingManagement___Projekt_zaliczeniowy.Concrete;
using BookingManagement___Projekt_zaliczeniowy.Data;
using BookingManagement___Projekt_zaliczeniowy.Index;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Raven.Client.Documents.Indexes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IHotelRepo, HotelRepo>();
builder.Services.AddScoped<IRoomRepo, RoomRepo>();
builder.Services.AddScoped<IGuestRepo, GuestRepo>();


var app = builder.Build();

IndexCreation.CreateIndexes(typeof(ReservationByTime).Assembly, Session.Store);
IndexCreation.CreateIndexes(typeof(HotelList).Assembly, Session.Store);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
app.MapRazorPages();


using(var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var role = "Admin";

    if (!await roleManager.RoleExistsAsync(role))
    {
        await roleManager.CreateAsync(new IdentityRole(role));
    }
}


app.Run();
