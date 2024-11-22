using Gestion_Del_Presupuesto.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace Gestion_Del_Presupuesto.Controllers
{
    public class RegistroController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        // Inyección de dependencias de UserManager para gestionar usuarios
        public RegistroController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: RegistroController
        public ActionResult Index()
        {
            return View();
        }

        // POST: RegistroController/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegistroModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            // Crear un nuevo usuario con los datos del modelo
            var user = new IdentityUser
            {
                UserName = model.Usuarios.Correo,
                Email = model.Usuarios.Correo
            };

            // Registrar al usuario
            var result = await _userManager.CreateAsync(user, model.Usuarios.Contraseña);

            if (result.Succeeded)
            {
                // Enviar correo de verificación
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string link = Url.Action("ConfirmEmail", "Registro", new { userId = user.Id, token }, Request.Scheme);
                EnviarCorreoVerificacion(model.Usuarios.Correo, link);

                return RedirectToAction("Index", "Home");
            }

            // Si hubo errores, agregarlos al ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("Index", model);
        }

        // Método para confirmar el correo electrónico
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return BadRequest("Error al confirmar el correo electrónico.");
        }
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

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            smtp.Send(message);
        }
    }
}
