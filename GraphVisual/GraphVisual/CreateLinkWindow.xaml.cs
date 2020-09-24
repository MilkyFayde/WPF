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
    /// Interaction logic for CreateLinkWindow.xaml
    /// </summary>
    public partial class CreateLinkWindow : Window
    {
        Func<string, int, int, int, bool> action;
        Graph graph;
        int id1 = -1;
        int id2 = -1;
        public CreateLinkWindow(Func<string, int, int, int, bool> action, Graph graph)
        {
            InitializeComponent();
            this.action = action;
            this.graph = graph;
        }

        public CreateLinkWindow(Func<string, int, int, int, bool> action, Graph graph, int id1, int id2)
        {
            InitializeComponent();
            this.action = action;
            this.graph = graph;
            this.id1 = id1;
            this.id2 = id2;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            int id1 = int.Parse(comboBox1.SelectedValue.ToString());
            int id2 = int.Parse(comboBox2.SelectedValue.ToString());

            if (textBox.Text.Length == 0)
            {
                MessageBox.Show("\"Vertex Name\" field cant be empty.");
                return;
            }

            if (id1 == id2)
            {
                MessageBox.Show("Vertex cant be linked with himself.");
                return;
            }

            if (!action.Invoke(textBox.Text, Size(), id1, id2))
            {
                MessageBox.Show($"Link from \"{comboBox1.Text}\" to \"{comboBox2.Text}\" already exists.");
                return;
            }

            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => this.Close();

        void ChangeSelectedIndex(int id, ComboBox comboBox)
        {
            int Selected = -1;
            int count = comboBox.Items.Count;
            for (int i = 0; i <= count - 1; i++)
            {
                comboBox.SelectedIndex = i;
                if ((int)(comboBox.SelectedValue) == id)
                {
                    Selected = i;
                    break;
                }
            }
            comboBox.SelectedIndex = Selected;
        } // ChangeSelectedIndex

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            int iValue = -1;

            if (Int32.TryParse(textBox2.Text, out iValue) == false)
            {
                TextChange textChange = e.Changes.ElementAt<TextChange>(0);
                int iAddedLength = textChange.AddedLength;
                int iOffset = textChange.Offset;

                textBox2.Text = textBox2.Text.Remove(iOffset, iAddedLength);
            }
        } // textBox2_TextChanged

        int Size()
        {
            if (textBox2.Text.Length == 0) return 0;
            int size = int.Parse(textBox2.Text);
            return size;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        } // Window_KeyDown

        private void ComboBox1_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = graph.Vertices;
            comboBox.DisplayMemberPath = "Title";
            comboBox.SelectedValuePath = "Id";
            comboBox.SelectedIndex = 0;
            if (id1 >= 0) ChangeSelectedIndex(id1, comboBox);
        } // ComboBox2_Loaded

        private void ComboBox2_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = graph.Vertices;
            comboBox.DisplayMemberPath = "Title";
            comboBox.SelectedValuePath = "Id";
            comboBox.SelectedIndex = 0;
            if (id2 >= 0) ChangeSelectedIndex(id2, comboBox);
        } // ComboBox2_Loaded
    }
}
