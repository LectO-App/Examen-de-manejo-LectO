using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace Examen_de_manejo_LectO
{
    class Cuestionario
    {
        public static LiteDatabase db = new LiteDatabase("Filename=database.db; Mode=Exclusive");
        ILiteCollection<Pregunta> preguntas;

        public Cuestionario()
        {
            preguntas = db.GetCollection<Pregunta>("preguntas");
        }

        public void AgregarPregunta (string pregunta, string respuestaCorrecta, string opcion1, string opcion2)
        {
            Pregunta agregar = new Pregunta
            {
                TextoPregunta = pregunta,
                Imagen = "",
                RespuestaCorrecta = respuestaCorrecta,
                Opcion1 = opcion1,
                Opcion2 = opcion2
            };

            preguntas.Insert(agregar);
        }
    }
}
