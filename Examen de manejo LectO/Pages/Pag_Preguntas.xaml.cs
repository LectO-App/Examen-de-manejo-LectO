using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Speech.Synthesis;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Examen_de_manejo_LectO.Pages
{
    /// <summary>
    /// Lógica de interacción para Pag_Preguntas.xaml
    /// </summary>
    public partial class Pag_Preguntas : Page
    {
        Cuestionario cuestionario;
        SpeechSynthesizer synth;

        public Pag_Preguntas()
        {
            InitializeComponent();
            cuestionario = new Cuestionario();
            synth = new SpeechSynthesizer();
        }

        private void ttsPregunta_Click(object sender, RoutedEventArgs e)
        {
            synth.SpeakAsyncCancelAll();
            synth.SpeakAsync(textBoxPregunta.Text);
        }

        private void ttsA_Click(object sender, RoutedEventArgs e)
        {
            synth.SpeakAsyncCancelAll();
            synth.SpeakAsync("Opción A \n" + textBoxA.Text);
        }

        private void ttsB_Click(object sender, RoutedEventArgs e)
        {
            synth.SpeakAsyncCancelAll();
            synth.SpeakAsync("Opción B \n" + textBoxB.Text);
        }

        private void ttsC_Click(object sender, RoutedEventArgs e)
        {
            synth.SpeakAsyncCancelAll();
            synth.SpeakAsync("Opción C \n" + textBoxC.Text);
        }

        private void ttsGeneral_Click(object sender, RoutedEventArgs e)
        {
            synth.SpeakAsyncCancelAll();
            synth.SpeakAsync(textBoxPregunta.Text + "\nOpción A \n" + textBoxA.Text + "\nOpción B \n" + textBoxB.Text + "\nOpción C \n" + textBoxC.Text);
        }
    }
}
