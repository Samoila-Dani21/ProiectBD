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

namespace OnlineShop
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AdminMeniu : Window
    {
        public AdminMeniu()
        {
            InitializeComponent();
        }
        private void Client_Button_Click(Object sender, RoutedEventArgs e)
        {
            
            ClientStatistici clientStatistici = new ClientStatistici();
            clientStatistici.Show();
            this.Close();

        }
        private void Produse_Button_Click(Object sender, RoutedEventArgs e)
        {
            ProduseStatistici produseStatistici = new ProduseStatistici();
            produseStatistici.Show();
            this.Close();
        }
        private void Comenzi_Button_Click(Object sender, RoutedEventArgs e)
        {
            ComenziStatistici comenziStatistici = new ComenziStatistici();
            comenziStatistici.Show();
            this.Close();
        }
    }
}
