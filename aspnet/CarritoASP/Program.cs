var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// 1. Registrar el almacenamiento en memoria para sesiones
builder.Services.AddDistributedMemoryCache();

// 2. Configurar la sesion con cookie segura
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true; // flag HttpOnly
    options.Cookie.SecurePolicy =
        CookieSecurePolicy.SameAsRequest; // Secure solo si HTTPS (para localhost sin cert)
    options.Cookie.SameSite = SameSiteMode.Strict; // flag SameSite
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseRouting();

// 3. Habilitar el middleware de sesion (ANTES de MapControllers)
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Carrito}/{action=Index}/{id?}");

app.Run();
