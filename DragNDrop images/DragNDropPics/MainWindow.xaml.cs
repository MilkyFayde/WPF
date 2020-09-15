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

namespace DragNDropPics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Image selectedImage;
        Point imgPoint;
        bool isDraggin;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (data == null || data.Length == 0) return;
                for (int i = 0; i < data.Length; i++)
            {
                if (IsImage(System.IO.Path.GetExtension(data[i]).ToLower())) AddImage(data[i], e.GetPosition(myGrid), i);
                else if (System.IO.Directory.Exists(data[i])) AddImagesFromFolder(data[i], e.GetPosition(myGrid), i);
            }
        } // Grid_Drop

        private void Grid_DragEnter(object sender, DragEventArgs e)
        {
            if ((e.AllowedEffects & DragDropEffects.Copy) != 0)
            {
                e.Effects = DragDropEffects.Copy;
            }
        } // Grid_DragEnter
        void AddImagesFromFolder(string path, Point point, int step)
        {
            System.IO.DirectoryInfo dinfo = new System.IO.DirectoryInfo(path);
            System.IO.FileInfo[] files = dinfo.GetFiles();
            for(int i =0; i < files.Length; i++)
                if (IsImage(System.IO.Path.GetExtension(files[i].FullName).ToLower())) AddImage(files[i].FullName, point, i + step);
        } // AddImagesFromFolder

        void AddImage(string path, Point point, int step)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(path);
            bmp.EndInit();

            Image img = new Image();
            img.Source = bmp;
            img.Height = bmp.Height;
            img.Width = bmp.Width;
            img.HorizontalAlignment = HorizontalAlignment.Left;
            img.VerticalAlignment = VerticalAlignment.Top;
            img.Margin = new Thickness(point.X + step, point.Y + step, 0, 0);
            img.RenderTransformOrigin = new Point(0.5, 0.5);

            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(new ScaleTransform());
            transformGroup.Children.Add(new RotateTransform());
            img.RenderTransform = transformGroup;

            myGrid.Children.Add(img);
            selectedImage = img;
            img.MouseDown += Img_MouseDown;
            img.MouseUp += Img_MouseUp;
        } // AddImage

        private void Img_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDraggin = false;
        } // Img_MouseUp

        void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            selectedImage = (Image)sender;
            isDraggin = true;
            imgPoint = e.GetPosition(selectedImage);
        } // Img_MouseDown

        bool IsImage(string ext) => ext == ".jpg" || ext == ".png" || ext == ".gif" || ext == ".bmp";

        private void sizeBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (selectedImage == null) return;
            double value = sizeBar.Value;

            TransformGroup transformGroup = selectedImage.RenderTransform as TransformGroup;
            transformGroup.Children[0] = new ScaleTransform(value, value);
        } // sizeBar_ValueChanged

        private void rotateBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (selectedImage == null) return;
            double value = rotateBar.Value;

            TransformGroup transformGroup = selectedImage.RenderTransform as TransformGroup;
            transformGroup.Children[1] = new RotateTransform(value);
        } // sizeBar_ValueChanged

        private void myGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDraggin) return;

            Point point = e.GetPosition(myGrid);
            selectedImage.Margin = new Thickness(point.X-imgPoint.X, point.Y - imgPoint.Y, 0, 0);
        } // myGrid_MouseMove

        private void myGrid_MouseUp(object sender, MouseButtonEventArgs e) => isDraggin = false;

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.Owner = this;
            window.ShowDialog();
        } // About_Click
    }
}
