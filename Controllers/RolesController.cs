using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gestion_Del_Presupuesto.Controllers
{
    public class RolesController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RolesController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AsignarRol(string email, string role)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role))
            {
                return BadRequest("El email y el rol son obligatorios.");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Usuario con email {email} no encontrado.");
            }

            if (await _userManager.IsInRoleAsync(user, role))
            {
                return BadRequest($"El usuario ya tiene el rol {role} asignado.");
            }

            var result = await _userManager.AddToRoleAsync(user, role);
            if (result.Succeeded)
            {
                return Ok($"Rol {role} asignado correctamente a {email}.");
            }

            return BadRequest("Ocurrió un error al asignar el rol.");
        }
    }
}
