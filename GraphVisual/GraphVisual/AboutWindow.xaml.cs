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
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            string str = "Graphs Visualizer.\n";
            string str1 = "1) Create or Open existing \"Graph\" via menu or hotkeys.\n";
            string str2 = "2) Add \"Vertices\" via menu or hotkeys.\n";
            string str3 = "3) Add \"Links\" between \"Vertices\" via menu or hotkeys.\n";
            string str4 = "4) Click on \"Vertex\" to  select it. Drag it to move.\n";
            string str5 = "5) Delete selected \"Graph\", \"Vertex\" or  \"Link\" via menu or hotkeys.\n";
            string str6 = "5) Save created \"Graph\" via menu or hotkeys.\n";

            textBlock1.Text = $"{str}{str1}{str2}{str3}{str4}{str5}{str6}";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        } // Window_KeyDown
    }
}
