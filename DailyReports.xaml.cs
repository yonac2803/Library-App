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
    /// Interaction logic for DailyReports.xaml
    /// </summary>
    public partial class DailyReports : Window
    {
        public DailyReports()
        {
            InitializeComponent();



            Count.Text = ItemCollection.Instance.CurrentlyLend.Count().ToString();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var window = new LibraryFront();
            window.Show();
            Close();
        }
        private void FullscreenButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                fullscreenButton.Content = "Exit Fullscreen";
                fullscreenButton.FontSize = 35;
                fullscreenButton.FontWeight = FontWeights.DemiBold;
                seeall.FontSize = 30;

            }
            else
            {
                WindowState = WindowState.Normal;
                fullscreenButton.Content = "Toggle Fullscreen";
                fullscreenButton.FontSize = 20;
                fullscreenButton.FontWeight = FontWeights.DemiBold;
                seeall.FontSize = 20;

            }
        }
        private void SeeAll_Click(object sender, RoutedEventArgs e)
        {
            lvReports.ItemsSource = ItemCollection.Instance.CurrentlyLend.ToList();
        }
        private void Report_Click(object sender, RoutedEventArgs e)
        {
            if (dpDate.SelectedDate.HasValue)
            {
                if (ItemCollection.Instance.CurrentlyLend.Where(item => item.LendDate.Date == dpDate.SelectedDate.Value).Count() == 0)
                {
                    MessageBox.Show("No Items Were Lend This Day.");
                }
                else
                {
                    lvReports.ItemsSource = ItemCollection.Instance.CurrentlyLend.Where(item => item.LendDate.Date == dpDate.SelectedDate.Value).ToList();
                }
                CountItems.Text = ItemCollection.Instance.CurrentlyLend.Where(item => item.LendDate.Date == dpDate.SelectedDate.Value).Count().ToString();
            }
            else
            {
                MessageBox.Show("Please Enter Date.");
            }

        }

        private void Extended_Info(object sender, RoutedEventArgs e)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Items is Library: ");
            stringBuilder.AppendLine(ItemCollection.Instance.LibraryItems.Count.ToString());
            stringBuilder.AppendLine("Books: ");
            stringBuilder.AppendLine(ItemCollection.Instance.LibraryItems.OfType<Book>().Count().ToString());
            stringBuilder.AppendLine("Journals: ");
            stringBuilder.AppendLine(ItemCollection.Instance.LibraryItems.OfType<Journal>().Count().ToString());
            MessageBox.Show(stringBuilder.ToString());
        }
    }
}
