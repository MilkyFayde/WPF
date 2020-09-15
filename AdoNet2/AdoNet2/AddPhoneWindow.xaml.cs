using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for AddPhoneWindow.xaml
    /// </summary>
    public partial class AddPhoneWindow : Window
    {
        DataTable dataTable;
        Action<int, string, int, int> action;
        public AddPhoneWindow(DataTable dataTable, Action<int, string, int, int> action)
        {
            InitializeComponent();
            this.dataTable = dataTable;
            this.action = action;
        }

        private void Button_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        } // Window_KeyDown

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            int manufactId = int.Parse(comboBox.SelectedValue.ToString());
            action.Invoke(manufactId, textBox1.Text, Size(textBox2.Text), Size(textBox3.Text));
        } // Ok_Click

        private void digitTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int iValue = -1;
            TextBox textBox = sender as TextBox;

            if (Int32.TryParse(textBox.Text, out iValue) == false)
            {
                TextChange textChange = e.Changes.ElementAt<TextChange>(0);
                int iAddedLength = textChange.AddedLength;
                int iOffset = textChange.Offset;

                textBox.Text = textBox.Text.Remove(iOffset, iAddedLength);
            }
        } // textBox2_TextChanged

        int Size(string text)
        {
            if (text.Length == 0) return 0;
            return int.Parse(text);
        } // Size

        private void comboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;

            comboBox.ItemsSource = dataTable.DefaultView;

            comboBox.DisplayMemberPath = "name";
            comboBox.SelectedValuePath = "id";
            comboBox.SelectedIndex = 0;
        } // comboBox_Loaded
    }
}
