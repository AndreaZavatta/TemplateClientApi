using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ClientApi
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Persona> persone = GetPersone();
            Persona persona = new Persona
            {
                Nome = "andrea",
                Cognome = "zavatta"
            };
            SetPersona(persona);

        }

        private static void Set(Object obj, string url)
        {

            WebRequest request = WebRequest.Create("https://localhost:44334" + url);
            request.Method = "POST";
            string json = JsonSerializer.Serialize(obj);
            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            request.ContentLength = byteArray.Length;
            request.ContentType = "application/json";
            Stream dataStreamRequest = request.GetRequestStream();
            dataStreamRequest.Write(byteArray, 0, byteArray.Length);
            dataStreamRequest.Close();
            WebResponse response = request.GetResponse();
            using (Stream dataStreamResponse = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStreamResponse);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                // Display the content.
                Console.WriteLine(responseFromServer);
            }
            response.Close();
        }

        public static void SetPersona(Persona persona)
        {
            Set(persona, "/persona/setpersona");
        }

        private static List<Persona> GetPersone()
        {
            List<Persona> lista = null;
            WebRequest request = WebRequest.Create("https://localhost:44334/persona/GetPersone?cognome=Zavatta");
            WebResponse response = request.GetResponse();
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                lista = JsonSerializer.Deserialize<List<Persona>>(responseFromServer);


                // Display the content.
                Console.WriteLine(responseFromServer);
            }

            // Close the response.
            response.Close();
            return lista;
        }
    }

    public class Persona
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
    }
}
