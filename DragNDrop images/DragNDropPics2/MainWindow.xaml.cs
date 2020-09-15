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

namespace DragNDropPics2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int size; // board = size * size
        List<Image> images = new List<Image>(); // list of board images
        public MainWindow()
        {
            InitializeComponent();

            Start();
        }

        private void Start(int size = 4)
        {
            this.size = size;
            images.Clear();
            CreateGridBoard(); // create grid row and columns

            Image img;

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    img = new Image();

                    Grid.SetColumn(img, i);
                    Grid.SetRow(img, j);
                    myGrid.Children.Add(img);
                    images.Add(img);
                }
        } // Start

        void CreateGridBoard()
        {
            myGrid.Children.Clear();
            myGrid.ColumnDefinitions.Clear();
            myGrid.RowDefinitions.Clear();
            myGrid.ShowGridLines = true;

            ColumnDefinition tempColumn;
            RowDefinition tempRow;

            for (int i = 0; i < size; i++)
            {
                tempColumn = new ColumnDefinition();
                tempRow = new RowDefinition();

                myGrid.ColumnDefinitions.Add(tempColumn);
                myGrid.RowDefinitions.Add(tempRow);
            } // for i
        } // CreateBoard

        private void myGrid_DragEnter(object sender, DragEventArgs e)
        {
            if ((e.AllowedEffects & DragDropEffects.Copy) != 0)
                e.Effects = DragDropEffects.Copy;
        } // myGrid_DragEnter

        private void myGrid_Drop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (data == null || data.Length == 0) return;
            if (IsImage(System.IO.Path.GetExtension(data[0]).ToLower())) ReplaceImage(data[0], e.GetPosition(myGrid)); // replace old image with new
        } // myGrid_Drop

        void ReplaceImage(string path, Point point)
        {
            GetIndexes(out int column, out int row, point); // get column and row by mouse position
            Image oldImg = GetImageByIndex(column, row); // get image by column and row

            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri(path);
            bmp.EndInit();

            oldImg.Source = bmp;
        } // AddImage

        bool IsImage(string ext) => ext == ".jpg" || ext == ".png" || ext == ".gif" || ext == ".bmp";

        Image GetImageByIndex(int column, int row) => myGrid.Children.Cast<Image>().First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);

        void GetIndexes(out int column, out int row, Point point)
        {
            column = 0;
            row = 0;

            double width = 0;
            for (int i = 0; i < myGrid.ColumnDefinitions.Count; i++)
            {
                width += myGrid.ColumnDefinitions[i].ActualWidth;
                if (width > point.X)
                {
                    column = i;
                    break;
                }
            }

            double height = 0;
            for (int i = 0; i < myGrid.RowDefinitions.Count; i++)
            {
                height += myGrid.RowDefinitions[i].ActualHeight;
                if (height > point.Y)
                {
                    row = i;
                    break;
                }
            }
        } // GetIndexes

        private void ChangeSize_Click(object sender, RoutedEventArgs e)
        {
            ChangeSizeWindow window = new ChangeSizeWindow(size);
            window.action = Start;
            window.Owner = this;
            window.ShowDialog();
        } // ChangeSize_Click

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        } // Exit_Click

        private void ShuffleBoard_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage[,] tempBitmaps = new BitmapImage[size, size];
            Random rand = new Random();
            int index;

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    index = rand.Next(images.Count);
                    tempBitmaps[i, j] = new BitmapImage();
                    tempBitmaps[i, j] = images[index].Source as BitmapImage;
                    images.RemoveAt(index);
                }

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    Image oldImg = GetImageByIndex(i, j);
                    oldImg.Source = tempBitmaps[i, j];
                    images.Add(oldImg);
                }
        } // ShuffleBoard_Click

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.Owner = this;
            window.ShowDialog();
        } // About_Click

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        } // Window_KeyDown
    } // class MainWindow
}
