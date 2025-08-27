using HttpMessageParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpMessageParser
{
    public class HttpResponseWriter : IResponseWriter
    {
        public string WriteResponse(HttpResponse response)
        {
            //Comprobacion de los paramatros.
            if (response == null) throw new ArgumentNullException();

            if (string.IsNullOrEmpty(response.Protocol))
                throw new ArgumentException();

            if (response.StatusCode == null)
                throw new ArgumentException();

            if (string.IsNullOrEmpty(response.StatusText))
                throw new ArgumentException();

            if(response.Headers==null)
                throw new ArgumentException();

            StringBuilder EstoyCansadoJefe = new StringBuilder();

            foreach (var head in response.Headers)
            {
                if (string.IsNullOrEmpty(head.Key) || head.Value == null) 
                    throw new ArgumentException();
            }
            // Se llama Bob por bob el constructor
            var Bob = new StringBuilder();

           
            Bob.Append($"{response.Protocol} {response.StatusCode} {response.StatusText}\n");

            if (response.Headers.Count > 0)
                foreach (var header in response.Headers)
                    Bob.Append($"{header.Key}: {header.Value}\n");

          
            Bob.Append("\n");

           
            if (!string.IsNullOrEmpty(response.Body))
                Bob.Append(response.Body);

            return Bob.ToString();
        
    


        }

    }
}
