var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDistributedMemoryCache(); // For session storage
builder.Services.AddHttpContextAccessor();    // To access HTTP context
builder.Services.AddSession();                // Adds session state support
builder.Services.AddControllersWithViews();   // Add MVC pattern support

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    // Use exception handler in non-development environments
    app.UseExceptionHandler("/Home/Error");
}

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Enable serving static files (e.g., CSS, JS, images)
app.UseStaticFiles();

// Enable session middleware
app.UseSession(); // Must come before UseRouting and UseAuthorization

// Enable authentication middleware for securing endpoints
app.UseAuthentication();

// Configure request routing
app.UseRouting();

// Enable authorization middleware to enforce user access policies
app.UseAuthorization();

// Map default route with area support
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Teacher}/{controller=Auth}/{action=Login}/{id?}"
);

// Run the application
app.Run();
