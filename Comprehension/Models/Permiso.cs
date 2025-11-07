namespace Comprehension.Models
{
    public class Permiso
    {
        public Guid PermisoID { get; set; }
        public Guid RecursoID { get; set; }
        public string TipoRecurso { get; set; }
        public Guid UsuarioID { get; set; }
        public string Rol { get; set; }
        public Usuario Usuario { get; set; }
    }
}