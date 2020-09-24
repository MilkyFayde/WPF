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
using System.Windows.Shapes;

namespace GraphVisual
{
    /// <summary>
    /// Interaction logic for VertexPathWindow.xaml
    /// </summary>
    public partial class VertexPathWindow : Window
    {
        Graph graph;
        public VertexPathWindow(Graph graph)
        {
            InitializeComponent();
            this.graph = graph;
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = graph.Vertices;
            comboBox.DisplayMemberPath = "Title";
            comboBox.SelectedValuePath = "Id";
            comboBox.SelectedIndex = 0;
        } // ComboBox2_Loaded

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            int id1 = int.Parse(comboBox1.SelectedValue.ToString());
            int id2 = int.Parse(comboBox2.SelectedValue.ToString());

            if (id1 == id2)
            {
                MessageBox.Show("Vertex \"from\" and vertex \"to\" cant be the same.");
                return;
            }
            Vertex from = graph.FindVertex(id1);
            Vertex to = graph.FindVertex(id2);

            List<Vertex> vertices = graph.DijkstraSearch(from, to);
            if(vertices == null)
            {
                MessageBox.Show("No path found.");
                return;
            }

            listBox1.Items.Clear();
            foreach (var vertex in vertices)
            {
                ListBoxItem item = new ListBoxItem();
                item.Content = vertex.Title;
                listBox1.Items.Add(item);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
