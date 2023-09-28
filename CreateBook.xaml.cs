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
using Library_Dal2;
using ItemCollection = Library_Dal2.ItemCollection;

namespace LibraryProject_Yonatan
{
    /// <summary>
    /// Interaction logic for CreateBook.xaml
    /// </summary>
    public partial class CreateBook : Window
    {
        public CreateBook()
        {
            InitializeComponent();
        }

        private void PriceTextBox_Input(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsNumericInput(e.Text);
        }

        private void DiscountBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsNumericInput(e.Text);
        }
        private void EditionBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsNumericInput(e.Text);
        }

        private bool IsNumericInput(string input)
        {
            return double.TryParse(input, out _);
        }

        private void AddBookClick(object sender, RoutedEventArgs e)
        {
            string name = "";
            double price = 0;
            double discount = 0;
            int edition = 0;
            DateTime entryDate = DateTime.Today;
            DateTime publicationDate = DateTime.Today;
            string author = "";
            string publisher = "";
            List<Genres> selectedGenres = new List<Genres>();

            try
            {
                if (NameTextBox.Text != string.Empty)
                {
                    name = NameTextBox.Text;
                }
                else
                {
                    throw new NoInputException("Invalid input for: Name", "Name");

                }
                if (PublicationDatePicker.SelectedDate != null)
                {
                    publicationDate = PublicationDatePicker.SelectedDate.Value;

                }
                else
                {
                    throw new NoInputException("Invalid input for: publication Date", "Publication Date");
                }


                if (IsNumericInput(PriceTextBox.Text))
                {
                    price = double.Parse(PriceTextBox.Text);
                }
                else
                {
                    throw new NoInputException("Invalid input for: price.", "Price");
                }

                if (IsNumericInput(discountBox.Text))
                {
                    discount = double.Parse(discountBox.Text);
                }
                else
                {
                    throw new NoInputException("Invalid input for: discount.", "Discount");
                }

                if (IsNumericInput(EditionTextBox.Text))
                {
                    edition = int.Parse(EditionTextBox.Text);
                }
                else
                {
                    throw new NoInputException("Invalid input for: Edition.", "Edition");
                }


                if (AuthorTextBox.Text != string.Empty)
                {
                    author = AuthorTextBox.Text;
                }
                else
                {
                    throw new NoInputException("Invalid input for: Author", "Author");
                }

                if (PublisherTextBox.Text != string.Empty)
                {
                    publisher = PublisherTextBox.Text;
                }
                else
                {
                    throw new NoInputException("Invalid input for: Publisher", "Publisher");
                }

                if (Adv.IsChecked == true) selectedGenres.Add(Genres.Adventure);
                if (Mys.IsChecked == true) selectedGenres.Add(Genres.Mystery);
                if (Rom.IsChecked == true) selectedGenres.Add(Genres.Romance);
                if (Hor.IsChecked == true) selectedGenres.Add(Genres.Horror);
                if (Sfi.IsChecked == true) selectedGenres.Add(Genres.ScienceFiction);
                if (Fan.IsChecked == true) selectedGenres.Add(Genres.Fantasy);
                if (Dra.IsChecked == true) selectedGenres.Add(Genres.Drama);
                if (Nov.IsChecked == true) selectedGenres.Add(Genres.Novel);
                if (Dfi.IsChecked == true) selectedGenres.Add(Genres.Dystopian_Fiction);
                if (Hfa.IsChecked == true) selectedGenres.Add(Genres.High_fantasy);


                Book NewBook = new Book(name, publicationDate, entryDate, price, discount);
                NewBook.Author = author;
                NewBook.Edition = edition;
                NewBook.Publisher = publisher;
                NewBook.Genres = selectedGenres;

                ItemCollection.Instance.LibraryItems.Add(NewBook);

                Close();
            }
            catch (NoInputException ex)
            {
                MessageBox.Show(ex.Message);

                // Set focus on the input control that caused the exception
                switch (ex.InvalidInputField)
                {
                    case "Name":
                        NameTextBox.Focus();
                        break;
                    case "Publication Date":
                        PublicationDatePicker.Focus();
                        break;
                    case "Price":
                        PriceTextBox.Focus();
                        break;
                    case "Discount":
                        discountBox.Focus();
                        break;
                    case "Edition":
                        EditionTextBox.Focus();
                        break;
                    case "Author":
                        AuthorTextBox.Focus();
                        break;
                    case "Publisher":
                        PublisherTextBox.Focus();
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
