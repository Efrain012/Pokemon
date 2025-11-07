namespace Comprehension.Models
{
    public class Token
    {
        public string TokenID { get; set; }
        public Guid UsuarioID { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public Usuario Usuario { get; set; }
    }
}
