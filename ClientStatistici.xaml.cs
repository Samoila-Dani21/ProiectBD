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
using System.Windows.Shapes;

namespace OnlineShop
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ClientStatistici : Window
    {
        SqlConnection sqlConnection;
        public ClientStatistici()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineShop.Properties.Settings.Evidenta_magazin_onlineConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
           // sqlConnection.Open();
            ShowClienti();
            ShowAdresa();
            ScrieClienti();
            ScrieAdresa();
        }
        private void ScrieAdresa()
        {
            string var = codClientTextBox.Text.ToString();
            int cod_client = Convert.ToInt32(var);
            string query = "select * from Adresa where cod_client=@cod_client  ";
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.Add("@cod_client", cod_client);
            // the SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-Objects
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            using (sqlDataAdapter)
            {
                //sqlCommand.Parameters.AddWithValue("@id_tip_produs", listProduse.SelectedValue);
                DataTable codClientDataTable = new DataTable();
                sqlDataAdapter.Fill(codClientDataTable);
                // codClientTextBox.Text = codClientDataTable.Rows[0]["cod_client"].ToString();
                JudetTextBox.Text = codClientDataTable.Rows[0]["judet"].ToString();
                OrasTextBox.Text = codClientDataTable.Rows[0]["oras"].ToString();
                StradaTextBox.Text = codClientDataTable.Rows[0]["strada"].ToString();
                NumarTextBox.Text = codClientDataTable.Rows[0]["numar"].ToString();
                BlocTextBox.Text = codClientDataTable.Rows[0]["bloc"].ToString();
                ApartamentTextBox.Text = codClientDataTable.Rows[0]["apartament"].ToString();
                EtajTextBox.Text = codClientDataTable.Rows[0]["etaj"].ToString();

            }
            sqlConnection.Close();
        }
        private void ScrieClienti()
        {
            string var = codClientTextBox.Text.ToString();
            int cod_client=Convert.ToInt32(var);
            string query = "select * from Clienti where cod_client=@cod_client  ";
            sqlConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlCommand.Parameters.Add("@cod_client", cod_client);
            // the SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-Objects
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            using (sqlDataAdapter)
            {
                //sqlCommand.Parameters.AddWithValue("@id_tip_produs", listProduse.SelectedValue);
                DataTable codClientDataTable = new DataTable();
                sqlDataAdapter.Fill(codClientDataTable);
               // codClientTextBox.Text = codClientDataTable.Rows[0]["cod_client"].ToString();
                NumeTextBox.Text = codClientDataTable.Rows[0]["nume"].ToString();
                PrenumeTextBox.Text = codClientDataTable.Rows[0]["prenume"].ToString();
                CNPTextBox.Text = codClientDataTable.Rows[0]["cnp"].ToString();
                TelefonTextBox.Text = codClientDataTable.Rows[0]["telefon"].ToString();
                EmailTextBox.Text = codClientDataTable.Rows[0]["email"].ToString();
                ParolaTextBox.Text = codClientDataTable.Rows[0]["parola"].ToString();
                sexTextBox.Text = codClientDataTable.Rows[0]["sex"].ToString();
                dataTextBox.Text = codClientDataTable.Rows[0]["data_nasterii"].ToString();
            }
            sqlConnection.Close();
        }

        private void ShowClienti()
        {
            try
            {
                string query = "select * from Clienti";
                // the SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-Objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable zooTable = new DataTable();
                    sqlDataAdapter.Fill(zooTable);
                    listClientiGrid.ItemsSource = zooTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }
        private void ShowAdresa()
        {
            try
            {
                string query = "select * from Adresa";
                // the SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-Objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable zooTable = new DataTable();
                    sqlDataAdapter.Fill(zooTable);
                    listAdresaGrid.ItemsSource = zooTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }
        private int getCodClient()
        {
            string query = "select top 1 cod_client from Adresa order by cod_client desc ";

            string cod_client;
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            // the SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-Objects
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            using (sqlDataAdapter)
            {
                
                DataTable codClientDataTable = new DataTable();
                sqlDataAdapter.Fill(codClientDataTable);
                cod_client = codClientDataTable.Rows[0]["cod_client"].ToString();
            }

            return Convert.ToInt32(cod_client);
        }
        private void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            
                sqlConnection.Open();
                string query = "update Adresa set"+
 " cod_client = @cod_client,judet = @judet,oras = @oras,strada = @strada,numar = @numar,bloc = @bloc, " +
 "apartament = @apartament,etaj = @etaj" +
" where cod_client = @cod_client " +
"update Clienti "+
 "set cod_client = @cod_client, nume = @nume, prenume = @prenume, cnp = @cnp," +
 "telefon = @telefon, email = @email, parola = @parola, sex = @sex,data_nasterii = @data_nasterii "+
  " where cod_client = @cod_client";
                int indexCodClient = getCodClient();

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@nume", NumeTextBox.Text);
                sqlCommand.Parameters.AddWithValue("@prenume", PrenumeTextBox.Text);
                sqlCommand.Parameters.AddWithValue("@cnp", CNPTextBox.Text);
                sqlCommand.Parameters.AddWithValue("@telefon", TelefonTextBox.Text);
                sqlCommand.Parameters.AddWithValue("@email", EmailTextBox.Text);
                sqlCommand.Parameters.AddWithValue("@parola", ParolaTextBox.Text);
                if (sexTextBox.Text == "Barbat")
                    sqlCommand.Parameters.AddWithValue("@sex", "m");
                else
                    sqlCommand.Parameters.AddWithValue("@sex", "f");
                sqlCommand.Parameters.AddWithValue("@data_nasterii", dataTextBox.Text);
                sqlCommand.Parameters.Add("@cod_client", codClientTextBox.Text);
                sqlCommand.Parameters.AddWithValue("@judet", JudetTextBox.Text);
                sqlCommand.Parameters.AddWithValue("@oras", OrasTextBox.Text);
                sqlCommand.Parameters.AddWithValue("@strada", StradaTextBox.Text);
                sqlCommand.Parameters.AddWithValue("@numar", NumarTextBox.Text);
                if (Equals(BlocTextBox.Text.ToString(), ""))
                    sqlCommand.Parameters.AddWithValue("@bloc", "-");
                else
                    sqlCommand.Parameters.AddWithValue("@bloc", BlocTextBox.Text);
                if (Equals(ApartamentTextBox.Text.ToString(), ""))
                    sqlCommand.Parameters.AddWithValue("@apartament", "-");
                else
                    sqlCommand.Parameters.AddWithValue("@apartament", ApartamentTextBox.Text);
                if (Equals(EtajTextBox.Text.ToString(), ""))
                    sqlCommand.Parameters.AddWithValue("@etaj", "-");
                else
                    sqlCommand.Parameters.AddWithValue("@etaj", EtajTextBox.Text);
                
                sqlCommand.ExecuteScalar();
                sqlConnection.Close();
            ScrieClienti();
            ScrieAdresa();
            ShowClienti();
            ShowAdresa();
        }
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {

            sqlConnection.Open();
            string query = " delete from Clienti where cod_client = @cod_client;"+
            " delete from Adresa where cod_client = @cod_client; ";
            

            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
          
            sqlCommand.Parameters.Add("@cod_client", codClientTextBox.Text);
            

            sqlCommand.ExecuteScalar();
            sqlConnection.Close();
            ShowClienti();
            ShowAdresa();
        }
        private void Button_Statistici(object sender, RoutedEventArgs e) 
        { 
            Statistici statistici= new Statistici();
            statistici.Show();
            this.Close();
        }

        private void codClientTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           // ScrieAdresa();
           // ScrieClienti();
        }
    }
}
