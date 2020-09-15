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

namespace AdoNet2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection cn;
        //DataTable manufactTable = new DataTable();
       // DataTable phonesTable = new DataTable();
        SqlDataAdapter manufactAdapter;
        SqlDataAdapter phonesAdapter;
        DataSet dataSet = new DataSet();

        public MainWindow()
        {
            InitializeComponent();
            cn = new SqlConnection();
            string connection = ConfigurationManager.ConnectionStrings["AdoNet2.Properties.Settings.mssqlConnectionString"].ConnectionString;
            cn.ConnectionString = connection;
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            // CREATE TABLE Manufact(id int primary key, name varchar(15) NULL, country varchar(15) NULL, city varchar(15) NULL, employees int NULL)
            // insert into Manufact values(0, 'Apple', 'USA', 'California', 137000)
            // insert into Manufact values(1, 'Samsung', 'South Korea', 'Seoul', 309630)
            // insert into Manufact values(2, 'Huawei', 'China', 'Shenzhen', 194000 )

            //CREATE TABLE Phones(id int primary key, Manufact_Id int, model varchar(15) NULL, price int NULL, weight int NULL)

            //insert into Phones values(0, 0, 'Iphone 8 Plus', 500, 194 )
            //insert into Phones values(1, 0, 'Iphone X', 600, 188 )
            //insert into Phones values(2, 0, 'Iphone 11', 650, 190 )
            //insert into Phones values(3, 0, 'Iphone 11 Pro', 750, 179 )
            //insert into Phones values(4, 1, 'Galaxy S20', 705, 199 )
            //insert into Phones values(5, 1, 'Galaxy Note 20', 969, 222 )
            //insert into Phones values(6, 1, 'Galaxy Note 20U', 889, 199 )
            //insert into Phones values(7, 1, 'Galaxy S10', 500, 180 )
            //insert into Phones values(8, 2, 'P30 Pro', 1000, 205 )
            //insert into Phones values(9, 2, 'Mate 20 Pro', 900, 211 )
            //insert into Phones values(10, 2, 'P30', 750, 197 )
            //insert into Phones values(11, 2, 'P40 Pro', 1100, 216 )
            dataSet.Clear();
            LoadManufactToDataGrid();
            LoadPhonesToDataGrid();

            if(dataSet.Relations.Count > 0) return;
            DataRelation drel = new DataRelation("dr1", dataSet.Tables["Manufact"].Columns["id"], dataSet.Tables["Phones"].Columns["Manufact_Id"]);
            dataSet.Relations.Add(drel);
        } // Load_Click

        void LoadManufactToDataGrid()
        {
            SqlCommand command = cn.CreateCommand();
            command.CommandText = "select id, name, country, city, employees from Manufact";
            manufactAdapter = new SqlDataAdapter(command);
            manufactAdapter.Fill(dataSet, "Manufact");
            manufactDataGrid.ItemsSource = dataSet.Tables[0].DefaultView;
            manufactDataGrid.CanUserAddRows = false;
            manufactDataGrid.Columns[0].IsReadOnly = true;
        } // LoadManufactToDataGrid
        void LoadPhonesToDataGrid()
        {
            SqlCommand command = cn.CreateCommand();
            command.CommandText = "select id, Manufact_Id, model, price, weight from Phones";
            phonesAdapter = new SqlDataAdapter(command);
            phonesAdapter.Fill(dataSet, "Phones");
            phonesDataGrid.ItemsSource = dataSet.Tables[1].DefaultView;
            phonesDataGrid.CanUserAddRows = false;
            phonesDataGrid.Columns[0].IsReadOnly = true;
            phonesDataGrid.Columns[1].IsReadOnly = true;
        } // LoadManufactToDataGrid
        private void AddManufact_Click(object sender, RoutedEventArgs e)
        {
            if (dataSet.Tables.Count < 2) return;
            AddWindow window = new AddWindow("name", "country", "city", "employees", AddManufact);
            window.Owner = this;
            window.ShowDialog();
        } // AddManufact_Click
        void AddManufact(string name, string country, string city, int empl)
        {
            int id = dataSet.Tables[0].Rows[dataSet.Tables[0].Rows.Count - 1].Field<int>(0);
            DataRow dr = dataSet.Tables[0].NewRow();
            dr["id"] = ++id;
            dr["name"] = name;
            dr["country"] = country;
            dr["city"] = city;
            dr["employees"] = empl;
            dataSet.Tables[0].Rows.Add(dr);
        } // AddManufact
        private void Sync_Click(object sender, RoutedEventArgs e)
        {
            if (dataSet.Tables.Count < 2) return;
            SyncManufact();
            SyncPhones();
            dataSet.Clear();

            manufactAdapter.Fill(dataSet, "Manufact");
            phonesAdapter.Fill(dataSet, "Phones");
        } // Sync_Click
        void SyncManufact()
        {
            string com = "insert into Manufact(id, name, country, city, employees) values (@p1, @p2, @p3, @p4,@p5)";
            SqlCommand command = new SqlCommand(com, cn);
            command.Parameters.Add("@p1", SqlDbType.Int, 4, "id");
            command.Parameters.Add("@p2", SqlDbType.VarChar, 15, "name");
            command.Parameters.Add("@p3", SqlDbType.VarChar, 15, "country");
            command.Parameters.Add("@p4", SqlDbType.VarChar, 15, "city");
            command.Parameters.Add("@p5", SqlDbType.Int, 8, "employees");
            manufactAdapter.InsertCommand = command;

            com = "delete from Manufact where id=@p1";
            command = new SqlCommand(com, cn);
            command.Parameters.Add("@p1", SqlDbType.Int, 4, "id");
            manufactAdapter.DeleteCommand = command;

            com = "update Manufact set name=@p2, country=@p3, city=@p4, employees=@p5 where au_id=@p1";
            command = new SqlCommand(com, cn);
            command.Parameters.Add("@p1", SqlDbType.Int, 4, "id");
            command.Parameters.Add("@p2", SqlDbType.VarChar, 15, "name");
            command.Parameters.Add("@p3", SqlDbType.VarChar, 15, "country");
            command.Parameters.Add("@p4", SqlDbType.VarChar, 15, "city");
            command.Parameters.Add("@p5", SqlDbType.Int, 8, "employees");
            manufactAdapter.UpdateCommand = command;

            manufactAdapter.Update(dataSet.Tables[0]);
        } // SyncManufact
        void SyncPhones()
        {
            string com = "insert into Phones(id, Manufact_Id, model, price, weight) values (@p1, @p2, @p3, @p4,@p5)";
            SqlCommand command = new SqlCommand(com, cn);
            command.Parameters.Add("@p1", SqlDbType.Int, 4, "id");
            command.Parameters.Add("@p2", SqlDbType.Int, 4, "Manufact_Id");
            command.Parameters.Add("@p3", SqlDbType.VarChar, 15, "model");
            command.Parameters.Add("@p4", SqlDbType.Int, 8, "price");
            command.Parameters.Add("@p5", SqlDbType.Int, 8, "weight");
            phonesAdapter.InsertCommand = command;

            com = "delete from Phones where id=@p1";
            command = new SqlCommand(com, cn);
            command.Parameters.Add("@p1", SqlDbType.Int, 4, "id");
            phonesAdapter.DeleteCommand = command;

            com = "update Phones set Manufact_Id=@p2, model=@p3, price=@p4, weight=@p5 where au_id=@p1";
            command = new SqlCommand(com, cn);
            command.Parameters.Add("@p1", SqlDbType.Int, 4, "id");
            command.Parameters.Add("@p2", SqlDbType.Int, 4, "Manufact_Id");
            command.Parameters.Add("@p3", SqlDbType.VarChar, 15, "model");
            command.Parameters.Add("@p4", SqlDbType.Int, 8, "price");
            command.Parameters.Add("@p5", SqlDbType.Int, 8, "weight");
            phonesAdapter.UpdateCommand = command;

            phonesAdapter.Update(dataSet.Tables[1]);
        } // SyncPhones
        private void AddPhone_Click(object sender, RoutedEventArgs e)
        {
            if (dataSet.Tables.Count < 2) return;
            AddPhoneWindow window = new AddPhoneWindow(dataSet.Tables[0], AddPhone);
            window.Owner = this;
            window.ShowDialog();
        } // AddPhone_Click
        void AddPhone(int manufactId, string model, int price, int weight)
        {
            int id = dataSet.Tables[1].Rows[dataSet.Tables[1].Rows.Count - 1].Field<int>(0);
            DataRow dr = dataSet.Tables[1].NewRow();
            dr["id"] = ++id;
            dr["Manufact_Id"] = manufactId;
            dr["model"] = model;
            dr["price"] = price;
            dr["weight"] = weight;
            dataSet.Tables[1].Rows.Add(dr);
        } // AddManufact

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        } // Window_KeyDown

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow window = new AboutWindow();
            window.Owner = this;
            window.ShowDialog();
        } // About_Click
    }
}
