var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//For implementing the same functionality as SessionStorage using C#
builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(3);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true; // Make the session cookie essential 
    }
);


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
