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
    public partial class Statistici : Window
    {
        SqlConnection sqlConnection;
        public Statistici()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["OnlineShop.Properties.Settings.Evidenta_magazin_onlineConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
        }
        private void ShowTabel(string query)
        {
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlCommand.Parameters.Add("@nume",numeTextBox.Text);
                sqlCommand.Parameters.Add("@numeComplex", numeComplexTextBox.Text);
                // the SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-Objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    DataTable zooTable = new DataTable();
                    sqlDataAdapter.Fill(zooTable);
                    interogari.ItemsSource = zooTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        private void Button_join1(object sender, RoutedEventArgs e)
        {
            string query = "select c.nume,c.prenume from Clienti c " +
" inner join Comenzi a on a.cod_client = c.cod_client " +
" inner join detalii_comanda dc on dc.id_comanda = a.id_comanda " +
" where data_comanda = '01-16-2022'";
            ShowTabel(query);
        }
        private void Button_join2(object sender, RoutedEventArgs e)
        {
            string query = "select c.nume,c.prenume from Clienti c "+
" inner join Comenzi a on a.cod_client = c.cod_client "+
" group by c.nume,c.prenume "+
" having sum(suma) > 50 ";
            ShowTabel(query);
        }
        private void Button_join3(object sender, RoutedEventArgs e)
        {
            string query = "select c.nume,c.telefon from Clienti c" +
                " inner join Adresa a on a.cod_client=c.cod_client" +
                " where a.strada='strada1' ";
            ShowTabel(query);
        }
        private void Button_join4(object sender, RoutedEventArgs e)
        {
            string query = "select c.nume,c.prenume from Clienti c " +
                "inner join Comenzi a on a.cod_client=c.cod_client " +
                "inner join detalii_comanda dc on dc.id_comanda=a.id_comanda " +
                "inner join Produse p on p.cod_produs=dc.cod_produs where nume_produs='minge'";
            ShowTabel(query);
        }
        private void Button_join5(object sender, RoutedEventArgs e)
        {
            string query = "select top 1 p.nume_produs from Produse p " +
                "inner join Tipuri_produse tp on tp.id_tip_produs=p.id_tip_produs " +
                "where tp.tip_produs='haine' order by pret";
            ShowTabel(query);
        }
        private void Button_join6(object sender, RoutedEventArgs e)
        {
            string query = "select a.judet,a.oras,a.strada,a.numar,a.bloc,a.apartament,a.etaj" +
                " from Adresa a inner join Clienti c on a.cod_client=c.cod_client" +
                " where c.nume=@nume";
            ShowTabel(query);
        }
        private void Button_complex1(object sender, RoutedEventArgs e)
        {
            string query = "select c.nume,c.prenume from Clienti c inner join Comenzi a" +
                " on a.cod_client=c.cod_client inner join detalii_comanda dc" +
                " on dc.id_comanda=a.id_comanda where dc.data_comanda in " +
                "(select max(data_comanda) from detalii_comanda)";
            ShowTabel(query);
        }
        private void Button_complex2(object sender, RoutedEventArgs e)
        {
            string query = "select p.nume_produs,tp.tip_produs from Produse p " +
                "inner join Tipuri_produse tp on tp.id_tip_produs=p.id_tip_produs " +
                "where Exists (select min(p.pret),tp.tip_produs from Produse p " +
                "inner join Tipuri_produse tp on tp.id_tip_produs=p.id_tip_produs " +
                "group by tp.tip_produs)";
            ShowTabel(query);
        }
        private void Button_complex3(object sender, RoutedEventArgs e)
        {
            string query = "select year(dc.data_comanda) as Anul,count(dc.data_comanda) as NumarComenzi" +
                "  from detalii_comanda dc group by year(dc.data_comanda) " +
                "having count(dc.data_comanda)=( select top 1 count(dc.data_comanda)" +
                " from detalii_comanda group by data_comanda)";
            ShowTabel(query);
        }
        private void Button_complex4(object sender, RoutedEventArgs e)
        {
            string query = "select sum(c.suma) as SumaTotala " +
                "from ( select c.suma from Comenzi c inner join " +
                "Clienti a on c.cod_client=a.cod_client where a.nume=@numeComplex ) as c";
            ShowTabel(query);
        }
    }
}
