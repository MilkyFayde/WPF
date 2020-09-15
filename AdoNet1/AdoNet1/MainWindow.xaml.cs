using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdoNet1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection cn;
        public MainWindow()
        {
            InitializeComponent();
            cn = new SqlConnection();
            string connection = ConfigurationManager.ConnectionStrings["AdoNet1.Properties.Settings.mssqlConnectionString"].ConnectionString;
            cn.ConnectionString = connection;

        }

        private void verbWordsButton_Click_1(object sender, RoutedEventArgs e)
        {
            //CREATE TABLE VerbWords(id int primary key, name nvarchar(20) NULL, count int NULL)

            if (!System.IO.Directory.Exists(pathTextBox.Text))
            {
                MessageBox.Show($"Path \"{pathTextBox.Text}\" doesn't exist");
                return;
            }

            string[] endsWith = new string[] {
                "ешь","ет", "ем","ете", "ут","ют","ишь","ит","им","ите","aт", "ят", "ить", "ять"};

            TxtReader reader = new TxtReader(pathTextBox.Text, endsWith); // class to find words from .txt files by endings
            Dictionary<string, int> verbWords = reader.GetWords(); // get found words

            AddToSQL(verbWords, "VerbWords");
        } // verbWordsButton_Click_1

        private void adjectiveWordsButton_Click(object sender, RoutedEventArgs e)
        {
            //CREATE TABLE AdjectiveWords(id int primary key, name nvarchar(20) NULL, count int NULL)

            if (!System.IO.Directory.Exists(pathTextBox.Text))
            {
                MessageBox.Show($"Path \"{pathTextBox.Text}\" doesn't exist");
                return;
            }

            string[] endsWith = new string[] {
                "ой","ей", "ому","ему", "ый","ий","ья","ье","ые","ая","ое",};

            TxtReader reader = new TxtReader(pathTextBox.Text, endsWith); // class to find words from .txt files by endings
            Dictionary<string, int> adjectiveWords = reader.GetWords(); // get found words

            AddToSQL(adjectiveWords, "AdjectiveWords");
        } // adjectiveWordsButton_Click

        void AddToSQL(Dictionary<string, int> words, string tableName)
        {
            string command = $"insert into {tableName} (id, name, count) values (@id, @name, @cnt)"; // sql command

            SqlCommand sqlCmd = new SqlCommand(command, cn);
            SqlParameter param1 = new SqlParameter("@id", SqlDbType.Int, 4);
            sqlCmd.Parameters.Add(param1);

            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "@name";
            param2.SqlDbType = SqlDbType.NVarChar;
            param2.Size = 20;
            sqlCmd.Parameters.Add(param2);

            SqlParameter param3 = new SqlParameter("@cnt", SqlDbType.Int, 4);
            sqlCmd.Parameters.Add(param3);

            cn.Open();

            int id = 0;
            listBox1.Items.Clear();
            foreach (var word in words)
            {
                string str = id.ToString() + " " + word.Key + " " + word.Value;
                param1.Value = id++;
                param2.Value = word.Key;
                param3.Value = word.Value;

                sqlCmd.ExecuteNonQuery();
                listBox1.Items.Add(str);
            } // foreach
            cn.Close();
        } // AddToSQL

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.Owner = this;
            window.ShowDialog();
        } // aboutButton_Click

        private void clearAdjectiveButton_Click(object sender, RoutedEventArgs e) => ExecuteSQLCommand("delete AdjectiveWords");

        private void clearVerbButton_Click(object sender, RoutedEventArgs e) => ExecuteSQLCommand("delete VerbWords");

        private void createAdjectiveButton_Click(object sender, RoutedEventArgs e) => ExecuteSQLCommand("CREATE TABLE AdjectiveWords (id int primary key, name nvarchar (20) NULL, count int NULL)");

        private void createVerbButton_Click(object sender, RoutedEventArgs e) => ExecuteSQLCommand("CREATE TABLE VerbWords (id int primary key, name nvarchar (20) NULL, count int NULL)");

        void ExecuteSQLCommand(string command)
        {
            cn.Open();
            SqlCommand sqlCmd = new SqlCommand(command, cn);
            sqlCmd.ExecuteNonQuery();
            cn.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
    } // class MainWindow 
}
