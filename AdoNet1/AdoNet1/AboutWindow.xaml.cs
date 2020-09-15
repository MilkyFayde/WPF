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

namespace AdoNet1
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            string str = "Ado.Net.\n";
            string str1 = "1) Enter folder path to scan for .txt files.\n";
            string str2 = "2) Read all .txt files from entered path, add all Verb Words to \"list\" and to MS SQL VerbWords table.\n";
            string str3 = "3) Read all .txt files from entered path, add all Adjective Words to \"list\" and to MS SQL Adjectivetable.\n";

            textBlock1.Text = $"{str}{str1}{str2}{str3}";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
    }
}
