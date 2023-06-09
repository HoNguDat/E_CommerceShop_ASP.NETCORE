using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ASP.NETCORE_PROJECT.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
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
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "Manager", "Employee", "Customer" };
    foreach (var item in roles)
    {
        if (!await roleManager.RoleExistsAsync(item))
        {
            await roleManager.CreateAsync(new IdentityRole(item));
        }
    }
}
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    string email = "hongudat@gmail.com";
    string pass = "Ngudat4102.";
    string fullname = "Hồ Ngũ Đạt";
    string address = "Quận 9";
    string phone = "0825875052";
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new ApplicationUser();
        user.UserName = email;
        user.Email = email;
        user.FullName = fullname;
        user.Address = address;
        user.PhoneNumber = phone;
        var result = await userManager.CreateAsync(user, pass);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }


    }
}
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    string email = "honguduc@gmail.com";
    string pass = "Nguduc8.";
    string fullname = "Hồ Ngũ Đức";
    string address = "Quận 9";
    string phone = "0918130560";
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new ApplicationUser();
        user.UserName = email;
        user.Email = email;
        user.FullName = fullname;
        user.Address = address;
        user.PhoneNumber = phone;
        var result = await userManager.CreateAsync(user, pass);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Manager");
        }


    }
}
app.MapRazorPages();
app.Run();
