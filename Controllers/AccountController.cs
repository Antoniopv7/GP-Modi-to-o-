using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Gestion_Del_Presupuesto.Models;

public class AccountController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    // Método para registrar un nuevo usuario
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegistroModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new IdentityUser
        {
            UserName = model.Usuarios.Nombre,
            Email = model.Usuarios.Correo
        };

        var result = await _userManager.CreateAsync(user, model.Usuarios.Contraseña);

        if (result.Succeeded)
        {
            // Generar el token de verificación
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

            // Enviar el correo de verificación
            EnviarCorreoVerificacion(model.Usuarios.Correo, confirmationLink);

            return RedirectToAction("ConfirmRegistration");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    // Método para confirmar el correo electrónico
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (userId == null || token == null)
        {
            return BadRequest("Error al confirmar el correo electrónico.");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound($"No se encontró el usuario con el ID '{userId}'.");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return View("EmailConfirmed");
        }

        return BadRequest("Error al confirmar el correo electrónico.");
    }

    // Método para enviar el correo de verificación
    private void EnviarCorreoVerificacion(string correoDestino, string linkVerificacion)
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

    // Método para cerrar sesión
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
