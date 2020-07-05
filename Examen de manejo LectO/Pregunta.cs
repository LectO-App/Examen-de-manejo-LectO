using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// using LiteDB;

namespace Examen_de_manejo_LectO
{
    public class Pregunta
    {
        public int Id { get; set; }
        public string TextoPregunta { get; set; }
        public string Imagen { get; set; }
        public string RespuestaCorrecta { get; set; }
        public string Opcion1 { get; set; }
        public string Opcion2 { get; set; }
    }

    public class Opcion
    {
        public string texto { get; set; }
        public bool esCorrecto { get; set; }
    }

    public class PreguntaRandomizada
    {
        public string pregunta { get; set; }
        public List<Opcion> opciones { get; set; }
        public string imagen { get; set; }
        public int respondido { get; set; }

        public PreguntaRandomizada()
        {
            respondido = -1;
        }
    }

    [Serializable]
    public class Resultado
    {
        public string numeroDocumento { get; set; }
        public string nombre { get; set; }
        public int aciertos { get; set; }
        public int totalPreguntas { get; set; }
        public List<PreguntaRandomizada> preguntas { get; set; }
    }

    public static class Shuffle
    {
        public static void List<T> (ref List<T> lista)
        {
            int n = lista.Count();
            Random random = new Random();

            for (int i = 0; i < n; i++)
            {
                int change = random.Next(n);
                T value = lista[i];
                lista[i] = lista[change];
                lista[change] = value;
            }
        }
    }
}
