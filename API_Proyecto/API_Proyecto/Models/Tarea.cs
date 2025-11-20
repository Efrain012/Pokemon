using static System.Runtime.InteropServices.JavaScript.JSType;

/*
namespace API_Proyecto.Models
{

    public class Tarea
    {
        private string _id;
        private string _name;
        private Tarea padre;
        private string ProjectID;
        //
        private DateTime deadline;
        //
        private Usuario responsable;


        public Tarea(string nombre, DateTime deadline, string idProyecto)
        {
            this._id = GenerarIdTarea();  // crear funcion para generar ID
            this._name = nombre;
            this.deadline = deadline;
            this.ProjectID = idProyecto;
        }

        public string getIdTarea() { return _id; }

        public string getNombre() { return _name; }
        public void setNombre(string nombre) { this._name = nombre; }

        public Tarea getPadre() { return padre; }

        public string getIdProyecto() { return ProjectID; }
        public void setIdProyecto(string idProyecto) { this.ProjectID = idProyecto; }

        public void asignarPadre(Tarea tareaPadre)
        {
            this.padre = tareaPadre;
        }
        public DateTime GetDeadline() { return deadline; }
        public void SetDeadline(DateTime deadline) { this.deadline = deadline; }

        //
        public Usuario GetResponsable() { return responsable; }
        public void AsignarResponsable(Usuario usuario)
        {
            this.responsable = usuario;
        }


        public void actualizarDatos(string nuevoNombre, DateTime nuevoDeadline)
        {
            int nNumlen = nuevoNombre.Length;
            if (nuevoNombre != null && nNumlen > 0)
            {
                this._name = nuevoNombre;
            }

            this.deadline = nuevoDeadline;

        }

        private string GenerarIdTarea()
        {
            // Posiblemnte cambiarlo despues
            long ticks = DateTime.Now.Ticks;
            return "T" + ticks;
        }
    }
}
*/