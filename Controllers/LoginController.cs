using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gestion_Del_Presupuesto.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: LoginController
        public IActionResult Index()
        {
            return View();
        }

        // POST: LoginController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "El email y la contraseña son obligatorios.";
                return View("Index");
            }

            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                ViewBag.Error = "La cuenta está bloqueada. Intenta más tarde.";
            }
            else
            {
                ViewBag.Error = "Credenciales incorrectas.";
            }

            return View("Index");
        }

        // GET: LoginController/Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        // Método para obtener el nombre del usuario actual
        public IActionResult GetNombreUsuario()
        {
            var nombreUsuario = User.Identity?.Name;
            return Content(nombreUsuario ?? "Invitado");
        }

        // Método para obtener el rol del usuario actual
        public async Task<IActionResult> GetRolUsuario()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Content("Sin rol");
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var roles = await _userManager.GetRolesAsync(user);
            return Content(roles.FirstOrDefault() ?? "Sin rol");
        }
    }
}
