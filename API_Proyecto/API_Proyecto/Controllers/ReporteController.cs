using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using API_Proyecto.Models;
using System.Linq;

namespace API_Proyecto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReporteController : ControllerBase
    {
        [HttpGet("tareas-a-json")]
        public ActionResult<List<TarjetaTarea>> ObtenerTareasAJSON()
        {
            var listaTuplas = new List<(string id, string titulo, string descripcion, string prioridad, string tipo, string estado, string idProyecto, object dependencias)>
            {
                ("item-001", "Card 1 (Adapted)", "Description for Card 1", "High", "Bug", "active", "1", null),
                ("item-002", "Card 2 (Adapted)", "Description for Card 2", "Medium", "Feature", "active", "1", "item-001"),
                ("item-003", "Card 3 (Adapted)", "Description for Card 3", "Low", "Task", "inactive", "1", "item-002"),
                ("item-004", "Fix the main login flow", "Users are reporting they cannot log in.", "High", "Bug", "inactive", "1", null),
                ("item-005", "Deploy new homepage", "The new homepage is complete and staged.", "Medium", "Feature", "inactive", "1", new List<string> { "item-006", "item-001" }),
                ("item-006", "Refactor database component", "Code review is needed for the new DB logic.", "Medium", "Task", "inactive", "1", "item-004"),
                ("item-007", "Task 7", "Code review is needed for the new DB logic.", "Medium", "Task", "inactive", "1", "item-006"),
                ("item-008", "Task 8", "Code review is needed for the new DB logic.", "Medium", "Task", "active", "1", new List<string> { "item-003", "item-002" }),
                ("item-009", "Task 9", "Scrollinggggg.", "Medium", "Task", "active", "1", new List<string> { "item-003", "item-008" })
            };

            var listaTarjetas = new List<TarjetaTarea>();

            foreach (var tupla in listaTuplas)
            {
                var tarjeta = new TarjetaTarea
                {
                    IDItem = tupla.id,
                    Titulo = tupla.titulo,
                    Descripcion = tupla.descripcion,
                    Prioridad = tupla.prioridad,
                    Tipo = tupla.tipo,
                    Estado = tupla.estado,
                    IDProyecto = tupla.idProyecto,
                    DependeDe = tupla.dependencias
                };
                listaTarjetas.Add(tarjeta);
            }

            return Ok(listaTarjetas);
        }
    }
}