using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});


builder.Services.AddControllersWithViews();




var app = builder.Build();

app.UseStaticFiles();


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
    );




app.MapControllerRoute(
    name: "defaut",
    pattern: "{controller=Home}/{action=Index}/{id?}" 
    );



app.Run();
