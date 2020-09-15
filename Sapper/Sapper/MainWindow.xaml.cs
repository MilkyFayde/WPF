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

namespace Sapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int size; // board height and width 
        int bombs; // counter of squares with bombs
        int safeSquares; // counter of squares without bombs
        bool inProgress; // is game active
        Square[,] board;

        SolidColorBrush safeBrush = new SolidColorBrush(Color.FromArgb(177, 148, 225, 255));
        SolidColorBrush whiteBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        ImageBrush bombBrush = new ImageBrush();
        ImageBrush flagBrush = new ImageBrush();

        public MainWindow()
        {
            InitializeComponent();

            Image image = new Image();
            image.Source = new BitmapImage(new Uri("flag.png", UriKind.Relative));
            flagBrush.ImageSource = image.Source;

            image = new Image();
            image.Source = new BitmapImage(new Uri("bomb.png", UriKind.Relative));
            bombBrush.ImageSource = image.Source;

            Start(); // Game start
        }

        void Start(int size = 5)
        {
            inProgress = true;
            this.size = size;

            bombs = (int)(size * size * 0.25);
            bombsLabel.Content = bombs;
            safeSquares = size * size - bombs;

            CreateGridBoard(); // create grid size * size
            CreateAndFillBoard(); // fill board 2d array 
            GenerateBombs(); // generate and add bombs to board 2d array 
        } // Start

        void CreateAndFillBoard()
        {
            Rectangle rect;

            board = new Square[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    board[i, j] = new Square();
                    rect = new Rectangle();
                    rect.Fill = whiteBrush;
                    rect.Margin = new Thickness(0, 0, 0, 0);
                    Grid.SetColumn(rect, i);
                    Grid.SetRow(rect, j);
                    myGrid.Children.Add(rect);
                }
        } // CreateAndFillBoard
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
        void GenerateBombs()
        {
            Random rand = new Random();
            int maxIndex = size * size;
            List<int> bombsIndex = new List<int>();

            for (int i = 0; i < bombs; i++)
            {
                int tempIndex = rand.Next(0, maxIndex);
                while (bombsIndex.Contains(tempIndex)) tempIndex = rand.Next(0, maxIndex);
                bombsIndex.Add(tempIndex);
            }

            AddBombsToBoard(bombsIndex); // Add bombs to board 2d array
        } // GenerateBombs

        void AddBombsToBoard(List<int> bombsIndex)
        {
            int i, j;
            foreach (var bomb in bombsIndex)
            {
                i = bomb / size;
                j = bomb % size;
                board[i, j].IsBomb = true;
                IncreaseBombsCounter(i, j); // increase bomb count of near by squares
            }
        } // AddBombsToBoard

        void IncreaseBombsCounter(int x, int y)
        {
            for (int i = x - 1; i <= x + 1; i++)
                for (int j = y - 1; j <= y + 1; j++)
                    if (i >= 0 && i < size && j >= 0 && j < size && (x != i || y != j))
                        board[i, j].NearBombs++;
        } // IncreaseBombsCounter

        private void myGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!inProgress) return; // if game is not in progress
            var element = e.Source;

            if (element is TextBlock) return;

            if (e.ChangedButton == MouseButton.Right) // if right click
                RightClick((Rectangle)element);
            else // if left click
            {
                int column = Grid.GetColumn((UIElement)element);
                int row = Grid.GetRow((UIElement)element);
                LeftClick(column, row, (Rectangle)element);
            }

            if (safeSquares == 0) VictoryScreen(); // show Victory screen
        } // myGrid_MouseDown

        void ShowAllBombs()
        {
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    if (board[i, j].IsBomb)
                         ShowBomb(i, j);
        } // ShowAllBombs

        void ShowBomb(int column, int row)
        {
            Rectangle rect = myGrid.Children.Cast<Rectangle>().First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
            rect.Fill = bombBrush;
        } // ShowBomb

        void ShowCountOfNearByBombs(int column, int row)
        {
            int cnt = board[column, row].NearBombs;
            TextBlock bombsNearBy = new TextBlock();
            bombsNearBy.Text = cnt.ToString();

            Grid.SetColumn(bombsNearBy, column);
            Grid.SetRow(bombsNearBy, row);

            bombsNearBy.FontSize = 20;
            bombsNearBy.VerticalAlignment = VerticalAlignment.Center;
            bombsNearBy.HorizontalAlignment = HorizontalAlignment.Center;

            if (cnt == 0) bombsNearBy.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 128, 255));
            else if(cnt < 3) bombsNearBy.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 255));
            else bombsNearBy.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));

            myGrid.Children.Add(bombsNearBy);
        } // ShowCountOfNearByBombs

        void LeftClick(int column, int row, Rectangle rect)
        {
            if (rect.Fill == flagBrush || rect.Fill == safeBrush) return;

            if (board[column, row].IsBomb)
            {
                ShowAllBombs(); // show all bombs on board
                DefeatScreen(); // show Defeat screen
                return;
            }

            rect.Fill = safeBrush;
            ShowCountOfNearByBombs(column, row); // show count of near by bombs
            safeSquares--;
        } // LeftClick

        void RightClick(Rectangle rect)
        {
            if (rect.Fill == safeBrush) return;
            if (rect.Fill == flagBrush)
            {
                rect.Fill = whiteBrush;
                bombs++;
            }
            else if (bombs > 0)
            {
                rect.Fill = flagBrush;
                bombs--;
            }
            bombsLabel.Content = bombs;
        } // RightClick

        void VictoryScreen()
        {
            VictoryWindow window = new VictoryWindow();
            window.Owner = this;
            window.ShowDialog();
            inProgress = false;
        } // VictoryScreen

        void DefeatScreen()
        {
            DefeatWindow window = new DefeatWindow();
            window.Owner = this;
            window.ShowDialog();
            inProgress = false;
        } // DefeatScreen

        private void NewGame_Click(object sender, RoutedEventArgs e) => Start(size);

        private void ChangeSize_Click(object sender, RoutedEventArgs e)
        {
            ChangeBoardSizeWindow window = new ChangeBoardSizeWindow();
            window.Owner = this;
            window.action += Start;
            window.ShowDialog();
        } // ChangeSize_Click

        private void ShowAllBombs_Click(object sender, RoutedEventArgs e) => ShowAllBombs();
    } // class MainWindow
}
