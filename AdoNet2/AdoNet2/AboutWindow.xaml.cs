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
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            string str = "Ado.Net2.\n";
            string str1 = "1) Create 2 tables from comments to work with.\n";
            string str2 = "2) Load tables from MS SQL.\n";
            string str3 = "3) Edit tables.\n";
            string str4 = "4) Manufacturer table is in a relation with Phones table.\n";
            string str5 = "5) Sync with MS SQL to save changes.\n";

            textBlock1.Text = $"{str}{str1}{str2}{str3}{str4}{str5}";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        } // Window_KeyDown
    }
}
