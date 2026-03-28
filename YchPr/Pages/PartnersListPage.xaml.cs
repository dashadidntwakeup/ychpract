using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Логика взаимодействия для PartnersListPage.xaml
    /// </summary>
    public partial class PartnersListPage : Page
    {
        private List<Partners> allPartners;
        private ObservableCollection<Partners> partnersCollection; 
        public PartnersListPage()
        {
            InitializeComponent();
            allPartners = Connection.connect.Partners.ToList();
            
            PartnerLW.ItemsSource =
                Connection.connect.Partners.ToList();
            partnersCollection = new ObservableCollection<Partners>(Connection.connect.Partners.ToList());
            PartnerLW.ItemsSource = partnersCollection;
            UpdateListView();
        }
        private void UpdateListView()
        {
            IEnumerable<Partners> result = allPartners;

            // Фильтрация по поиску
            string searchText = SearchBox.Text.Trim();
            if (!string.IsNullOrEmpty(searchText))
            {
                result = result.Where(p =>
                    p.NamePartner?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    p.SurnameDirector?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            // Сортировка
            if (SortCombo.SelectedItem is ComboBoxItem selected)
            {
                string tag = selected.Tag?.ToString();
                switch (tag)
                {
                    case "NameAsc":
                        result = result.OrderBy(p => p.NamePartner);
                        break;
                    case "NameDesc":
                        result = result.OrderByDescending(p => p.NamePartner);
                        break;
                    case "RatingAsc":
                        result = result.OrderBy(p => p.Raiting);
                        break;
                    case "RatingDesc":
                        result = result.OrderByDescending(p => p.Raiting);
                        break;
                    case "DiscountAsc":
                        result = result.OrderBy(p => p.SizeDiscount);
                        break;
                    case "DiscountDesc":
                        result = result.OrderByDescending(p => p.SizeDiscount);
                        break;
                }
            }
            else
            {
                // сортировка по умолчанию
                result = result.OrderBy(p => p.NamePartner);
            }

            PartnerLW.ItemsSource = result.ToList();
        }

        // Обработчик изменения текста поиска
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateListView();
        }

        // Обработчик выбора сортировки
        private void SortCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateListView();
        }
        

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PartnersAddEdit(new Partners()));
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var selPartner = PartnerLW.SelectedItem as Partners;
            if (selPartner != null)
            { NavigationService.Navigate(new PartnersAddEdit(selPartner)); }
            else { MessageBox.Show("Не выбран партнер для редактирования!"); }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedPartner = PartnerLW.SelectedItem as Partners;
            if (selectedPartner == null)
            {
                MessageBox.Show("Выберите партнёра для удаления.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Удалить партнёра \"{selectedPartner.NamePartner}\"?",
                                         "Подтверждение", MessageBoxButton.YesNo,
                                         MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                
                UpdateListView();
                partnersCollection.Remove(selectedPartner);
                MessageBox.Show("Партнёр удалён из списка.", "Успех",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void HistoryBtnClick(object sender, RoutedEventArgs e)
        {
            var selectedPartner = PartnerLW.SelectedItem as Partners;
            if (selectedPartner == null)
            {
                MessageBox.Show("Выберите партнёра для просмотра истории продаж.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            NavigationService.Navigate(new SalesHistoryPage(selectedPartner));
        }
    }
}
