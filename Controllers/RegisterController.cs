using Gestion_Del_Presupuesto.Data;
using Gestion_Del_Presupuesto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Gestion_Del_Presupuesto.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Register
        public IActionResult Index()
        {
            var viewModel = new RegisterViewModel
            {
                // Cargar los roles desde la base de datos
                Roles = _context.Roles.ToList()
            };
            return View(viewModel);
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegisterViewModel model)
        {
            Console.WriteLine("Formulario recibido");  // Depuración para ver si la acción está siendo ejecutada

            if (ModelState.IsValid)
            {
                try
                {
                    // Crear el nuevo usuario
                    // Crear el nuevo usuario
                    var usuario = new Usuario
                    {
                        Nombre = model.NombreUsuario,
                        Email = model.Email,
                        Contraseña = model.Contraseña,  // En un caso real, deberías encriptar la contraseña
                        Id_Rol = model.Id_Rol,
                        Rut = model.Rut,               // Nuevo campo
                        Telefono = model.Telefono      // Nuevo campo
                    };

                    // Agregar el usuario al contexto
                    _context.Usuarios.Add(usuario);

                    // Guardar los cambios en la base de datos
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Usuario registrado exitosamente");  // Depuración

                    return RedirectToAction("Index", "Login");
                }
                catch (Exception ex)
                {
                    // Si ocurre un error, mostrarlo en la consola
                    Console.WriteLine($"Error al guardar el usuario: {ex.Message}");
                }
            }
            else
            {
                // Si el modelo no es válido, mostrar los errores
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");  // Mostrar errores en consola
                }
            }

            // Recargar los roles para mostrar en el formulario
            model.Roles = _context.Roles.ToList();
            return View(model);
        }
    }
}
