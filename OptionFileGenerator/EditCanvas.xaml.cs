using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OptionFileGenerator
{
    /// <summary>
    /// Interaction logic for EditCanvas.xaml
    /// </summary>
    public partial class EditCanvas : Window
    {
        public EditCanvas()
        {
            InitializeComponent();
           
            this.Width = 436;
            this.Height = 633;

            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri(Worker.FileName));
            CanvasEdit.Background = brush;
            
        }

        private void CanvasEdit_Loaded(object sender, RoutedEventArgs e)
        {
            CanvasEdit.Width = Worker.CanvasChangedWidth;
            CanvasEdit.Height = Worker.CanvasChangedHeight;
        }

        private void CanvasEdit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(CanvasEdit);
            Worker.CalculateCoordinate(pos);
            this.Close();
            //MessageBox.Show($"{Worker.XCoord};{Worker.YCoord}");

        }


        private void CanvasEdit_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Worker.SetBackendSheetCoefficients(CanvasEdit.Height, CanvasEdit.Width);
        }

        private void window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
            CanvasEdit.Width = stackPan.ActualWidth;
            CanvasEdit.Height = stackPan.ActualHeight;
            
        }
    }
}
