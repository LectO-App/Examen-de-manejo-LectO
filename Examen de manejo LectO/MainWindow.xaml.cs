using Examen_de_manejo_LectO.Pages;
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

namespace Examen_de_manejo_LectO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Pag_Home Home = new Pag_Home();

        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            MainFrame.Content = Home;
        }
    }
}
