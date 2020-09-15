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
    /// Interaction logic for CreateVertexWindow.xaml
    /// </summary>
    public partial class CreateVertexWindow : Window
    {
        Func<string, int, bool> action;
        public CreateVertexWindow(Func<string, int, bool> action)
        {
            InitializeComponent();
            this.action = action;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text.Length == 0)
            {
                MessageBox.Show("\"Vertex Name\" field cant be empty.");
                return;
            }

            if(!action.Invoke(textBox.Text, Size()))
            {
                MessageBox.Show($"Vertex with name \"{textBox.Text}\" already exists.");
                return;
            }
            
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) => this.Close();

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            int iValue = -1;

            if (Int32.TryParse(textBox2.Text, out iValue) == false)
            {
                TextChange textChange = e.Changes.ElementAt<TextChange>(0);
                int iAddedLength = textChange.AddedLength;
                int iOffset = textChange.Offset;

                textBox2.Text = textBox2.Text.Remove(iOffset, iAddedLength);
                return;
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
    }
}
