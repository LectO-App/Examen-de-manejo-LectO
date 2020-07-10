using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Speech.Synthesis;
using System.Timers;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using Pruebas_WPF_2;

namespace Examen_de_manejo_LectO.Pages
{
    /// <summary>
    /// Lógica de interacción para Pag_Preguntas.xaml
    /// </summary>
    public partial class Pag_Preguntas : Page
    {
        public Cuestionario cuestionario;
        SpeechSynthesizer synth;
        static BrushConverter converter = new BrushConverter();
        int preguntaActual = 1;
        public string nombreApellido;
        public string documento;
        Button btnAnterior;
        Pag_Home pantallaInicial;

        public Pag_Preguntas(Pag_Home home)
        {
            InitializeComponent();
            cuestionario = new Cuestionario();
            synth = new SpeechSynthesizer();
            btnAnterior = preg1;
            cambiaPregunta(1);
            pantallaInicial = home;
            nombreApellido = home.nombreApellido;
            documento = home.documento;
        }

        // ==============
        // TEXT TO SPEECH
        // ==============

        private void ttsPregunta_Click(object sender, RoutedEventArgs e)
        {
            synth.SpeakAsyncCancelAll();
            synth.SpeakAsync(new TextRange(textBoxPregunta.Document.ContentStart, textBoxPregunta.Document.ContentEnd).Text);
        }

        private void ttsA_Click(object sender, RoutedEventArgs e)
        {
            synth.SpeakAsyncCancelAll();
            synth.SpeakAsync("Opción A \n" + new TextRange(textBoxA.Document.ContentStart, textBoxA.Document.ContentEnd).Text);
        }

        private void ttsB_Click(object sender, RoutedEventArgs e)
        {
            synth.SpeakAsyncCancelAll();
            synth.SpeakAsync("Opción B \n" + new TextRange(textBoxB.Document.ContentStart, textBoxB.Document.ContentEnd).Text);
        }

        private void ttsC_Click(object sender, RoutedEventArgs e)
        {
            synth.SpeakAsyncCancelAll();
            synth.SpeakAsync("Opción C \n" + new TextRange(textBoxC.Document.ContentStart, textBoxC.Document.ContentEnd).Text);
        }

        private void ttsGeneral_Click(object sender, RoutedEventArgs e)
        {
            synth.SpeakAsyncCancelAll();
            synth.SpeakAsync(new TextRange(textBoxPregunta.Document.ContentStart, textBoxPregunta.Document.ContentEnd).Text + "\nOpción A \n" + new TextRange(textBoxA.Document.ContentStart, textBoxA.Document.ContentEnd).Text + "\nOpción B \n" + new TextRange(textBoxB.Document.ContentStart, textBoxB.Document.ContentEnd).Text + "\nOpción C \n" + new TextRange(textBoxC.Document.ContentStart, textBoxC.Document.ContentEnd).Text);
        }

        // ======================
        // RESPUESTAS A PREGUNTAS
        // ======================

        System.Windows.Media.Brush colorNoSeleccionado = (System.Windows.Media.Brush)converter.ConvertFromString("#FFDDDDDD");
        System.Windows.Media.Brush colorSeleccionado = (System.Windows.Media.Brush)converter.ConvertFromString("#FF8AB0FF");

        private void btnA_Click(object sender, RoutedEventArgs e)
        {
            cuestionario.responder(preguntaActual, 1);
            btnA.Background = colorSeleccionado;
            btnB.Background = colorNoSeleccionado;
            btnC.Background = colorNoSeleccionado;
        }

        private void btnB_Click(object sender, RoutedEventArgs e)
        {
            cuestionario.responder(preguntaActual, 2);
            btnA.Background = colorNoSeleccionado;
            btnB.Background = colorSeleccionado;
            btnC.Background = colorNoSeleccionado;
        }

        private void btnC_Click(object sender, RoutedEventArgs e)
        {
            cuestionario.responder(preguntaActual, 3);
            btnA.Background = colorNoSeleccionado;
            btnB.Background = colorNoSeleccionado;
            btnC.Background = colorSeleccionado;
        }

        // =================
        // MOSTRAR PREGUNTAS
        // =================

        private void cambiaPregunta(int numeroPregunta)
        {
            preguntaActual = numeroPregunta;

            string imagen = "";
            int respondida = 0;
            List<string> listaPreguntas = new List<string>();

            cuestionario.pregunta(numeroPregunta, ref imagen, ref respondida, ref listaPreguntas);
            Uri resourceUri = new Uri(imagen);
            MessageBox.Show(resourceUri.AbsoluteUri);
            imgPregunta.Source = new BitmapImage(resourceUri);


            textBoxPregunta.Document.Blocks.Clear();
            textBoxPregunta.Document.Blocks.Add(new Paragraph(new Run(listaPreguntas[0])));

            textBoxA.Document.Blocks.Clear();
            textBoxA.Document.Blocks.Add(new Paragraph(new Run(listaPreguntas[1])));

            textBoxB.Document.Blocks.Clear();
            textBoxB.Document.Blocks.Add(new Paragraph(new Run(listaPreguntas[2])));

            textBoxC.Document.Blocks.Clear();
            textBoxC.Document.Blocks.Add(new Paragraph(new Run(listaPreguntas[3])));

            btnA.Background = colorNoSeleccionado;
            btnB.Background = colorNoSeleccionado;
            btnC.Background = colorNoSeleccionado;

            switch (respondida)
            {
                case 1:
                    btnA.Background = colorSeleccionado;
                    break;

                case 2:
                    btnB.Background = colorSeleccionado;
                    break;

                case 3:
                    btnC.Background = colorSeleccionado;
                    break;
            }
        }

