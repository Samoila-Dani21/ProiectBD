using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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

namespace OnlineShop
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Meniu : Window
    {
        SqlConnection sqlConnection;
        DateTime date = DateTime.UtcNow.Date;
        public string codClientVechi { get; set; }
        public Meniu(string codClientVechi)
        {
            InitializeComponent();
            
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineShop.Properties.Settings.Evidenta_magazin_onlineConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            dataTextBox.Text = date.ToString("dd/MM/yyyy");
            ShowTipProdus();
            //ShowCodClient();
            codClientTextBox.Text = codClientVechi;
            //MessageBox.Show(codClientVechi);
        }
        private void ShowCodClient()
        {
            /*string query = "select top 1 cod_client from Adresa order by cod_client desc";
            string cod_client = "";
            sqlConnection.Open();

            SqlCommand cmdCodClient = new SqlCommand(query, sqlConnection);

            SqlDataReader codClientReader = cmdCodClient.ExecuteReader();
            //execute the SQLCommand


            //check if there are records
            if (codClientReader.HasRows)
            {
                while (codClientReader.Read())
                {
                    cod_client = codClientReader.GetString(0);

                    //display retrieved record
                    *//* Console.WriteLine(parola);
                     MessageBox.Show(parola);*//*
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }

            //close data reader
            codClientReader.Close();

            codClientTextBox.Text = cod_client;

            sqlConnection.Close();*/

            string query = "select top 1 cod_client from Adresa order by cod_client desc";

            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            // the SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-Objects
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            using (sqlDataAdapter)
            {
                //sqlCommand.Parameters.AddWithValue("@id_tip_produs", listProduse.SelectedValue);
                DataTable codClientDataTable = new DataTable();
                sqlDataAdapter.Fill(codClientDataTable);
                codClientTextBox.Text = codClientDataTable.Rows[0]["cod_client"].ToString();
            }
        }
        private void ShowTipProdus()
        {
            try
            {
                string query = "select * from Tipuri_produse";
                // the SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-Objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable tipProduseTable = new DataTable();

                    sqlDataAdapter.Fill(tipProduseTable);

                    //Which Information of the Table in DataTable should be shown in our ListBox?
                    listTipProdus.DisplayMemberPath = "tip_produs";
                    //Which Value should be delivered, when an Item from our ListBox is selected?
                    listTipProdus.SelectedValuePath = "id_tip_produs";
                    //The Reference to the Data the ListBox should populate
                    listTipProdus.ItemsSource = tipProduseTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }
        private void ShowProduse()
        {
            try
            {
                string query = "select * from Produse";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable produseTable = new DataTable();
                    sqlDataAdapter.Fill(produseTable);

                    listProduse.DisplayMemberPath = "nume_produs";
                    listProduse.SelectedValuePath = "id_tip_produs";
                    listProduse.ItemsSource = produseTable.DefaultView;

                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }
        private void ShowProduseAsociate()
        {
            try
            {
                string query = "select * from Produse a inner join Tipuri_produse" +
                    " tp on tp.id_tip_produs=a.id_tip_produs where a.id_tip_produs=@id_tip_produs";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                // the SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-Objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@id_tip_produs", listTipProdus.SelectedValue);

                    DataTable produseTable = new DataTable();

                    sqlDataAdapter.Fill(produseTable);

                    //Which Information of the Table in DataTable should be shown in our ListBox?
                    listProduse.DisplayMemberPath = "nume_produs";
                    //Which Value should be delivered, when an Item from our ListBox is selected?
                    listProduse.SelectedValuePath = "id_tip_produs";
                    //The Reference to the Data the ListBox should populate
                    listProduse.ItemsSource = produseTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
            }
        }

        private void listTipProdus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowProduseAsociate();
            ShowSelectedTipProdusInTextBox();
        }
        private void ShowCaracteristici()
        {
            string queryCaracteristici = "select caracteristici,pret,stoc from Produse where cod_produs = @cod_produs";

            SqlCommand sqlCommandCaracteristici = new SqlCommand(queryCaracteristici, sqlConnection);

            // the SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-Objects
            SqlDataAdapter sqlDataAdapterCaracteristici = new SqlDataAdapter(sqlCommandCaracteristici);
            using (sqlDataAdapterCaracteristici)
            {
                sqlCommandCaracteristici.Parameters.AddWithValue("@cod_produs", listProduse.SelectedValue);
                DataTable caracteristiciDataTable = new DataTable();
                sqlDataAdapterCaracteristici.Fill(caracteristiciDataTable);
                //produseTextBox.Text = produseDataTable.Rows[0]["nume_produs"].ToString();
            }

/*            string showProduse = "select * from Produse where id_tip_produs=@id_tip_produs  ";
            sqlConnection.Open();
            SqlCommand cmdProduse = new SqlCommand(showProduse, sqlConnection);
            cmdProduse.Parameters.Add("@id_tip_produs", listProduse.SelectedValue);
            SqlDataAdapter showProduseDataAdapter = new SqlDataAdapter(cmdProduse);
            DataSet ds = new DataSet();
            showProduseDataAdapter.Fill(ds, "Produse");*/
        }
        float suma = 1;
        int nr = 1;
        private void ShowSelectedProdusInTextBox()
        {
            try
            {
                string query = "select nume_produs from Produse where cod_produs = @cod_produs";
                
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                
                // the SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-Objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@cod_produs", listProduse.SelectedValue);
                    DataTable produseDataTable = new DataTable();
                    sqlDataAdapter.Fill(produseDataTable);
                    produseTextBox.Text = produseDataTable.Rows[0]["nume_produs"].ToString();
                }

                string showProduse = "select * from Produse where cod_produs=@cod_produs  ";
                sqlConnection.Open();
                SqlCommand cmdProduse = new SqlCommand(showProduse, sqlConnection);
                cmdProduse.Parameters.Add("@cod_produs", listProduse.SelectedValue);
                SqlDataAdapter showProduseDataAdapter = new SqlDataAdapter(cmdProduse);
                DataSet ds = new DataSet();
                showProduseDataAdapter.Fill(ds, "Produse");

                
                string sumaString = "";

                DataTable produseTable = ds.Tables[0];
                DataRow tempRow = null;
                foreach (DataRow tempRowVar in produseTable.Rows)
                {
                    tempRow = tempRowVar;
                    listCaracteristici.Items.Add(("Caracteristici: "+tempRow["caracteristici"] +
                    "\npret: " + tempRow["pret"] + "\nstoc: " + tempRow["stoc"]));
                    
                    sumaString=tempRow["pret"].ToString();
                }
                //showProduseDataAdapter.Fill(produseTable);
                ShowCaracteristici();
                string nrString = numarProduseTextBox.Text.ToString();
                
                nr = Convert.ToInt32(nrString);
                suma=float.Parse(sumaString);
                suma = (float)(suma * nr);
                sumaTextBox.Text = Convert.ToString(suma);
                //listClienti.ItemsSource = clientiTable.DefaultView;

            }
            catch (Exception e)
            {
                // MessageBox.Show(e.ToString());
            }

        }

        private void ShowSelectedTipProdusInTextBox()
        {
            try
            {
                string query = "select tip_produs from Tipuri_produse where id_tip_produs = @id_tip_produs";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                // the SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-Objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {

                    sqlCommand.Parameters.AddWithValue("@id_tip_produs", listTipProdus.SelectedValue);

                    DataTable tipProdusDataTable = new DataTable();

                    sqlDataAdapter.Fill(tipProdusDataTable);

                    tipProdusTextBox.Text = tipProdusDataTable.Rows[0]["tip_produs"].ToString();
                    
                }
            }
            catch (Exception e)
            {
                // MessageBox.Show(e.ToString());
            }
        }

        private void listProduse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listProduse.SelectedValuePath = "cod_produs";
            ShowSelectedProdusInTextBox();
        }

        private void Iesire_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Cumpara_Button_Click(object sender, RoutedEventArgs e)
        {
            /*try
            {*/
                string query = "insert into Comenzi (id_comanda,cod_client,cod_produs,nr_produse,suma) " +
                    "values ((select top 1 id_comanda from Comenzi order by id_comanda desc )+1," +
                    "@cod_client,@cod_produs,@nr_produs,@suma) insert into detalii_comanda " +
                    "(id_comanda,cod_produs,data_comanda) values ((select top 1 id_comanda " +
                    "from Comenzi order by id_comanda desc)," +
                    " (select top 1 cod_produs from Comenzi order by cod_produs desc ),@data)";

                //sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
             //   date.ToString("dd/MM/yyyy");
               // sqlCommand.Parameters.AddWithValue("@id_comanda", NumeTextBox.Text);
                sqlCommand.Parameters.AddWithValue("@cod_client", Convert.ToInt32(codClientVechi));
                sqlCommand.Parameters.AddWithValue("@cod_produs", listProduse.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@nr_produs", nr);
                sqlCommand.Parameters.AddWithValue("@suma", suma);
                sqlCommand.Parameters.AddWithValue("@data", date.ToString("MM/dd/yyyy"));
   

                sqlCommand.ExecuteScalar();
         /*   }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
            }*/
        }

        private void Statistici_Button_Click(object sender, RoutedEventArgs e)
        {
            Statistici statistici = new Statistici();
            statistici.Show();
            this.Close();
        }
        
    }
}
