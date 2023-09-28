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
    /// Interaction logic for UserEntry.xaml
    /// </summary>
    public partial class UserEntry : Window
    {
        public UserEntry()
        {
            InitializeComponent();

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            Close();
        }
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            bool entered = false;
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            if (username != null && password != null)
            {
                for (int i = 0; i < ItemCollection.Instance.Librarians.Count; i++)
                {
                    if (username.ToLower() == ItemCollection.Instance.Librarians[i].Librarian_Name.ToLower()
                        && password == ItemCollection.Instance.Librarians[i].Password)
                    {
                        ItemCollection.Instance.Librarians[i].LoggedIn = true;
                        entered = true;
                        var windowone = new LibraryFront();
                        windowone.Show();
                        Close();
                    }
                }
                if (!entered)
                {
                    MessageBox.Show("Wrong details");
                }
            }
            else
            {
            }
        }
    }
}
