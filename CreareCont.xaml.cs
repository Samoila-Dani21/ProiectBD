using Aspose.Cells.Drawing;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;


namespace OnlineShop
{

    
    public partial class CreareCont : Window
    {
        SqlConnection sqlConnection;
        public int var;
        DateTime date = DateTime.UtcNow.Date;
        string dataFinala;
        //DateTime dataFinala=DateT
        public CreareCont()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineShop.Properties.Settings.Evidenta_magazin_onlineConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            codClientTextBox.Text = Convert.ToString(getCodClient()+1);
            string yearString = date.Year.ToString();
            int yearInt = Convert.ToInt32(yearString)-18;
            yearString=Convert.ToString(yearInt);
            dataFinala = date.ToString("dd/MM/yyyy");
            dataFinala = dataFinala.Remove(6,4) + yearString;

            //ShowClienti();
        }
        private int getCodClient()
        {
            string query = "select * from (select count(*) as numar from clienti ) as a" +
                ", (select top 1 cod_client from Adresa order by cod_client desc) as cod";
            //string codCLient0 = "select count(*) from clienti";
            string cod_client;
            string nrClienti = "";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

            // the SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-Objects
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            using (sqlDataAdapter)
            {

                DataTable codClientDataTable = new DataTable();
                sqlDataAdapter.Fill(codClientDataTable);
                cod_client = codClientDataTable.Rows[0]["cod_client"].ToString();
                sqlDataAdapter.Fill(codClientDataTable);
                nrClienti = codClientDataTable.Rows[0]["numar"].ToString();
            }
            if (String.Compare(nrClienti, "0") == 0)
                return 0;
            
            return Convert.ToInt32(cod_client);
        }
        private void ShowClienti()
        {
            try
            {
                string showClienti = "select * from [Clienti]  ";
                sqlConnection.Open();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(showClienti,sqlConnection);
                DataSet ds = new DataSet();
                sqlDataAdapter.Fill(ds, "Clienti");
                
                DataTable clientiTable = ds.Tables[0];
                DataRow tempRow = null;
                foreach(DataRow tempRowVar in clientiTable.Rows)
                {
                    tempRow=tempRowVar;
                    listClienti.Items.Add((tempRow["cod_client"] + 
                    " (" + tempRow["nume"] + ") (" + tempRow["prenume"] + 
                    ")" + " (" + tempRow["cnp"] + ")"));
                    //var = tempRow["cod_client"];
                }
                sqlDataAdapter.Fill(clientiTable);

            //listClienti.ItemsSource = clientiTable.DefaultView;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        
        private void Sex_CheckedBarbat(object sender, RoutedEventArgs e)
        {
            sexCheckFemeie.IsChecked = false;
        }
        private void Sex_UncheckedBarbat(object sender, RoutedEventArgs e)
        {
            sexCheckFemeie.IsChecked = true;
        }
        private void Sex_CheckedFemeie(object sender, RoutedEventArgs e)
        {
            sexCheckBarbat.IsChecked = false;
        }
        private void Sex_UncheckedFemeie(object sender, RoutedEventArgs e)
        {
            sexCheckBarbat.IsChecked = true;
        }

        private void Creare_Button_Click(object sender, RoutedEventArgs e)
        {
            
            Boolean ok = false;
            string mesaj="";
            if (CNPTextBox.Text.Length != 13 || Equals(CNPTextBox.Text, ""))
            {
                ok = true;
                mesaj = "CNP-ul nu este corect!";
            }
            if (TelefonTextBox.Text.Length != 10 || Equals(TelefonTextBox, ""))
            {
                ok = true;
                mesaj += "\nNumarul de telefon nu corespunde politicii!";
            }
            if (String.Compare(DataText.Text, dataFinala) > 0 || Equals(DataText.Text, ""))
            {
                ok = true;
                mesaj += "\nNu aveti varsta necesara!";
            }
            if (Equals(NumeTextBox.Text, "") || Equals(PrenumeTextBox.Text, "") ||
                Equals(EmailTextBox.Text, "") || Equals(ParolaTextBox.Text = "") ||
                Equals(JudetTextBox.Text, "") || Equals(JudetTextBox.Text, "") || Equals(OrasTextBox.Text, "") ||
                Equals(StradaTextBox.Text, "") || Equals(NumarTextBox.Text, "") ||
                (!sexCheckBarbat.IsEnabled && !sexCheckFemeie.IsEnabled))
            {
                ok = true;
                mesaj += "\nTrebuiesc completate toate campurile!";
            }

            if (!ok)
            {
                /* try
                 *
                {*/
                
                var res = MessageBox.Show(
                    "Sunteti sigur ca ati introdus datele corect?", "Confirmare", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (res == System.Windows.Forms.DialogResult.Yes)
                {
                    MessageBox.Show("You have clicked yes Button");
                    string query = "insert into  Adresa (cod_client,judet,oras,strada,numar,bloc,apartament,etaj) " +
               "values (@cod_client,@judet,@oras,@strada,@numar,@bloc,@apartament,@etaj);" +
               " insert into Clienti (cod_client, nume,prenume,cnp,telefon, email, parola, sex, data_nasterii) " +
               "values ((select top 1 cod_client from Adresa order by cod_client desc),@nume,@prenume," +
                   "@cnp,@telefon,@email,@parola,@sex,@data_nasterii); ";
                    int indexCodClient = getCodClient();

                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@nume", NumeTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("@prenume", PrenumeTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("@cnp", CNPTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("@telefon", TelefonTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("@email", EmailTextBox.Text);
                    sqlCommand.Parameters.AddWithValue("@parola", ParolaTextBox.Text);
                    if (sexCheckBarbat.IsEnabled)
                        sqlCommand.Parameters.AddWithValue("@sex", "m");
                    else
                        sqlCommand.Parameters.AddWithValue("@sex", "f");
                    sqlCommand.Parameters.AddWithValue("@data_nasterii", DataText.Text);
                    sqlCommand.Parameters.Add("@cod_client", ++indexCodClient);
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
                    sqlConnection.Open();
                    sqlCommand.ExecuteScalar();
                    sqlConnection.Close();
                    /*  }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.ToString());
                      }
                      finally
                      {
                          sqlConnection.Close();
                          ShowClienti();
                      }*/

                    Meniu meniu = new Meniu(ParolaTextBox.Text);
                    meniu.Show();
                    this.Close();
                }
                if (res == System.Windows.Forms.DialogResult.No)
                {
                    MessageBox.Show("You have clicked no Button");
                    //Some task…
                }


            }
            else
            {
                MessageBox.Show(mesaj,"Informare", (MessageBoxButtons)MessageBoxButton.OK,MessageBoxIcon.Information);
            }
        }

        private void Iesire_Button_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
    }
}
