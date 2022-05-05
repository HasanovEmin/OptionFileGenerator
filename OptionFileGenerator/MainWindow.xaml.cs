using Microsoft.Win32;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace OptionFileGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public double CanvasWidth { get; set; }
        public double CanvasHeight { get; set; }
        public double CanvasChangedWidth { get; set; }
        public double CanvasChangedHeight { get; set; }
        public object OldSelectedVal { get; set; }


        ImageBrush brush;
        AddItem addCntrl;
        public MainWindow()
        {
            InitializeComponent();
            
            brush = new ImageBrush();

            listView.ItemsSource = ItemHolder.itemCollection;
            CmbSizes.ItemsSource = Worker.ListSizes;
            listView.SelectionMode = SelectionMode.Single;

        }

        
        private void CanvasMain_Loaded(object sender, RoutedEventArgs e)
        {
            CanvasWidth = CanvasMain.ActualWidth;
            CanvasHeight = CanvasMain.ActualHeight;

            screenLabel.Content = $"CW={CanvasWidth} x CH={CanvasHeight}";
        }

        private void CmbSizes_Loaded(object sender, RoutedEventArgs e)
        {
            CmbSizes.Text = Worker.ListSizes[2].ToString();
            Worker.SetBacksheetSize(CmbSizes.SelectedItem.ToString());

            CanvasChangedWidth = CanvasMain.ActualWidth;
            CanvasChangedHeight = CanvasMain.ActualHeight;

            Worker.SetBackendSheetCoefficients(CanvasChangedHeight, CanvasChangedWidth);
            OldSelectedVal = CmbSizes.SelectedValue;

        }

        private void CanvasMain_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(CanvasMain);
            Worker.CalculateCoordinate(pos);

            addCntrl = new AddItem();
            addCntrl.ShowDialog();
            if (CanvasMain.Children.Count == listView.Items.Count)
            {
                return;
            }
            DrawCanvas(Worker.XCoord, Worker.YCoord, ItemHolder.itemCollection.Count.ToString(), Colors.Blue);
        }

        private void CanvasMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CanvasChangedWidth = CanvasMain.ActualWidth;
            CanvasChangedHeight = CanvasMain.ActualHeight;
            Worker.SetBackendSheetCoefficients(CanvasChangedHeight, CanvasChangedWidth);
            ReDrawCanvas();
            listView.Height = CanvasChangedHeight * 0.8;
            
        }


        private void CmbSizes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listView.Items.Count != 0 && OldSelectedVal != CmbSizes.SelectedValue )
            {
                var dialog = MessageBox.Show("If you continue, you will lose all progress!", "Attention", MessageBoxButton.YesNo);
                if (dialog == MessageBoxResult.Yes)
                {
                    Worker.DeleteAllProgress();
                    ReDrawCanvas();
                    OldSelectedVal = CmbSizes.SelectedValue;
                }
                else
                {
                    CmbSizes.SelectedValue = OldSelectedVal;

                }
            }
            Worker.SetBacksheetSize(CmbSizes.SelectedItem.ToString());
            screenLabel.Content = $"CW={Worker.BackingsheetWidth} x CH={Worker.BackingsheetHeight}";
            Worker.SetBackendSheetCoefficients(CanvasChangedHeight, CanvasChangedWidth);
        }

        

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Item item = listView.SelectedItem as Item;
            if (item == null)
                return;


            addCntrl = new AddItem();
            addCntrl.FillEditableProperties(item.Id, item.Text, item.Attribute, item.XCoord, item.YCoord, item.Charh, item.Angle);
            addCntrl.ShowDialog();
            Worker.SetBackendSheetCoefficients(CanvasChangedHeight, CanvasChangedWidth);
            ReDrawCanvas();

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Item item = listView.SelectedItem as Item;
            if (item == null)
                return;

            Worker.DeleteItem(item.Id - 1);
            ReDrawCanvas();
        }

        // Draw method
        private void DrawCanvas(double xCoord, double yCoord, string text, Color color)
        {
            
            TextBlock textBlock = new TextBlock();
            textBlock.Tag = text;
            textBlock.Text = text;
            
            double convertedYCoord = Worker.BackingsheetHeight - yCoord;
            textBlock.Foreground = new SolidColorBrush(color);
            Canvas.SetLeft(textBlock, xCoord * Worker.CoefficientWidth - 2);
            Canvas.SetTop(textBlock, convertedYCoord * Worker.CoefficientHeight - 12);
            CanvasMain.Children.Add(textBlock);

        }


        // ReDraw method
        private void ReDrawCanvas()
        {
            CanvasMain.Children.Clear();
            foreach (var item in ItemHolder.itemCollection)
            {
                int iter = item.Id;
                DrawCanvas(item.XCoord, item.YCoord, iter.ToString(), Colors.Blue);
            }

        }


        private void CanvasMain_QueryCursor(object sender, QueryCursorEventArgs e)
        {
            currentCoordsLabel.Content = e.GetPosition(this).ToString();
            double x = Math.Round(e.GetPosition(this).X / Worker.CoefficientWidth, 2);
            double y = Worker.BackingsheetHeight - Math.Round(e.GetPosition(this).Y / Worker.CoefficientHeight, 2);
            currentCHCoordsLabel.Content = $"{Worker.ConverToString(x)} x {Worker.ConverToString(y)}";
        }


        // Add backingsheet canvas
        private void btnAddBack_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Images files (*.png; *jpeg)|*.png;*.jpeg|All files (*.*)|*.*";

            bool? result = openFileDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                brush.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName));
                CanvasMain.Background = brush;
                Worker.FileName = openFileDialog.FileName;
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (CanvasMain.Background == null)
            {
                MessageBox.Show("Please set backingsheet first", "Attention", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (listView.Items.Count != 0 )
            {
                var dialog = MessageBox.Show("If you continue, you will lose all progress!", "Attention", MessageBoxButton.YesNo);
                if (dialog == MessageBoxResult.No)
                {
                    return;
                    
                }
                ItemHolder.itemCollection.Clear();
                ReDrawCanvas();
            }
            ImportFile importFile = new ImportFile();
            importFile.Import();

            
            ReDrawCanvas();
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (listView.Items.Count == 0)
            {
                MessageBox.Show("There is nothing to export");
                return;
            }
            var collectionFromList = listView.Items;
            ExportFile exportFile = new ExportFile();
            exportFile.Export(collectionFromList);
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            if (listView.Items.Count == 0)
            {
                return;
            }
            var dialog = MessageBox.Show("Are you sure?", "Delete all progress", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dialog == MessageBoxResult.Yes)
            {
                Worker.DeleteAllProgress();
                ReDrawCanvas();
            }
            
        }
    }
}
