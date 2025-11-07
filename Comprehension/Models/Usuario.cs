namespace Comprehension.Models
{
    public class Usuario
    {
        public Guid UsuarioID { get; set; }
        public required string NombreUsuario { get; set; }
        public required string HashContrasena { get; set; }
        public required string Sal { get; set; }
    }
}
