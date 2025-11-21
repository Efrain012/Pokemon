using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace API_Proyecto.Models
{
    public class TarjetaTarea
    {
        [JsonPropertyName("id")]
        public string IDItem { get; set; }

        [JsonPropertyName("title")]
        public string Titulo { get; set; }

        [JsonPropertyName("description")]
        public string Descripcion { get; set; }

        [JsonPropertyName("priority")]
        public string Prioridad { get; set; }

        [JsonPropertyName("type")]
        public string Tipo { get; set; }

        [JsonPropertyName("status")]
        public string Estado { get; set; }

        [JsonPropertyName("projectId")]
        public string IDProyecto { get; set; }

        [JsonPropertyName("dependsOn")]
   
        public object DependeDe { get; set; }
    }
}