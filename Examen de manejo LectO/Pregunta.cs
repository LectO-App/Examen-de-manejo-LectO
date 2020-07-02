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
}
