using Gestion_Del_Presupuesto.Data;
using Gestion_Del_Presupuesto.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Gestion_Del_Presupuesto.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LoginController
        public IActionResult Index()
        {
            return View();
        }

        // POST: LoginController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string nombreUsuario, string contraseña)
        {
            // Buscar el usuario en la base de datos con su Rol incluido
            var usuario = _context.Usuarios
                .Include(u => u.Rol)  // Incluir la relación con el Rol
                .FirstOrDefault(u => u.Nombre == nombreUsuario && u.Contraseña == contraseña);

            if (usuario != null)
            {
                // Si el usuario existe, guardamos los datos en la sesión
                HttpContext.Session.SetString("NombreUsuario", usuario.Nombre);
                HttpContext.Session.SetString("RolUsuario", usuario.Rol.Nombre);  // Aquí usamos el nombre del rol

                // Redirigir a la página principal después de iniciar sesión
                return RedirectToAction("Index", "Home");
            }

            // Si las credenciales son incorrectas, mostrar mensaje de error
            ViewBag.Error = "Nombre de usuario o contraseña incorrectos.";
            return View("Index");
        }

        // GET: LoginController/Logout
        public IActionResult Logout()
        {
            // Limpiar la sesión
            HttpContext.Session.Clear();

            // Redirigir al login después de cerrar sesión
            return RedirectToAction("Index");
        }

        // Método que permite acceder al nombre del usuario en el Navbar
        public IActionResult GetNombreUsuario()
        {
            var nombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            return Content(nombreUsuario ?? "Invitado");
        }

        // Método que permite acceder al rol del usuario
        public IActionResult GetRolUsuario()
        {
            var rolUsuario = HttpContext.Session.GetString("RolUsuario");
            return Content(rolUsuario ?? "Sin rol");
        }
    }
}
