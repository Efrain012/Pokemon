
/*
namespace API_Proyecto.Models
{
    public class Usuario
    {
        private string idUsuario;
        private string correo;
        private string hashCon;
        private string rol;
        private bool activo;
        public Usuario(string idUsuario, string correo, string hashCon, string rol)
        {
            this.idUsuario = idUsuario;
            this.correo = correo;
            this.hashCon = hashCon;
            this.rol = rol;
            this.activo = true;
        }

        public string GetIdUsuario() { return idUsuario; }

        public string GetCorreo() { return correo; }
        public void SetCorreo(string correo) { this.correo = correo; }

        public string GetHashCon() { return hashCon; }
        private void SetHashCon(string hash) { this.hashCon = hash; }

        public string GetRol() { return rol; }
        public void SetRol(string rol) { this.rol = rol; }

        public bool EstaActivo() { return activo; }
        public void Desactivar() { this.activo = false; }

        //
        public void RecuperarCuenta(string correoEntrada,
                                string nuevaContrasena,
                                string codigoVerificacion)
        {
            if (this.correo == correoEntrada && codigoVerificacion != null)
            {
                string nuevoHash =//funcion para generar nuevo hash
                    ;
                SetHashCon(nuevoHash);
            }
        }

        //
        public string RevisarRolEnProyecto(int idProyecto)
        {
            return this.rol;
        }

        //
        public class SesionResultado
        {
            public bool Validado { get; set; }
            public string? Token { get; set; }
        }
        public SesionResultado IniciarSesion(string correoEntrada, string contrasenaEntrada)
        {
            SesionResultado resultado = new SesionResultado();

            if (!this.activo || this.correo != correoEntrada)
            {
                resultado.Validado = false;
                resultado.Token = null;
                return resultado;
            }

            string hashEntrada = //funcion para generar nuevo hash
                    ;
            if (hashEntrada == this.hashCon)
            {
                resultado.Validado = true;
                resultado.Token = //funcion para generar token de sesion
                    ;
            }
            else
            {
                resultado.Validado = false;
                resultado.Token = null;
            }

            return resultado;
        }
    }
}
*/