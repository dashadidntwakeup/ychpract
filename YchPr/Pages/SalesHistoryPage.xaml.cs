using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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
using YchPr.Model;

namespace YchPr.Pages
{
    /// <summary>
    /// Логика взаимодействия для SalesHistoryPage.xaml
    /// </summary>
    public partial class SalesHistoryPage : Page, INotifyPropertyChanged
    {
        private Partners currentPartner;
        private List<Sales> salesList;
        private decimal totalSales;
        public SalesHistoryPage(Partners partner)
        {
            InitializeComponent();
            currentPartner = partner;
            DataContext = this;
            LoadSalesHistory();
        }
        public string PartnerName => currentPartner?.NamePartner;
        public decimal TotalSales
        {
            get => totalSales;
            set
            {
                totalSales = value;
                OnPropertyChanged(nameof(TotalSales));
            }
        }

        private void LoadSalesHistory()
        {
            try
            {
                if (currentPartner == null) return;

                salesList = new List<Sales>();
                string connectionString = Connection.connect.Database.Connection.ConnectionString;
                string query = @"
            SELECT Id_sale, PartnerId, SaleDate, Amount, ProductName, Quantity 
            FROM PartnerSales 
            WHERE PartnerId = @PartnerId 
            ORDER BY SaleDate DESC";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PartnerId", currentPartner.Id_partner);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                salesList.Add(new Sales
                                {
                                    Id = reader.GetInt32(0),
                                    PartnerId = reader.GetInt32(1),
                                    Date = reader.GetDateTime(2),
                                    Amount = reader.GetDecimal(3),
                                    ProductName = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Quantity = reader.GetInt32(5)
                                });
                            }
                        }
                    }
                }

                TotalSales = salesList.Sum(s => s.Amount);
                OnPropertyChanged(nameof(TotalSales));
                SalesListView.ItemsSource = salesList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки истории продаж:\n{ex.Message}\n\n{ex.StackTrace}",
                                "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
