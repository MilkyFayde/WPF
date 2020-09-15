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

namespace AdoNet2
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        Action<string, string, string, int> action;
        public AddWindow(string label1, string label2, string label3, string label4, Action<string, string, string, int> action)
        {
            InitializeComponent();
            this.label1.Content = $"Enter {label1}:";
            this.label2.Content = $"Enter {label2}:";
            this.label3.Content = $"Enter {label3}:";
            this.label4.Content = $"Enter {label4}:";
            this.action = action;
        }

        private void textBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
            int iValue = -1;

            if (Int32.TryParse(textBox4.Text, out iValue) == false)
            {
                TextChange textChange = e.Changes.ElementAt<TextChange>(0);
                int iAddedLength = textChange.AddedLength;
                int iOffset = textChange.Offset;

                textBox4.Text = textBox4.Text.Remove(iOffset, iAddedLength);
            }
        } // textBox4_TextChanged

        int Size()
        {
            if (textBox4.Text.Length == 0) return 1;
            return int.Parse(textBox4.Text);
        } // Size

        private void Button_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            action.Invoke(textBox1.Text, textBox2.Text, textBox3.Text, Size());
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        } // Window_KeyDown
    }
}
