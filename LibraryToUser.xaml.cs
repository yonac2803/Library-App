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
    /// Interaction logic for LibraryToUser.xaml
    /// </summary>
    public partial class LibraryToUser : Window
    {
        public LibraryToUser()
        {
            InitializeComponent();
            AllItem.ItemsSource = ItemCollection.Instance.GetItems();
            RefreshItems();
        }
        private void RefreshItems()
        {
            AllItem.ItemsSource = ItemCollection.Instance.GetItems();
        }

        private void Display_Click(object sender, RoutedEventArgs e)
        {
            if (AllItem.SelectedItem != null)
            {
                StringBuilder strb = new StringBuilder();
                strb.Append(AllItem.SelectedItem);
                MessageBox.Show(strb.ToString());
            }
            else
            {
                MessageBox.Show("Please choose Book");
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var windowOne = new MainWindow();
            windowOne.Show();
            Close();
        }

        private void ByAuthors(object sender, RoutedEventArgs e)
        {
            AllItem.ItemsSource = ItemCollection.Instance.GetBookByAuthors().ToList();
        }

        private void ByBooks(object sender, RoutedEventArgs e)
        {
            AllItem.ItemsSource = ItemCollection.Instance.GetBooks().ToList();
        }

        private void ByJournal(object sender, RoutedEventArgs e)
        {
            AllItem.ItemsSource = ItemCollection.Instance.GetJournals().ToList();

        }
        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = tbxTitle.Text.ToLower();

            var filteredItems = ItemCollection.Instance.GetItems().Where(item =>
            item.Name.ToLower().Contains(searchText));

            AllItem.ItemsSource = filteredItems;

        }

        private void ByNames(object sender, RoutedEventArgs e)
        {
            AllItem.ItemsSource = ItemCollection.Instance.getByNames().ToList();
        }

        private void ByRealseDate(object sender, RoutedEventArgs e)
        {
            AllItem.ItemsSource = ItemCollection.Instance.getByRealseDate().ToList();
        }

        private void ByRealseDateDescending(object sender, RoutedEventArgs e)
        {
            AllItem.ItemsSource = ItemCollection.Instance.getByRealseDateDescending().ToList();

        }


    }
}
