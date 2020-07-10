using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;
using System.Net.Cache;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Examen_de_manejo_LectO
{
    
    [Serializable]
    public class Cuestionario
    {
        List<PreguntaRandomizada> preguntasExamen;
        public int cantidadPreguntas;

        public Cuestionario()
        {
            Task.Run(UpdateDB).Wait();
            preguntasExamen = new List<PreguntaRandomizada>();

            string str = File.ReadAllText("db.json");
            DB db = JsonConvert.DeserializeObject<DB>(str);

            int cantidadExamenes = db.examenes.Count;
            Random random = new Random();
            int examen = random.Next(cantidadExamenes);
            cantidadPreguntas = db.examenes[examen].preguntas.Count;


            for (int i = 0; i < db.examenes[examen].preguntas.Count; i++)
            {
                PreguntaRandomizada actual = new PreguntaRandomizada();
                Pregunta elemento = db.examenes[examen].preguntas[i];

                List <Opcion> temp = new List<Opcion>()
                {
                    new Opcion {texto = elemento.respuestaCorrecta, esCorrecto = true},
                    new Opcion {texto = elemento.opcion1, esCorrecto = false},
                    new Opcion {texto = elemento.opcion2, esCorrecto = false},
                };

                Shuffle.List(ref temp);

                preguntasExamen.Add(new PreguntaRandomizada
                {
                    pregunta = elemento.txtPregunta,
                    opciones = temp,
                    imagen = elemento.imagen
                });
            }
        }

        public void pregunta(int numeroPregunta, ref string imagen, ref int respondida, ref List<string> listaPregunta)
        {
            numeroPregunta--;
            if (!(numeroPregunta >= 0 && numeroPregunta < cantidadPreguntas)) throw new Exception("No existe esta pregunta. Asegúrate de ingresar un valor entre 1 y la cantidad de preguntas.");

            PreguntaRandomizada estaPregunta = preguntasExamen[numeroPregunta];

            respondida = estaPregunta.respondido;

            imagen = "Images/" + estaPregunta.imagen + ".png";
            imagen = Path.GetFullPath(imagen);

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

        public void terminar (string numeroDocumento, string nombreUsuario)
        {
            int respuestasCorrectas = 0;
            foreach (var g in preguntasExamen) if (g.respondido > 0 && g.opciones[g.respondido - 1].esCorrecto) respuestasCorrectas++;
            Resultado resultado = new Resultado
            {
                numeroDocumento = numeroDocumento,
                nombre = nombreUsuario,
                aciertos = respuestasCorrectas,
                totalPreguntas = cantidadPreguntas,
                preguntas = preguntasExamen
            };

            string json = JsonConvert.SerializeObject(resultado);
            File.WriteAllText("resultado.json",json);
            SendJson(json, "https://lecto-api.herokuapp.com/api/results");
        }

        public async Task SendJson (string json, string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.StatusCode != HttpStatusCode.OK) MessageBox.Show("No se envió correctamente la prueba. Contacta a un administrador.");
        }

        public static async Task UpdateDB()
        {
            try
            {
                HttpClient client = new HttpClient();
                WebClient wclient = new WebClient();

                string result = await client.GetStringAsync("https://lecto-api.herokuapp.com/api/exams");
                result = "{\"examenes\":" + result + "}"; 
                DB db = JsonConvert.DeserializeObject<DB>(result);

                if (!Directory.Exists("Images")) Directory.CreateDirectory("Images");

                foreach (Examen examen in db.examenes)
                {
                    foreach (Pregunta pregunta in examen.preguntas)
                    {
                        if (pregunta.imagen != null)
                        {
                            string url = pregunta.imagen;
                            for (int i = 0; i < pregunta.imagen.Length; i++) if (pregunta.imagen[i] == '/' || pregunta.imagen[i] == ':') pregunta.imagen = pregunta.imagen.Remove(i--, 1);

                            if (!File.Exists("Images/" + pregunta.imagen + ".png"))
                            {
                                Stream stream = wclient.OpenRead(url);
                                Bitmap bitmap = new Bitmap(stream);

                                if (bitmap != null)
                                {
                                    bitmap.Save("Images/" + pregunta.imagen + ".png");
                                }

                                stream.Flush();
                                stream.Close();
                            }

                            if (pregunta.txtPregunta == null) pregunta.txtPregunta = "¿?";
                        }
                    }
                }

                File.WriteAllText("db.json", JsonConvert.SerializeObject(db));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}