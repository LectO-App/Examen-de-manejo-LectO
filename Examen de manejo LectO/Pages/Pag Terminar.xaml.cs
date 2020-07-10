using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Lógica de interacción para Pag_Terminar.xaml
    /// </summary>
    public partial class Pag_Terminar : Page
    {
        Pag_Preguntas pantallaPreguntas;
        Cuestionario cuestionario;

        public Pag_Terminar(Pag_Preguntas preguntas)
        {
            InitializeComponent();
            pantallaPreguntas = preguntas;
            cuestionario = pantallaPreguntas.cuestionario;

            txtNombreApellido.Text = pantallaPreguntas.nombreApellido;
            txtDocumento.Text = pantallaPreguntas.documento;
        }

        private void btnTerminar_Click(object sender, RoutedEventArgs e)
        {
            if(txtDocumento.Text.Length > 0 && txtNombreApellido.Text.Length > 0)
            {
                cuestionario.terminar(txtDocumento.Text, txtNombreApellido.Text);
                NavigationService.Navigate(new Pag_Home());
            }
            else
            {
                MessageBox.Show("Por favor, complete los campos antes de comenzar");
            }
        }

        private void btnVolverAtras_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(pantallaPreguntas);
        }
    }
}
