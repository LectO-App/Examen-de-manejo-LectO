using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;
using LiteDB;

namespace Examen_de_manejo_LectO
{
    
    [Serializable]
    class Cuestionario
    {
        List<PreguntaRandomizada> preguntasExamen;
        int cantidadPreguntas;

        public Cuestionario(int cantidad = 30)
        {
            cantidadPreguntas = cantidad;

            preguntasExamen = new List<PreguntaRandomizada>();
            List<Pregunta> todasPreguntas = preguntas.FindAll().ToList();
            
            Shuffle.List(ref todasPreguntas);

            for (int i = 0, index = 0; i < cantidad; i++)
            {
                PreguntaRandomizada actual = new PreguntaRandomizada();
                Pregunta elemento = todasPreguntas[index];

                List <Opcion> temp = new List<Opcion>()
                {
                    new Opcion {texto = elemento.RespuestaCorrecta, esCorrecto = true},
                    new Opcion {texto = elemento.Opcion1, esCorrecto = false},
                    new Opcion {texto = elemento.Opcion2, esCorrecto = false},
                };

                Shuffle.List(ref temp);

                preguntasExamen.Add(new PreguntaRandomizada
                {
                    pregunta = elemento.TextoPregunta,
                    opciones = temp,
                    imagen = elemento.Imagen
                });

                index++;
                if (index == todasPreguntas.Count) index = 0;
            }
        }

        public void pregunta(int numeroPregunta, ref Bitmap imagen, ref int respondida, ref List<string> listaPregunta)
        {
            numeroPregunta--;
            if (!(numeroPregunta >= 0 && numeroPregunta < cantidadPreguntas)) throw new Exception("No existe esta pregunta. Asegúrate de ingresar un valor entre 1 y la cantidad de preguntas.");

            PreguntaRandomizada estaPregunta = preguntasExamen[numeroPregunta];

            imagen = new Bitmap(estaPregunta.imagen);
            respondida = estaPregunta.respondido;

            listaPregunta = new List<string>();
            listaPregunta.Add(estaPregunta.pregunta);

            foreach (var g in estaPregunta.opciones) listaPregunta.Add(g.texto);
        }

        public void responder (int numeroPregunta, int opcion)
        {
            numeroPregunta--;
            if (!(numeroPregunta >= 0 && numeroPregunta < cantidadPreguntas)) throw new Exception("No existe esta pregunta. Asegúrate de ingresar un valor entre 1 y la cantidad de preguntas.");
            if (!(opcion > 0 && opcion <= 3)) throw new Exception("Asegúrate que la opcion sea un número entre 1 y 3.");

            preguntasExamen[numeroPregunta].respondido = opcion;
        }

        public void terminar (string documento, string nombreUsuario)
        {
            int respuestasCorrectas = 0;
            foreach (var g in preguntasExamen) if (g.respondido > 0 && g.opciones[g.respondido - 1].esCorrecto) respuestasCorrectas++;
            Resultado resultado = new Resultado
            {
                numeroDocumento = documento,
                nombre = nombreUsuario,
                aciertos = respuestasCorrectas,
                totalPreguntas = cantidadPreguntas,
                preguntas = preguntasExamen
            };

            string json = JsonConvert.SerializeObject(resultado);
            File.WriteAllText("resultado.json",json);
        }


        // A partir de acá elementos estáticos
        static LiteDatabase db = new LiteDatabase("Filename=database.db; Mode=Exclusive");
        static ILiteCollection<Pregunta> preguntas = db.GetCollection<Pregunta>("preguntas");

        public static void AgregarPregunta (string pregunta, string respuestaCorrecta, string opcion1, string opcion2, string direccionImagen)
        {
            Bitmap bitmap = new Bitmap(direccionImagen);

            if (!Directory.Exists("Imagenes")) Directory.CreateDirectory("Imagenes");

            int i = 0; for (; File.Exists("Imagenes/" + i + ".png"); i++) continue;

            string idImagen = "Imagenes/" + i + ".png";
            bitmap.Save(idImagen);

            Pregunta agregar = new Pregunta
            {
                TextoPregunta = pregunta,
                Imagen = idImagen,
                RespuestaCorrecta = respuestaCorrecta,
                Opcion1 = opcion1,
                Opcion2 = opcion2
            };

            string id = preguntas.Insert(agregar);
        }
    }
}
