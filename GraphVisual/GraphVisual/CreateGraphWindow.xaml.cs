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
    /// Interaction logic for CreateGraphWindow.xaml
    /// </summary>
    public partial class CreateGraphWindow : Window
    {
        Action<string> action;
        public CreateGraphWindow(Action<string> action)
        {
            InitializeComponent();
            this.action = action;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text.Length == 0)
            {
                MessageBox.Show("\"Graph Name\" field cant be empty.");
                return;
            }
            action.Invoke(textBox.Text);
            this.Close();
        } // Ok_Click

        private void Cancel_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();

        } // Window_KeyDown
    }
}
