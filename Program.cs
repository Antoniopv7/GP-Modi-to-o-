using Microsoft.EntityFrameworkCore;
using Gestion_Del_Presupuesto.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", false);

// Configurar el servicio de DbContext para usar PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar Identity con soporte para roles
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();  // Esto es necesario para manejar la verificación de correos, restablecer contraseñas, etc.

// Configurar autenticación por cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Página de inicio de sesión
        options.AccessDeniedPath = "/Home/AccessDenied"; // Página de acceso denegado
    });

// Configurar autorización
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin")); // Política para Admin
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));   // Política para User
});

// Agregar servicios para controladores y vistas
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();  // Necesario para las sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Tiempo de inactividad
    options.Cookie.HttpOnly = true;                 // Seguridad
    options.Cookie.IsEssential = true;              // Cumplir con GDPR si es necesario
});

var app = builder.Build();

// Inicialización de roles y usuario administrador
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Crear roles iniciales
    await CrearRoles(services);

    // Crear usuario administrador inicial
    await CrearUsuarioAdmin(services);
}

// Configurar el pipeline de solicitud HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();          // Habilitar el uso de sesiones
app.UseAuthentication();   // Habilitar autenticación
app.UseAuthorization();    // Habilitar autorización

// Configurar las rutas predeterminadas de la aplicación
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Método para crear roles
async Task CrearRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "Dirección", "Coordinación", "Visitante" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

// Método para crear un usuario administrador
async Task CrearUsuarioAdmin(IServiceProvider serviceProvider)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

    // Puedes obtener la contraseña desde una configuración segura
    string email = configuration["AdminUser:Email"];
    string password = configuration["AdminUser:Password"];

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new IdentityUser { UserName = email, Email = email };
        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Admin");

            // Enviar correo de verificación
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = $"https://localhost:5001/Account/ConfirmEmail?userId={user.Id}&token={token}";
            EnviarCorreoVerificacion(user.Email, confirmationLink);
        }
    }
}

// Método para enviar el correo de verificación
void EnviarCorreoVerificacion(string correoDestino, string linkVerificacion)
{
    var fromAddress = new MailAddress("tucorreo@example.com", "Gestión de Campos Clínicos");
    var toAddress = new MailAddress(correoDestino);
    const string fromPassword = "tupassword"; // Usa un servicio de configuración segura para almacenar contraseñas
    const string subject = "Confirma tu correo electrónico";
    string body = $"Por favor, confirma tu cuenta haciendo clic en el siguiente enlace: <a href='{linkVerificacion}'>Confirmar Correo</a>";

    var smtp = new SmtpClient
    {
        Host = "smtp.example.com",
        Port = 587,
        EnableSsl = true,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
    };

    using (var message = new MailMessage(fromAddress, toAddress)
    {
        Subject = subject,
        Body = body,
        IsBodyHtml = true
    })
    {
        smtp.Send(message);
    }
}
