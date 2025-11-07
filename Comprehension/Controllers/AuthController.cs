using Comprehension.Data;
using Comprehension.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Comprehension.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ComprehensionContext db;

        public AuthController(ComprehensionContext context)
        {
            db = context;
        }

        [HttpPost("Registro")]
        public IActionResult Registro([FromBody] RegistroRequest request)
        {
            var usuarioExiste = db.Usuarios.Any(u => u.NombreUsuario == request.NombreUsuario);
            if (usuarioExiste)
            {
                return BadRequest("Ya existe el usuario");
            }

            var sal = GenerarSal();
            var hash = GenerarHash(request.Contrasena, sal);

            var usuario = new Usuario
            {
                UsuarioID = Guid.NewGuid(),
                NombreUsuario = request.NombreUsuario,
                HashContrasena = hash,
                Sal = sal
            };

            db.Usuarios.Add(usuario);
            db.SaveChanges();

            return Ok(new { mensaje = "Usuario registrado" });
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var usuario = db.Usuarios.FirstOrDefault(u => u.NombreUsuario == request.NombreUsuario);
            if (usuario == null)
            {
                return Unauthorized("Usuario o contraseña incorrectos");
            }

            var hash = GenerarHash(request.Contrasena, usuario.Sal);
            if (hash != usuario.HashContrasena)
            {
                return Unauthorized("Usuario o contraseña incorrectos");
            }

            var tokenID = GenerarTokenID();
            var token = new Token
            {
                TokenID = tokenID,
                UsuarioID = usuario.UsuarioID,
                FechaCreacion = DateTime.UtcNow,
                FechaExpiracion = DateTime.UtcNow.AddMinutes(60)
            };

            db.Tokens.Add(token);
            db.SaveChanges();

            return Ok(new { token = tokenID });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return BadRequest("Token no proporcionado");
            }

            var tokenID = authHeader.Substring(7);
            var token = db.Tokens.FirstOrDefault(t => t.TokenID == tokenID);

            if (token != null)
            {
                db.Tokens.Remove(token);
                db.SaveChanges();
            }

            return Ok(new { mensaje = "Sesion cerrada" });
        }

        private string GenerarSal()
        {
            var bytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes);
        }

        private string GenerarHash(string contrasena, string sal)
        {
            var combinado = contrasena + sal;
            var bytes = Encoding.UTF8.GetBytes(combinado);
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        private string GenerarTokenID()
        {
            var bytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return Convert.ToHexString(bytes);
        }
    }

    public class RegistroRequest
    {
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
    }

    public class LoginRequest
    {
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
    }
}