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

namespace DragNDropPics
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
            string str1 = "1) Drop image files or folder to display them.\n";
            string str2 = "2) You can move image in window.\n";
            string str3 = "3) Click on image in window to select it.\n";
            string str4 = "4) Change size of selected image via \"Size\" slide bar.\n";
            string str5 = "5) Rotate selected image via \"Rotate\" slide bar.\n";

            textBlock1.Text = $"{str}{str1}{str2}{str3}{str4}{str5}";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        } // Window_KeyDown
    }
}
