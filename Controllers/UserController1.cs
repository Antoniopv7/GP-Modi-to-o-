using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Gestion_Del_Presupuesto.Models;
using System.Threading.Tasks;
using Gestion_Del_Presupuesto.Data;

public class UsuarioController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;

    public UsuarioController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    // Crear un nuevo usuario
    [HttpPost]
    public async Task<IActionResult> CrearUsuario(string nombre, string correo, string password, string confirmarPassword, string rut, string telefono, int idRol)
    {
        // Validar si el usuario ya existe
        if (await _userManager.FindByEmailAsync(correo) != null)
        {
            return BadRequest($"El usuario con email {correo} ya existe.");
        }

        // Validar si la contraseña y la confirmación coinciden
        if (password != confirmarPassword)
        {
            return BadRequest("Las contraseñas no coinciden.");
        }

        // Crear el usuario en Identity
        var identityUser = new IdentityUser { UserName = correo, Email = correo };
        var result = await _userManager.CreateAsync(identityUser, password);

        if (!result.Succeeded)
        {
            return BadRequest($"Error al crear el usuario: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        // Crear el usuario en el modelo personalizado
        var usuario = new Usuario
        {
            Nombre = nombre,
            Correo = correo,
            Contraseña = password, // Se debe cifrar en la base de datos
            Rut = rut,
            Telefono = telefono,
            Id_Rol = idRol,
            ConfirmarPassword = confirmarPassword // Esta propiedad debería usarse solo para validación, no debería almacenarse.
        };

        // Asignar rol si es proporcionado
        var rol = _context.Roles.Find(idRol);
        if (rol != null)
        {
            usuario.Rol = rol;
            await _userManager.AddToRoleAsync(identityUser, rol.NombreRol);
        }

        // Guardar el usuario en la base de datos
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return Ok($"Usuario {correo} creado con éxito.");
    }
}
