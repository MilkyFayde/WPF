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
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            string str = "Drag N Drop.\n";
            string str1 = "1) Drop image file to cell to display it.\n";
            string str2 = "2) Change board size via menu.\n";
            string str3 = "3) Shuffle all images on board via menu.\n";

            textBlock1.Text = $"{str}{str1}{str2}{str3}";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        } // Window_KeyDown
    }
}