        System.Windows.Media.Brush colorBotonSeleccionado = (System.Windows.Media.Brush)converter.ConvertFromString("#FF56FFF7");
        System.Windows.Media.Brush colorBotonNoSeleccionado = (System.Windows.Media.Brush)converter.ConvertFromString("#FFDDDDDD");

        private void cambiaDePregunta_Click(object sender, RoutedEventArgs e)
        {
            synth.SpeakAsyncCancelAll();
            Button btnClicked = e.Source as Button;
            TextBlock input = btnClicked.Content as TextBlock;
            if (Convert.ToInt32(input.Text) >= 10)
            {
                txtNumeroPregunta.Text = input.Text + ".";
            }
            else
            {
                txtNumeroPregunta.Text = "0" + input.Text + ".";
            }
            cambiaPregunta(Convert.ToInt32(input.Text));
            btnAnterior.Background = colorBotonNoSeleccionado;
            btnClicked.Background = colorBotonSeleccionado;
            btnAnterior = btnClicked;
        }

        private void btnAnteriorPregunta_Click(object sender, RoutedEventArgs e)
        {
            if(preguntaActual > 1)
            {
                synth.SpeakAsyncCancelAll();
                string btnName = "preg" + Convert.ToString(preguntaActual - 1);
                Button btnClicked = gridSuperior.FindName(btnName) as Button;
                if(preguntaActual -1 >= 10)
                {
                    txtNumeroPregunta.Text = Convert.ToString(preguntaActual - 1) + ".";
                }
                else
                {
                    txtNumeroPregunta.Text = "0" + Convert.ToString(preguntaActual - 1) + ".";
                }
                cambiaPregunta(preguntaActual - 1);
                btnAnterior.Background = colorBotonNoSeleccionado;
                btnClicked.Background = colorBotonSeleccionado;
                btnAnterior = btnClicked;
            }
        }

        private void btnSiguientePregunta_Click(object sender, RoutedEventArgs e)
        {
            if (preguntaActual < cuestionario.cantidadPreguntas)
            {
                synth.SpeakAsyncCancelAll();
                string btnName = "preg" + Convert.ToString(preguntaActual + 1);
                Button btnClicked = gridSuperior.FindName(btnName) as Button;
                if (preguntaActual + 1 >= 10)
                {
                    txtNumeroPregunta.Text = Convert.ToString(preguntaActual + 1) + ".";
                }
                else
                {
                    txtNumeroPregunta.Text = "0" + Convert.ToString(preguntaActual + 1) + ".";
                }
                cambiaPregunta(preguntaActual + 1);
                btnAnterior.Background = colorBotonNoSeleccionado;
                btnClicked.Background = colorBotonSeleccionado;
                btnAnterior = btnClicked;
            }

        }

        // ================
        // FOTO POR PALABRA
        // ================

        private void imagenPalabra_MouseEnter(object sender, MouseEventArgs e)
        {
            imagenPalabra.Visibility = Visibility.Hidden;
            borderImagenPalabra.Visibility = Visibility.Hidden;
        }

        private void textBoxPregunta_MouseMove(object sender, MouseEventArgs e)
        {
            Foto_por_palabra.PalabraAbajoDeMouse(textBoxPregunta, imagenPalabra, borderImagenPalabra, generalGrid);
        }

        private void textBoxPregunta_MouseLeave(object sender, MouseEventArgs e)
        {
            imagenPalabra.Visibility = Visibility.Hidden;
            borderImagenPalabra.Visibility = Visibility.Hidden;
        }

        private void textBoxA_MouseMove(object sender, MouseEventArgs e)
        {
            Foto_por_palabra.PalabraAbajoDeMouse(textBoxA, imagenPalabra, borderImagenPalabra, generalGrid);
        }

        private void textBoxA_MouseLeave(object sender, MouseEventArgs e)
        {
            imagenPalabra.Visibility = Visibility.Hidden;
            borderImagenPalabra.Visibility = Visibility.Hidden;
        }

        private void textBoxB_MouseMove(object sender, MouseEventArgs e)
        {
            Foto_por_palabra.PalabraAbajoDeMouse(textBoxB, imagenPalabra, borderImagenPalabra, generalGrid);
        }

        private void textBoxB_MouseLeave(object sender, MouseEventArgs e)
        {
            imagenPalabra.Visibility = Visibility.Hidden;
            borderImagenPalabra.Visibility = Visibility.Hidden;
        }

        private void textBoxC_MouseMove(object sender, MouseEventArgs e)
        {
            Foto_por_palabra.PalabraAbajoDeMouse(textBoxC, imagenPalabra, borderImagenPalabra, generalGrid);
        }

        private void textBoxC_MouseLeave(object sender, MouseEventArgs e)
        {
            imagenPalabra.Visibility = Visibility.Hidden;
            borderImagenPalabra.Visibility = Visibility.Hidden;
        }

        // ===============
        // TERMINAR EXAMEN
        // ===============

        private void btnTerminarExamen_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pag_Terminar(this));
        }
    }
}
