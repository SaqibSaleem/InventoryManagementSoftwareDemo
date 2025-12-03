using InventoryManagementSoftwareDemo.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static InventoryManagementSoftwareDemo.Areas.Identity.Pages.Account.RegisterModel;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
	options.Password.RequireDigit = false;
	options.Password.RequiredLength = 3;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequireLowercase = false;
	options.SignIn.RequireConfirmedAccount = false;
	options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
	options.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IUserValidator<ApplicationUser>, CustomUserValidator<ApplicationUser>>();

// Services - NO GLOBAL FILTERS
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configure cookie
builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Identity/Account/Login";
	options.LogoutPath = "/Identity/Account/Logout";
	options.AccessDeniedPath = "/Identity/Account/AccessDenied";
	options.SlidingExpiration = true;

	// ✅ This is the key setting for post-login redirect
	options.ReturnUrlParameter = "returnUrl";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
//app.Use(async (context, next) =>
//{
//	var endpoint = context.GetEndpoint();

//	// Check if this is the home page and user is not authenticated
//	if (!context.User.Identity.IsAuthenticated)
//	{
//		context.Response.Redirect("/Identity/Account/Login");
//		return;
//	}

//	await next();
//});
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Simple mapping - let controller attributes handle auth
app.MapRazorPages();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();