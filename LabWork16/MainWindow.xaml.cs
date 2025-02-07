using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LabWork16
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectFolderButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FolderPathTextBox.Text;
            if (File.Exists(filePath))
            {
                try
                {
                    var bitmap = new BitmapImage(new Uri(filePath));
                    Image.Source = bitmap;
                    this.Title = System.IO.Path.GetFileName(filePath);
                    FolderPathTextBox.Text = $"{bitmap.PixelWidth} x {bitmap.PixelHeight}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка открытия изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Файл не найден или путь указан неверно.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.bmp;*.jpg;*.jpeg;*.png)|*.bmp;*.jpg;*.jpeg;*.png"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var filePath = openFileDialog.FileName;
                    var bitmap = new BitmapImage(new Uri(filePath));
                    Image.Source = bitmap;
                    ImageScaleTransform.ScaleX = 1;
                    ImageScaleTransform.ScaleY = 1;
                    this.Title = System.IO.Path.GetFileName(filePath);
                    var fileInfo = new FileInfo(filePath);
                    long fileSizeInKb = fileInfo.Length / 1024;
                    FolderPathTextBox.Text = $"{bitmap.PixelWidth} x {bitmap.PixelHeight}, {fileSizeInKb} KB";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка открытия изображения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }




        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void ScaleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ImageScaleTransform != null)
            {
                double scale = e.NewValue; 
                ImageScaleTransform.ScaleX = scale;
                ImageScaleTransform.ScaleY = scale;
                ScalePercentageTextBlock.Text = $"{(int)(scale * 100)}%";
                //UpdateScrollViewerVisibility();
            }
        }
        //Тут реализовано появление ScrollBar когда изображение слишком велико или сильно приближено
        //Иногда багует и изображение подгружается не полностью

        //private void UpdateScrollViewerVisibility()
        //{
        //    if (Image.Source is BitmapImage bitmap)
        //    {
        //        double scaledWidth = bitmap.PixelWidth * ImageScaleTransform.ScaleX;
        //        double scaledHeight = bitmap.PixelHeight * ImageScaleTransform.ScaleY;
        //        double viewerWidth = ImageScrollViewer.ActualWidth;
        //        double viewerHeight = ImageScrollViewer.ActualHeight;
        //        ImageScrollViewer.HorizontalScrollBarVisibility = scaledWidth > viewerWidth
        //            ? ScrollBarVisibility.Auto
        //            : ScrollBarVisibility.Disabled;
        //        ImageScrollViewer.VerticalScrollBarVisibility = scaledHeight > viewerHeight
        //            ? ScrollBarVisibility.Auto
        //            : ScrollBarVisibility.Disabled;
        //    }
        //}

    }
}