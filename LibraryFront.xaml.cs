using LogicForLibrary.Items;
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
using ItemCollection = Library_Dal2.ItemCollection;

namespace LibraryProject_Yonatan
{
    /// <summary>
    /// Interaction logic for LibraryFront.xaml
    /// </summary>
    public partial class LibraryFront : Window
    {
        public LibraryFront()
        {
            InitializeComponent();

            RefreshItems();
        }
        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            var window = new DailyReports();
            window.Show();
            Close();
        }


        private void RefreshItems()
        {
            lvItems.ItemsSource = ItemCollection.Instance.GetItems().ToList();
        }
        private void AddSale_Click(object sender, RoutedEventArgs e)
        {
            var window = new SalesWindow();
            window.Show();
        }
        private void AddBookClick(object sender, RoutedEventArgs e)
        {
            var windowTwo = new CreateBook();
            windowTwo.ShowDialog();
            RefreshItems();
        }
        private void AddJournalClick(object sender, RoutedEventArgs e)
        {
            var windowTwo = new CreateJournal();
            windowTwo.ShowDialog();
            RefreshItems();

        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var windowOne = new MainWindow();
            windowOne.Show();
            Close();
        }
        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (lvItems.SelectedItem != null)
            {
                LibraryItem selected = (LibraryItem)lvItems.SelectedItem;
                ItemCollection.Instance.RemoveItem(selected);
                RefreshItems();
            }
            else
            {
                MessageBox.Show("Please Choose an Item to Remove");
            }
        }
        private void Disply_Click(object sender, RoutedEventArgs e)
        {
            if (lvItems.SelectedItem != null)
            {
                StringBuilder strb = new StringBuilder();
                strb.Append(lvItems.SelectedItem);
                MessageBox.Show(strb.ToString());
            }
            else
            {
                MessageBox.Show("Please Choose an Item to Disply");
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            if (lvItems.SelectedItem != null)
            {
                Guid guid = Guid.Empty;
                if (lvItems.SelectedItem.GetType() == typeof(Book))
                {
                    var window = new EditBook((Book)lvItems.SelectedItem);
                    window.Show();
                    RefreshItems();
                }
                if (lvItems.SelectedItem.GetType() == typeof(Journal))
                {
                    var window = new EditJournal((Journal)lvItems.SelectedItem);
                    window.Show();
                    RefreshItems();
                }


            }
            else
            {
                MessageBox.Show("Please Choose an Item to Edit");
            }
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            RefreshItems();
        }
        private void Lend_Click(object sender, RoutedEventArgs e)
        {
            var windowOne = new LendingWindow();
            windowOne.Show();
            Close();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = tbxTitle.Text.ToLower();

            var filteredItems = ItemCollection.Instance.GetItems().Where(item =>
            item.Name.ToLower().Contains(searchText));

            lvItems.ItemsSource = filteredItems;
        }
    }
}
