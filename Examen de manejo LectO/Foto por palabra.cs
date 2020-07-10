using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.IO;
using System;

namespace Pruebas_WPF_2
{
    class Foto_por_palabra
    {
        static string palabraAnterior = "";
        static string palabraActual = "";
        static ImageSource sourceBlanco = (ImageSource)new ImageSourceConverter().ConvertFromString(@"./Imagenes/blanco.jpg");
        static TextPointer start;

        public static string PalabraAbajoDeUnPunto(TextPointer start, RichTextBox rtb)
        {
            TextPointer end = start;

            string stringBeforeCaret = new TextRange(rtb.Document.ContentStart, start).Text;
            string stringAfterCaret = new TextRange(start, rtb.Document.ContentEnd).Text;

            int countToMoveLeft = 0;
            int countToMoveRight = 0;

            for (int i = stringBeforeCaret.Length - 1; i >= 0; --i)
            {
                if (char.IsLetter(stringBeforeCaret[i]))
                    ++countToMoveLeft;
                else
                {
                    break;
                }
            }

            for (int i = 0; i < stringAfterCaret.Length; ++i)
            {
                if (char.IsLetter(stringAfterCaret[i]))
                    ++countToMoveRight;
                else
                {
                    break;
                }
            }

            string palabraIzquierda = stringBeforeCaret.Substring(stringBeforeCaret.Length - countToMoveLeft, countToMoveLeft);
            string palabraDerecha = stringAfterCaret.Substring(0, countToMoveRight);
            string text = palabraIzquierda + palabraDerecha;

            return text;
        }

        public static void CambiarFuenteImagen(System.Windows.Controls.Image imagenPalabra, Border borderImagenPalabra)
        {
            if (palabraActual == "")
            {
                imagenPalabra.Visibility = Visibility.Hidden;
                borderImagenPalabra.Visibility = Visibility.Hidden;
                imagenPalabra.Source = sourceBlanco;
            }
            else
            {
                if (palabraActual != palabraAnterior)
                {
                    string pathPNG = @"./Imagenes/" + palabraActual + ".png";
                    string pathJPG = @"./Imagenes/" + palabraActual + ".jpg";

                    if (File.Exists(pathPNG))
                    {
                        ImageSource sourceNow = (ImageSource)new ImageSourceConverter().ConvertFromString(pathPNG);
                        imagenPalabra.Source = sourceNow;
                        imagenPalabra.Visibility = Visibility.Visible;
                        borderImagenPalabra.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        if (File.Exists(pathJPG))
                        {
                            ImageSource sourceNow = (ImageSource)new ImageSourceConverter().ConvertFromString(pathJPG);
                            imagenPalabra.Source = sourceNow;
                            imagenPalabra.Visibility = Visibility.Visible;
                            borderImagenPalabra.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            imagenPalabra.Visibility = Visibility.Hidden;
                            borderImagenPalabra.Visibility = Visibility.Hidden;
                            imagenPalabra.Source = sourceBlanco;
                        }
                    }
                }
            }
        }

        public static void CambiarPosicionImagen(RichTextBox rtbTexto, System.Windows.Controls.Image imagenPalabra, Border borderImagenPalabra, Grid generalGrid)
        {
            Point posicion = Mouse.GetPosition(generalGrid);
            Point posicionTextBox = Mouse.GetPosition(rtbTexto);
            start = rtbTexto.GetPositionFromPoint(posicionTextBox, false);

            if (start != null)
            {
                Point relativePoint = rtbTexto.TransformToAncestor(generalGrid).Transform(new Point(0, 0));

                Rect rect = start.GetCharacterRect(LogicalDirection.Forward);
                Point posicionGeneral = rect.BottomRight;

                Thickness margin = imagenPalabra.Margin;
                margin.Left = posicion.X - 75;
                margin.Top = posicionGeneral.Y + 150 + relativePoint.Y;
                borderImagenPalabra.Margin = margin;
            }
            else
            {
                borderImagenPalabra.Visibility = Visibility.Hidden;
                imagenPalabra.Visibility = Visibility.Hidden;
                imagenPalabra.Source = sourceBlanco;
            }
        }

        public static void PalabraAbajoDeMouse(RichTextBox rtbTexto, System.Windows.Controls.Image imagenPalabra, Border borderImagenPalabra, Grid generalGrid)
        {
            CambiarPosicionImagen(rtbTexto, imagenPalabra, borderImagenPalabra, generalGrid);

            if (start != null)
            {
                palabraActual = PalabraAbajoDeUnPunto(start, rtbTexto);
            }
            else
            {
                palabraActual = "";
            }

            CambiarFuenteImagen(imagenPalabra, borderImagenPalabra);
        }
    }
}
