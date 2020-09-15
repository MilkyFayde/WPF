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

namespace DragNDropPics2
{
    /// <summary>
    /// Interaction logic for ChangeSizeWindow.xaml
    /// </summary>
    public partial class ChangeSizeWindow : Window
    {
        public Action<int> action { get; set; }
        public ChangeSizeWindow(int size)
        {
            InitializeComponent();
            textBox1.Text = size.ToString();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            int iValue = -1;

            if (Int32.TryParse(textBox.Text, out iValue) == false)
            {
                TextChange textChange = e.Changes.ElementAt<TextChange>(0);
                int iAddedLength = textChange.AddedLength;
                int iOffset = textChange.Offset;

                textBox.Text = textBox.Text.Remove(iOffset, iAddedLength);
                return;
            }
        } // TextBox_TextChanged

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text.Length == 0) textBox1.Text = "0";
            int size = int.Parse(textBox1.Text);
            if (size < 4) size = 4;
            if (size > 10) size = 10;
            action.Invoke(size);
            this.Close();
        } // Ok_Click

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TextBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        } // TextBox_PreviewExecuted

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        } // Window_KeyDown
    }
}
