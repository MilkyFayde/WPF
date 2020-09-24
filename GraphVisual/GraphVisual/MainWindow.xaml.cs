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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphVisual
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Graph graph;
        FrameworkElement selectedElement; // currently selected item
        bool isMoving; // if currently selected item is dragged
        Point currentPoint; // current mouse pos
        Dictionary<Link, Line> links = new Dictionary<Link, Line>();
        double figureWidth = 150;
        double figureHeight = 50;
        Line tempLine;

        public MainWindow()
        {
            InitializeComponent();


            //graph = new Graph("1234", new Point((this.Width - figureWidth) / 2, 50), figureWidth, figureHeight);
            //graph.AddVertex("a", 5, new Point(0, 22));
            //graph.AddVertex("b", 5, new Point(0, 92));
            //graph.AddVertex("c", 5, new Point(170, 22));
            //graph.AddVertex("d", 5, new Point(170, 92));
            //graph.AddLink("link1", 5, "a", "b");
            //graph.AddLink("link1", 5, "a", "c");
            //DrawAll();
        }

        void DrawGraph(Point position) => DrawFigure(new Rectangle(), position, graph.Title, null);
        void DrawVertex(Point position, string title, Vertex vertex) => DrawFigure(new Ellipse(), position, title, vertex);
        void DrawFigure(Shape figure, Point position, string title, Vertex vertex)
        {
            figure.HorizontalAlignment = HorizontalAlignment.Left;
            figure.VerticalAlignment = VerticalAlignment.Center;
            figure.Width = figureWidth;
            figure.Height = figureHeight;

            TextBlock text = new TextBlock();
            text.Text = title;
            text.DataContext = vertex;
            text.TextWrapping = TextWrapping.Wrap;
            text.FontSize = 14;
            text.VerticalAlignment = VerticalAlignment.Center;
            text.HorizontalAlignment = HorizontalAlignment.Center;

            Grid grid = new Grid();
            grid.Children.Add(figure);
            grid.Children.Add(text);
            grid.MouseDown += Grid_MouseDown;
            grid.MouseUp += Grid_MouseUp;

            Canvas.SetLeft(grid, position.X);
            Canvas.SetTop(grid, position.Y);
            Canvas.SetZIndex(grid, 1);
            grid.Height = figureHeight;
            grid.Width = figureWidth;

            if (vertex != null)
            {
                figure.ToolTip = vertex.toolTip.toolTip;
                text.ToolTip = vertex.toolTip.toolTip;
            }
            else
            {
                figure.ToolTip = graph.toolTip.toolTip;
                text.ToolTip = graph.toolTip.toolTip;
            }

            myCanvas.Children.Add(grid);
        } // DrawFigure

        void DrawLink(Link link)
        {
            Line line = new Line();

            line.X1 = link.PointFrom.X;
            line.X2 = link.PointTo.X;
            line.Y1 = link.PointFrom.Y;
            line.Y2 = link.PointTo.Y;
            line.HorizontalAlignment = HorizontalAlignment.Left;
            line.VerticalAlignment = VerticalAlignment.Center;
            line.StrokeThickness = 5;
            line.MouseDown += Line_MouseDown;
            Canvas.SetZIndex(line, 0);

            line.ToolTip = link.toolTip.toolTip;


            links.Add(link, line);
            myCanvas.Children.Add(line);
        } // DrawLine
        void DrawAll()
        {
            DrawGraph(graph.GraphPoint);

            foreach (var vertex in graph.Vertices)
            {
                DrawVertex(vertex.VertexPoint, $"{vertex.Title}\n{vertex.Weight}", vertex);
                foreach (var link in vertex.Links)
                {
                    if (links.ContainsKey(link)) continue;
                    else DrawLink(link);
                }
            }
        } // DrawAll
        DropShadowEffect CreateShadow()
        {
            return new DropShadowEffect
            {
                Color = new Color { A = 0, R = 0, G = 0, B = 0 },
                Direction = 300,
                BlurRadius = 50,
                RenderingBias = RenderingBias.Quality,
                ShadowDepth = 10,
                Opacity = 0.8
            };
        } // CreateShadow

        private void Line_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (selectedElement != null) selectedElement.ClearValue(EffectProperty);
            selectedElement = (FrameworkElement)sender;
            selectedElement.Effect = CreateShadow();
        } // Line_MouseDown
        private void Exit_Click(object sender, RoutedEventArgs e) => this.Close();
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (selectedElement != null) selectedElement.ClearValue(EffectProperty);
            selectedElement = (FrameworkElement)sender;
            Canvas.SetZIndex(selectedElement, 10);
            selectedElement.Effect = CreateShadow();

            if (e.ChangedButton == MouseButton.Right)
            {
                tempLine = new Line();
                Point coords = e.GetPosition(myCanvas);
                tempLine.X1 = coords.X;
                tempLine.Y1 = coords.Y;
                tempLine.X2 = coords.X;
                tempLine.Y2 = coords.Y;

                tempLine.HorizontalAlignment = HorizontalAlignment.Left;
                tempLine.VerticalAlignment = VerticalAlignment.Center;
                tempLine.StrokeThickness = 5;
                tempLine.MouseDown += Line_MouseDown;
                Canvas.SetZIndex(tempLine, 0);

                //links.Add(link, line);
                myCanvas.Children.Add(tempLine);
                return;
            }


            isMoving = true;
            currentPoint = e.GetPosition(selectedElement);

            e.Handled = true;
        } // Graph_MouseDown

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (tempLine != null)
            {
                Grid grid1 = selectedElement as Grid;
                Grid grid2 = sender as Grid;
  
                if (grid2.Children[0] is Ellipse && grid1 != grid2)
                {
                    TextBlock textBox1 = grid1.Children[1] as TextBlock;
                    Vertex vertex1 = (Vertex)textBox1.DataContext;
                    TextBlock textBox2 = grid2.Children[1] as TextBlock;
                    Vertex vertex2 = (Vertex)textBox2.DataContext;

                    CreateLinkWindow window = new CreateLinkWindow(CheckAndAddLink, graph, vertex1.Id, vertex2.Id);
                    window.Owner = this;
                    window.ShowDialog();
                }
                myCanvas.Children.Remove(tempLine);
                tempLine = null;
            }
        } // Grid_MouseUp

        private void myCanavas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (tempLine != null)
            {
                myCanvas.Children.Remove(tempLine);
                tempLine = null;
            }
            if (isMoving)
            {
                //selectedElement.ClearValue(EffectProperty);
                Canvas.SetZIndex(selectedElement, 1);
                isMoving = false;
            } // if
        } // myCanavas_MouseUp
        private void myCanavas_MouseMove(object sender, MouseEventArgs e)
        {
            Point coords = e.GetPosition(myCanvas);
            if (tempLine != null)
            {
                tempLine.X2 = coords.X;
                tempLine.Y2 = coords.Y;
                return;
            }

            if (isMoving)
            {
                coords.X -= currentPoint.X;
                coords.Y -= currentPoint.Y;
                Canvas.SetLeft(selectedElement, coords.X);
                Canvas.SetTop(selectedElement, coords.Y);
                Grid grid = selectedElement as Grid;
                TextBlock textBox = grid.Children[1] as TextBlock;

                Point temp = new Point();
                temp.X = Canvas.GetLeft(grid);
                temp.Y = Canvas.GetTop(grid);

                if (grid.Children[0] is Rectangle) graph.GraphPoint = temp;
                if (grid.Children[0] is Ellipse)
                {
                    Vertex vertex = (Vertex)textBox.DataContext;
                    vertex.ChangePoint(temp);
                    foreach (var link in vertex.Links)
                        MoveLine(link);
                } // if Ellipse
            } // if isMoving
        } // myCanavas_MouseMove

        void MoveLine(Link link)
        {
            Line line = links[link];
            if (line.X1 != link.PointFrom.X) line.X1 = link.PointFrom.X;
            if (line.Y1 != link.PointFrom.Y) line.Y1 = link.PointFrom.Y;
            if (line.X2 != link.PointTo.X) line.X2 = link.PointTo.X;
            if (line.Y2 != link.PointTo.Y) line.Y2 = link.PointTo.Y;
        } // MoveLine

        private void CreateGraph_Click(object sender, RoutedEventArgs e)
        {
            CreateGraphWindow window = new CreateGraphWindow(CreateNewGraph);
            window.Owner = this;
            window.ShowDialog();
        } // CreateGraph_Click

        void CreateNewGraph(string name)
        {
            myCanvas.Children.Clear();
            links.Clear();

            Point point = new Point((this.Width - figureWidth) / 2, 50);
            graph = new Graph(name, point, figureWidth, figureHeight);
            DrawGraph(point);
        } // CreateNewGraph

        private void AddVertex_Click(object sender, RoutedEventArgs e)
        {
            if (graph == null)
            {
                MessageBox.Show($"Create new \"Graph\" first");
                return;
            }
            CreateVertexWindow window = new CreateVertexWindow(CheckAndAddVertex);
            window.Owner = this;
            window.ShowDialog();
        } // AddVertex_Click

        private void AddLink_Click(object sender, RoutedEventArgs e)
        {
            if (graph == null)
            {
                MessageBox.Show($"Create new \"Graph\" first");
                return;
            }

            if (graph.GetVerticesCount() == 0)
            {
                MessageBox.Show($"Add more \"Vertices\" to create \"Links\"");
                return;
            }
            CreateLinkWindow window = new CreateLinkWindow(CheckAndAddLink, graph);
            window.Owner = this;
            window.ShowDialog();
        } // AddLink_Click

        bool CheckAndAddVertex(string name, int weight)
        {
            if (graph.Exist(name)) return false;
            Point point = new Point();
            if (graph.GetVerticesCount() == 0)
            {
                point.X = graph.GraphPoint.X;// + graph.FigureWidth
                point.Y = graph.GraphPoint.Y + graph.FigureHeight + 50;
                graph.AddVertex(name, weight, point);
            }
            else
            {
                point.X = graph.Vertices.Last().VertexPoint.X;
                point.Y = graph.Vertices.Last().VertexPoint.Y + graph.FigureHeight + 25;
                graph.AddVertex(name, weight, point);
            }
            string text = $"{graph.Vertices.Last().Title}\n {graph.Vertices.Last().Weight}";
            DrawVertex(point, text, graph.Vertices.Last());
            return true;
        } // CheckAndAddVertex

        bool CheckAndAddLink(string name, int weight, int id1, int id2)
        {
            Vertex vertex1 = graph.FindVertex(id1);
            Vertex vertex2 = graph.FindVertex(id2);
            if (vertex1.IsLinkExists(vertex2)) return false;
            Link link = graph.AddLink(name, weight, vertex1, vertex2);

            DrawLink(link);
            return true;
        } // CheckAndAddVertex

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedElement == null) return;
            object obj = selectedElement;

            if (obj is Line)
            {
                DeleteLink((Line)obj);
            }

            else if (obj is Grid)
            {
                Grid grid = obj as Grid;
                if (grid.Children[0] is Rectangle) DeleteGraph(grid);
                else if (grid.Children[0] is Ellipse) DeleteVertex(grid);
            }
        } // Delete_Click

        void DeleteGraph(Grid grid)
        {
            MessageBoxResult dialog = MessageBox.Show($"Do you want to delete Graph?", "Deleting graph", MessageBoxButton.YesNo);

            if (dialog == MessageBoxResult.Yes)
            {
                myCanvas.Children.Clear();
                graph = null;
            } // if
        } // DeleteGraph

        void DeleteVertex(Grid grid)
        {
            TextBlock textBox = grid.Children[1] as TextBlock;
            Vertex vertex = (Vertex)textBox.DataContext;
            MessageBoxResult dialog = MessageBox.Show($"Do you want to delete Vertex \"{vertex.Title}\"?", "Deleting vertex", MessageBoxButton.YesNo);

            if (dialog == MessageBoxResult.Yes)
            {
                myCanvas.Children.Remove(grid);

                foreach (var link in vertex.Links)
                {
                    myCanvas.Children.Remove(links[link]);
                    links.Remove(link);
                }

                graph.RemoveVertex(vertex);
            } // if
        } // DeleteVertex

        void DeleteLink(Line line)
        {
            Link link = links.First(x => x.Value == line).Key;
            MessageBoxResult dialog = MessageBox.Show($"Do you want to delete link \"{link.Title}\"?", "Deleting link", MessageBoxButton.YesNo);

            if (dialog == MessageBoxResult.Yes)
            {
                graph.RemoveLink(link);
                myCanvas.Children.Remove(links[link]);
                links.Remove(link);
            }
        } // DeleteLink

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();

            else if (e.Key == Key.Delete)
                Delete_Click(null, null);
            else if (e.Key == Key.V)
                AddVertex_Click(null, null);
            else if (e.Key == Key.X)
                AddLink_Click(null, null);

            else if (e.Key == Key.F)
                FindPath_Click(null, null);

            else if (e.Key == Key.N && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                CreateGraph_Click(null, null);
            else if (e.Key == Key.O && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                Open_Click(null, null);
            else if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                Save_Click(null, null);
        } // Window_KeyDown

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".mgr";
            dlg.Filter = "Graph Files (*.mgr)|*.mgr";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                links.Clear();
                myCanvas.Children.Clear();
                graph = new Graph(figureWidth, figureHeight);
                graph.Load(dlg.FileName);
                DrawAll();
            }
        } // Open_Click

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (graph == null)
            {
                MessageBox.Show("Graph is empty. Create it first.");
                return;
            }
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();

            dlg.DefaultExt = ".mgr";
            dlg.Filter = "Graph Files (*.mgr)|*.mgr";

            bool? result = dlg.ShowDialog();

            if (result != null && result == true)
            {
                graph.Save(dlg.FileName);
            }
        } // Save_Click

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.Owner = this;
            window.ShowDialog();
        } // About_Click

        private void FindPath_Click(object sender, RoutedEventArgs e)
        {
            if (graph == null)
            {
                MessageBox.Show("Graph is empty. Create it first.");
                return;
            }
            VertexPathWindow window = new VertexPathWindow(graph);
            window.Owner = this;
            window.ShowDialog();
        } // FindPath_Click
    }
}
