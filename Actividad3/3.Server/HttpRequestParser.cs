using HttpMessageParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HttpMessageParser
{
    public class HttpRequestParser : IRequestParser
    {
        // Añadimos el metodo
        public HttpRequest ParseRequest(string requestText)
        {
            // cadenas nulas y vacías
            if (requestText == null)
                throw new ArgumentNullException();

            if (requestText == string.Empty)
                throw new ArgumentException();

            // Dividir request en líneas
            var requestTextLineas = requestText.Split(new[] {"\n"}, StringSplitOptions.None);

            // Primera línea
            var PrimeraLinea = requestTextLineas[0].Trim();

            // Divididir la division
            var PrimeraLineaPartes = PrimeraLinea.Split(' ');

            // Comprobar que tenga exactamente 3 partes y que no estén vacías
            if (PrimeraLineaPartes.Length != 3 || string.IsNullOrWhiteSpace(PrimeraLineaPartes[0]) || string.IsNullOrWhiteSpace(PrimeraLineaPartes[1]) || string.IsNullOrWhiteSpace(PrimeraLineaPartes[2]))
                throw new ArgumentException();

            // Tiene "/"
            if (!PrimeraLineaPartes[1].Contains("/"))
                throw new ArgumentException();

            // Empieza con HTTP
            if (!PrimeraLineaPartes[2].StartsWith("HTTP"))
                throw new ArgumentException();

            // Separar cabeza y cuerpo
            int indiceLineaVacia = Array.FindIndex(requestTextLineas, string.IsNullOrWhiteSpace);
            var encabezados = new Dictionary<string, string>();
            int finEncabezados = indiceLineaVacia == -1 ? requestTextLineas.Length : indiceLineaVacia;

           
            for (int i = 1; i < finEncabezados; i++)
            {
                var lineaEncabezado = requestTextLineas[i].Trim();

                if (string.IsNullOrWhiteSpace(lineaEncabezado))
                    continue;

                var indiceDosPuntos = lineaEncabezado.IndexOf(':');

                if (indiceDosPuntos == -1 || lineaEncabezado.LastIndexOf(':') != indiceDosPuntos)
                    throw new ArgumentException();

                var nombreEncabezado = lineaEncabezado.Substring(0, indiceDosPuntos).Trim();
                var valorEncabezado = lineaEncabezado.Substring(indiceDosPuntos + 1).Trim();

                if (string.IsNullOrWhiteSpace(nombreEncabezado))
                    throw new ArgumentException();

                if (string.IsNullOrWhiteSpace(valorEncabezado))
                    throw new ArgumentException();

                encabezados[nombreEncabezado] = valorEncabezado;
            }

            // Estoy cansado jefe
            string? cuerpo = null;
            if (indiceLineaVacia != -1 && indiceLineaVacia + 1 < requestTextLineas.Length)
            {
                var lineasCuerpo = requestTextLineas.Skip(indiceLineaVacia + 1);
                cuerpo = string.Join("\n", lineasCuerpo);

                if (string.IsNullOrWhiteSpace(cuerpo))
                    cuerpo = null;
            }

            return new HttpRequest()
            {
                Method = PrimeraLineaPartes[0],
                RequestTarget = PrimeraLineaPartes[1],
                Protocol = PrimeraLineaPartes[2],
                Headers = encabezados,
                Body = cuerpo
            };
        }
    }
}
//https://external-preview.redd.it/DYZZBDT-zM7UEOBKfGKWuIVERSu9aXuf0rreL2DMA80.jpg?width=640&crop=smart&auto=webp&s=52963cc6a1a2027b7aae32f82d739445852b4a61