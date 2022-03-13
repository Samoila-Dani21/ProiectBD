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

namespace OnlineShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;
        
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["OnlineShop.Properties.Settings.Evidenta_magazin_onlineConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            Afisare_parola.IsChecked = true;

        }
        private int getCodClient(string var)
        {
            string querySpecial = "select cod_client from clienti where parola=@parola ";

            string cod_client;
            SqlCommand sqlCommand = new SqlCommand(querySpecial, sqlConnection);
            sqlCommand.Parameters.Add("@parola", var);
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

        SqlDataReader parolaReader;
        SqlDataReader userEmailReader;
        SqlDataReader userTelefonReader;
        private void Button_Click_Inainte(object sender, EventArgs e)
        {
            string parola="";
            string userEmail="";
            string userTelefon="";
            try
            {
                //Initializare 
                if (String.Compare(parolaBox.Password.ToString(), parolaTextBox.Text.ToString()) < 0)
                    parolaTextBox.Text = parolaBox.Password;
                else
                    parolaBox.Password = parolaTextBox.Text;

                //Interogarile pentru parola si username
                string queryParola = "select parola from Clienti where " +
                    "(email=@username or telefon=@username)";
                string queryUsernameEmail = "select email from Clienti where " +
                    "(parola=@parola or parola=@parolaCopie)";
                string queryUsernameTelefon = "select telefon from Clienti where " +
                    "(parola=@parola or parola=@parolaCopie)";

                //open connection
                sqlConnection.Open();
                
                //define the SqlCommand object
                SqlCommand sqlParola = new SqlCommand(queryParola, sqlConnection);
                SqlCommand sqlUsernameEmail = new SqlCommand(queryUsernameEmail, sqlConnection);
                SqlCommand sqlUsernameTelefon = new SqlCommand(queryUsernameTelefon, sqlConnection);
    
                //Scrierea valorilor in variabilele din sql
                sqlParola.Parameters.Add("@username", numeUtilizator.Text.ToString());

                sqlUsernameEmail.Parameters.Add("@parolaCopie", parolaTextBox.Text.ToString());
                sqlUsernameEmail.Parameters.Add("@parola", parolaBox.ToString());

                sqlUsernameTelefon.Parameters.Add("@parolaCopie", parolaTextBox.Text.ToString());
                sqlUsernameTelefon.Parameters.Add("@parola", parolaBox.ToString());
                
                //execute the SQLCommand
                parolaReader = sqlParola.ExecuteReader();
 
                //check if there are records
                if (parolaReader.HasRows)
                {
                    while (parolaReader.Read() )
                    {
                        parola = parolaReader.GetString(0);                      
                    }
                }
                else
                {
                    Console.WriteLine("No data found.");
                }

                //close data reader
                parolaReader.Close();

                //se deshcide alt reader
                userEmailReader = sqlUsernameEmail.ExecuteReader();

                //check if there are records
                if (userEmailReader.HasRows)
                {
                    while (userEmailReader.Read())
                    {

                        userEmail = userEmailReader.GetString(0);
                    }
                }
                else
                {
                    Console.WriteLine("No data found.");
                }
                //close data reader
                userEmailReader.Close();

                //se deshcide alt reader
                userTelefonReader = sqlUsernameTelefon.ExecuteReader();

                //check if there are records
                if (userTelefonReader.HasRows)
                {
                    while (userTelefonReader.Read())
                    {
                        userTelefon = userTelefonReader.GetString(0);
                    }
                }
                else
                {
                    Console.WriteLine("No data found.");
                }
                //close data reader
                userTelefonReader.Close();
                string cParola = "";
                string cUser = "";
                if(String.Compare(parolaTextBox.Text.ToString(), "")==0)
                    cParola = parolaBox.ToString();
                else 
                    cParola=parolaTextBox.Text.ToString(); 
                cUser=numeUtilizator.Text.ToString();

                if (parola == "" || (userEmail == "" && userTelefon == ""))
                    MessageBox.Show("Username sau parola gresita!!");
                else if(String.Compare(cUser,"admin")==0&&String.Compare(cParola,"admin")==0)
                {
                   // MessageBox.Show(cUser+" "+cParola);
                    AdminMeniu adminMeniu = new AdminMeniu();
                    adminMeniu.Show();
                    this.Close();

                }
                else
                {
               /* MessageBox.Show(cUser + " " + cParola);
                MessageBox.Show(Convert.ToString(getCodClient(cParola)));*/
                    string codClientNou = Convert.ToString(getCodClient(cParola));
                    //MessageBox.Show(Convert.ToString(getCodClient(cParola)));
                    Meniu meniu = new Meniu(codClientNou);
                    meniu.Show();
                    meniu.codClientVechi = Convert.ToString(getCodClient(cParola));
                    this.Close();
                }

                //close connection
                sqlConnection.Close();
            }
            catch (Exception ex)
            {
                //display error message
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
        private void Afisare_parola_Checked(object sender, RoutedEventArgs e)
        {
            parolaTextBox.Text = parolaBox.Password;
            parolaBox.Visibility = Visibility.Collapsed;
            parolaTextBox.Visibility = Visibility.Visible;
        }
        private void Afisare_parola_Unchecked(object sender, RoutedEventArgs e)
        {
            parolaBox.Password = parolaTextBox.Text;
            parolaTextBox.Visibility = Visibility.Collapsed;
            parolaBox.Visibility = Visibility.Visible;
        }

        private void parolaTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            parolaBox.Password=parolaTextBox.Text;
        }
        private void Button_Click_Iesire(object sender,EventArgs e)
        {
            this.Close();
        }
        private void Button_Click_Creare(object sender,EventArgs e)
        {
            CreareCont creareCont = new CreareCont();
            creareCont.Show();
            this.Close();
        }
    }
}
