using LogicForLibrary.Items;
using LogicForLibrary.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for LendingWindow.xaml
    /// </summary>
    public partial class LendingWindow : Window
    {
        public LendingWindow()
        {
            InitializeComponent();




            availableList.ItemsSource = ItemCollection.Instance.GetItems().Where(item => item.IsBorrowed == false);
            historyLends.ItemsSource = ItemCollection.Instance.LendHistory;
            customerList.ItemsSource = ItemCollection.Instance.Customers;
            currentLends.ItemsSource = ItemCollection.Instance.CurrentlyLend;

        }
      
        private void FullscreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                fullscreenButton.Content = "Exit Fullscreen";
                fullscreenButton.FontSize = 35;
                fullscreenButton.FontWeight = FontWeights.DemiBold;
            }
            else
            {
                WindowState = WindowState.Normal;
                fullscreenButton.Content = "Toggle Fullscreen";
                fullscreenButton.FontSize = 35;
                fullscreenButton.FontWeight = FontWeights.DemiBold;
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var windowone = new LibraryFront();
            windowone.Show();
            Close();
        }

        private void AvailableList(object sender, SelectionChangedEventArgs e)
        {
            LibraryItem selectedItem = availableList.SelectedItem as LibraryItem;
            if (selectedItem != null)
            {
                copyItemId.Text = selectedItem.GuidNumber.ToString();

            }
        }
        private void HistoryLends(object sender, SelectionChangedEventArgs e)
        {
            LibraryItem libraryItem = historyLends.SelectedItem as LibraryItem;
            if (libraryItem != null)
            {
                copyItemId.Text = libraryItem.GuidNumber.ToString();
                copyCustId.Text = libraryItem.LenderId.ToString();
            }
        }
        private void CustomerList(object sender, SelectionChangedEventArgs e)
        {
            Customer selectedCustomer = customerList.SelectedItem as Customer;
            if (selectedCustomer != null)
            {
                copyCustId.Text = selectedCustomer.ID.ToString();
            }
        }

        private void CurrentLends(object sender, SelectionChangedEventArgs e)
        {
            LibraryItem libraryItem = currentLends.SelectedItem as LibraryItem;
            if (libraryItem != null)
            {
                copyItemId.Text = libraryItem.GuidNumber.ToString();
                copyCustId.Text = libraryItem.LenderId.ToString();
            }
        }

        private void Lend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool same = false;
                bool Match = false;
                Guid itemGuid = Guid.Empty;
                if (!Guid.TryParse(ItemId.Text, out itemGuid))
                    throw new ArgumentException("Wrong Guid Number");
                string custId = CustomerId.Text;

                DateTime LendDate;
                if (Take_date.SelectedDate.HasValue) LendDate = Take_date.SelectedDate.Value;
                else throw new ArgumentException("Lend Date is empty");

                DateTime returneDate;
                if (return_date.SelectedDate.HasValue) returneDate = return_date.SelectedDate.Value;
                else throw new ArgumentException("Return Date is empty");

                if (DateTime.Compare(LendDate, returneDate) >= 0)
                {
                    throw new ArgumentException("The return date you chose is the same or earlier than the issue date");
                }

                for (int i = 0; i < ItemCollection.Instance.LibraryItems.Count; i++)
                {
                    if (same) break;
                    Guid item2Guid = Guid.Empty;
                    item2Guid = Guid.Parse(ItemCollection.Instance.LibraryItems[i].GuidNumber.ToString());
                    if (item2Guid.Equals(itemGuid))
                    {
                        for (int j = 0; j < ItemCollection.Instance.Customers.Count; j++)
                        {
                            if (ItemCollection.Instance.Customers[j].ID.ToString() == custId)
                            {
                                var newCust = ItemCollection.Instance.Customers[j];
                                ItemCollection.Instance.LibraryItems[i].IsBorrowed = true;
                                ItemCollection.Instance.LibraryItems[i].LendDate = LendDate;
                                ItemCollection.Instance.LibraryItems[i].ReturnDate = returneDate;
                                ItemCollection.Instance.LibraryItems[i].LenderName = ItemCollection.Instance.Customers[j].Customer_Name;
                                ItemCollection.Instance.LibraryItems[i].LenderId = ItemCollection.Instance.Customers[j].ID;

                                if (DateTime.Today > returneDate) ItemCollection.Instance.LibraryItems[i].IsReturnLate = IsLate.Late;
                                else ItemCollection.Instance.LibraryItems[i].IsReturnLate = IsLate.OnTime;

                                ItemCollection.Instance.LendHistory.Add(ItemCollection.Instance.LibraryItems[i]);
                                ItemCollection.Instance.CurrentlyLend.Add(ItemCollection.Instance.LibraryItems[i]);
                                Match = true;
                                availableList.ItemsSource = ItemCollection.Instance.GetItems().Where(item => item.IsBorrowed == false);
                                historyLends.ItemsSource = ItemCollection.Instance.LendHistory;
                                currentLends.ItemsSource = ItemCollection.Instance.CurrentlyLend;
                                CollectionViewSource.GetDefaultView(historyLends.ItemsSource).Refresh();
                                CollectionViewSource.GetDefaultView(currentLends.ItemsSource).Refresh();

                                same = true;
                                break;
                            }
                        }
                    }

                }
                if (!Match)
                {
                    throw new ArgumentException("No Match Found");
                }
                Take_date.SelectedDate = null;
                return_date.SelectedDate = null;
                ItemId.Text = string.Empty;
                CustomerId.Text = string.Empty;
            }

            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool match = false;
                Guid itemGuid = Guid.Empty;
                if (!Guid.TryParse(ItemId.Text, out itemGuid))
                    throw new ArgumentException("Wrong Guid Number");
                string Id = CustomerId.Text;
                int custId = int.Parse(Id);
                for (int i = 0; i < ItemCollection.Instance.CurrentlyLend.Count; i++)
                {
                    if (ItemCollection.Instance.CurrentlyLend[i].GuidNumber == itemGuid && ItemCollection.Instance.CurrentlyLend[i].LenderId == custId)
                    {
                        if (DateTime.Today > ItemCollection.Instance.CurrentlyLend[i].ReturnDate)
                        {
                            MessageBox.Show("You are Late - 10$ Fine");
                            for (int j = 0; j < ItemCollection.Instance.Customers.Count; j++)
                            {
                                if (ItemCollection.Instance.Customers[j].ID == custId)
                                {
                                    var newCust = ItemCollection.Instance.Customers[j];

                                }
                            }
                        }
                        ItemCollection.Instance.CurrentlyLend[i].IsBorrowed = false;
                        ItemCollection.Instance.CurrentlyLend[i].LendDate = DateTime.MaxValue;
                        ItemCollection.Instance.CurrentlyLend[i].ReturnDate = DateTime.MaxValue;
                        ItemCollection.Instance.CurrentlyLend[i].LenderName = null;
                        ItemCollection.Instance.CurrentlyLend[i].LenderId = null;
                        ItemCollection.Instance.CurrentlyLend[i].IsReturnLate = IsLate.Default;
                        ItemCollection.Instance.CurrentlyLend.RemoveAt(i);

                        currentLends.ItemsSource = ItemCollection.Instance.GetItems().Where(item => item.IsBorrowed == true);
                        availableList.ItemsSource = ItemCollection.Instance.GetItems().Where(item => item.IsBorrowed == false);
                        CollectionViewSource.GetDefaultView(historyLends.ItemsSource).Refresh();
                        CollectionViewSource.GetDefaultView(currentLends.ItemsSource).Refresh();

                        match = true;
                        break;
                    }
                }
                if (match == false) throw new ArgumentException("No Id Match Found");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       
    }
}
