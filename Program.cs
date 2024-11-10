using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Gestion_Del_Presupuesto.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", false);

// Configurar el servicio de DbContext para usar PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar autenticación por cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";  // La página de inicio de sesión
        options.AccessDeniedPath = "/Home/AccessDenied";  // Página de acceso denegado
    });

// Configurar autorización
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin")); // Política para Admin
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User")); // Política para User
});

// Agregar servicios para controladores y vistas
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();  // Necesario para las sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Tiempo de inactividad
    options.Cookie.HttpOnly = true;  // Seguridad
    options.Cookie.IsEssential = true;  // Cumplir con GDPR si es necesario
});

var app = builder.Build();

// Configurar el pipeline de solicitud HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();  // Habilitar el uso de sesiones
app.UseAuthentication();  // Habilitar autenticación
app.UseAuthorization();   // Habilitar autorización

// Configurar las rutas predeterminadas de la aplicación
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");//Cambiar Home Por Login

app.Run();
