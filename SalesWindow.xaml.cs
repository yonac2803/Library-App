using LogicForLibrary.Exceptions;
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
    /// Interaction logic for SalesWindow.xaml
    /// </summary>
    public partial class SalesWindow : Window
    {
        public SalesWindow()
        {
            InitializeComponent();

            DiscountedItemsListView.ItemsSource = ItemCollection.Instance.DiscountedItems.ToList();
            Allitems.ItemsSource = ItemCollection.Instance.GetItems();
        }
        private bool IsNumericInput(string input)
        {
            return double.TryParse(input, out _);
        }
        private void Sale_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtDiscountPercentage == null)
                {
                    throw new ArgumentException("DiscountPercentage Textbox is empty");
                }
                else if (!IsNumericInput(txtDiscountPercentage.Text))
                {
                    throw new NoInputException("Invalid input for: Discount Percentage", "Discount Percentage");
                }
                else
                {
                    if (chkAuthors.IsChecked == true)
                    {
                        if (txtAuthor.Text != null)
                        {
                            bool authorExists = false;
                            foreach (var item in ItemCollection.Instance.LibraryItems)
                            {
                                if (item.Author == txtAuthor.Text)
                                {
                                    authorExists = true;
                                    break;
                                }
                            }
                            if (authorExists)
                            {
                                foreach (var item in ItemCollection.Instance.LibraryItems)
                                {
                                    if (item.Author == txtAuthor.Text)
                                    {
                                        if (item.DiscountPercentage < int.Parse(txtDiscountPercentage.Text))
                                        {
                                            item.DiscountPercentage = int.Parse(txtDiscountPercentage.Text);
                                            item.ActualPrice = item.CalcDiscount();
                                            ItemCollection.Instance.DiscountedItems.Add(item);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                throw new ArgumentException("Author Not Found");
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Author Not Found");
                        }
                    }
                    else if (chkDate.IsChecked == true)
                    {
                        if (dpDate.SelectedDate != null)
                        {
                            foreach (var item in ItemCollection.Instance.LibraryItems)
                            {
                                if (item.ReleaseDate.Equals(dpDate.SelectedDate.Value))
                                {
                                    if (item.DiscountPercentage < int.Parse(txtDiscountPercentage.Text))
                                    {
                                        item.DiscountPercentage = int.Parse(txtDiscountPercentage.Text);
                                        item.ActualPrice = item.CalcDiscount();
                                        ItemCollection.Instance.DiscountedItems.Add(item);

                                    }
                                }
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Date not picked");
                        }
                    }
                    else if (chkPublisher.IsChecked == true)
                    {
                        if (txtPublisher.Text != null)
                        {
                            bool authorExists = false;
                            foreach (var item in ItemCollection.Instance.LibraryItems)
                            {
                                if (item.Publisher == txtPublisher.Text)
                                {
                                    authorExists = true;
                                    break;
                                }
                            }
                            if (authorExists)
                            {
                                foreach (var item in ItemCollection.Instance.LibraryItems)
                                {
                                    if (item.Publisher == txtPublisher.Text)
                                    {
                                        if (item.DiscountPercentage < int.Parse(txtDiscountPercentage.Text))
                                        {
                                            item.DiscountPercentage = int.Parse(txtDiscountPercentage.Text);
                                            item.ActualPrice = item.CalcDiscount();
                                            ItemCollection.Instance.DiscountedItems.Add(item);

                                        }
                                    }
                                }
                            }

                            else
                            {
                                throw new ArgumentException("Publisher Not Found");
                            }
                        }
                        else
                        {
                            throw new ArgumentException("Textbox empty");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("no checkbox were checked");
                    }
                }
                RefreshItems();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (NoInputException ex2)
            {
                MessageBox.Show(ex2.Message);
            }
        }

        private void RefreshItems()
        {
            DiscountedItemsListView.ItemsSource = ItemCollection.Instance.DiscountedItems.ToList();
        }

        private void Remove_disc_Click(object sender, RoutedEventArgs e)
        {
            LibraryItem item = (LibraryItem)DiscountedItemsListView.SelectedItem;
            if (item != null)
            {
                item.DiscountPercentage = 0;
                item.ActualPrice = item.CalcDiscount();
                ItemCollection.Instance.DiscountedItems.Remove(item);
            }
            else
            {
                MessageBox.Show("Item not Selected");
            }
            RefreshItems();
        }

        private void Add_Discount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtDiscountPercentage == null)
                {
                    throw new ArgumentException("DiscountPercentage Textbox is empty");
                }
                else if (!IsNumericInput(txtDiscountPercentage.Text))
                {
                    throw new NoInputException("Invalid input for: Discount Percentage", "Discount Percentage");
                }
                else
                {
                    LibraryItem item = (LibraryItem)Allitems.SelectedItem;
                    bool itemExists = false;
                    foreach (var val in ItemCollection.Instance.DiscountedItems)
                    {
                        if (item.GuidNumber == val.GuidNumber)
                        {
                            if (int.Parse(txtDiscountPercentage.Text) <= val.DiscountPercentage)
                            {
                                MessageBox.Show("The selected item already has a greater discount.");
                                return;
                            }
                            else
                            {
                                item.DiscountPercentage = int.Parse(txtDiscountPercentage.Text);
                                item.ActualPrice = item.CalcDiscount();
                                itemExists = true;
                            }
                        }
                    }
                    if (!itemExists)
                    {
                        item.DiscountPercentage = int.Parse(txtDiscountPercentage.Text);
                        item.ActualPrice = item.CalcDiscount();
                        ItemCollection.Instance.DiscountedItems.Add(item);
                    }
                    RefreshItems();


                }
            }
            catch (ArgumentException ex1)
            {
                MessageBox.Show(ex1.Message);
            }
            catch (NoInputException ex2)
            {
                MessageBox.Show(ex2.Message);
            }
        }
    }
}
