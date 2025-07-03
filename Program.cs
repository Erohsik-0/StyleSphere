using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using StyleSphere.DataAccess.DbContext;
using StyleSphere.DataAccess.Repositories;
using StyleSphere.Domain.Interfaces.ICart;
using StyleSphere.Domain.Interfaces.IProduct;
using StyleSphere.Mapping;
using StyleSphere.Domain.Entities;
using StyleSphere.Service;
using StyleSphere.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddSession();

// Adding AutoMapper support
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();


// Adding Session support
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer("Server=.;Database=stylesphere_db_v2;Trusted_Connection=True;TrustServerCertificate=True;");
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedEmail = false;

}).AddEntityFrameworkStores<AppDbContext>()
  .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
});

// Register the email sender service
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

// Register the verification store service
builder.Services.AddSingleton<IVerificationStore, InMemoryVerificationStore>();


// Add services to the container.
builder.Services.AddControllersWithViews();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//Must be called before UseRouting and UseEndpoints to ensure session state is available in the request pipeline.
app.UseSession();

app.UseHttpsRedirection();
app.UseRouting();


app.UseAuthorization();

app.MapStaticAssets();

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    

app.UseDefaultFiles();
app.UseStaticFiles();
app.Run();
