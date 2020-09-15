using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sapper
{
    /// <summary>
    /// Interaction logic for ChangeBoardSizeWindow.xaml
    /// </summary>
    public partial class ChangeBoardSizeWindow : Window
    {
        Regex regex = new Regex("d+");
        int size;
        string lastText = "";
        public Action<int> action { get; set; }

        public ChangeBoardSizeWindow()
        {
            InitializeComponent();
        }

        bool IsTextAllowed(string text)
        {
            return regex.IsMatch(text);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (size < 5) size = 5;
            if (size > 15) size = 15;
            action.Invoke(size);
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
            size = int.Parse(textBox.Text);
        } // TextBox_TextChanged

        private void TextBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
    }
}
