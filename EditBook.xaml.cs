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
    /// Interaction logic for EditBook.xaml
    /// </summary>
    public partial class EditBook : Window
    {
        private Book Book { get; set; }
        public EditBook(Book book)
        {
            InitializeComponent();
            Book = book;

            NameTextBox.Text = book.Name;
            AuthorTextBox.Text = book.Author;
            PriceTextBox.Text = book.Price.ToString();
            PublisherTextBox.Text = book.Publisher;
            PublicationDatePicker.Text = book.ReleaseDate.ToString();
            EditionTextBox.Text = book.Edition.ToString();

            foreach (var item in GenreListBox.Children)
            {
                if (item is CheckBox checkBox)
                {
                    if (Enum.TryParse(checkBox.Content.ToString(), out Genres result) && book.Genres.Contains(result))
                    {
                        checkBox.IsChecked = true;
                    }
                }
            }
            discountBox.Text = book.DiscountPercentage.ToString();
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

        private void EditClick(object sender, RoutedEventArgs e)
        {
            Book.Price = 0;
            Book.DiscountPercentage = 0;
            Book.Edition = 0;
            try
            {
                if (NameTextBox.Text != string.Empty)
                {
                    Book.Name = NameTextBox.Text;
                }
                else
                {
                    throw new NoInputException("Invalid input for: Name", "Name");
                }

                if (PublicationDatePicker.SelectedDate != null)
                {
                    Book.ReleaseDate = PublicationDatePicker.SelectedDate.Value;
                }
                else
                {
                    throw new NoInputException("Invalid input for: publication Date", "Publication Date");

                }

                if (IsNumericInput(PriceTextBox.Text))
                {
                    Book.Price = double.Parse(PriceTextBox.Text);
                }
                else
                {
                    throw new NoInputException("Invalid input for: price.", "Price");
                }

                if (IsNumericInput(discountBox.Text))
                {
                    Book.DiscountPercentage = double.Parse(discountBox.Text);
                    Book.ActualPrice = Book.CalcDiscount();
                }
                else
                {
                    throw new NoInputException("Invalid input for: discount.", "Discount");
                }

                if (IsNumericInput(EditionTextBox.Text))
                {
                    Book.Edition = int.Parse(EditionTextBox.Text);
                }
                else
                {
                    throw new NoInputException("Invalid input for: Edition.", "Edition");
                }

                if (AuthorTextBox.Text != string.Empty)
                {
                    Book.Author = AuthorTextBox.Text;
                }
                else
                {
                    throw new NoInputException("Invalid input for: Author", "Author");
                }

                if (PublisherTextBox.Text != string.Empty)
                {
                    Book.Publisher = PublisherTextBox.Text;
                }
                else
                {
                    throw new NoInputException("Invalid input for: Publisher", "Publisher");
                }

                Book.Genres.Clear();
                List<Genres> selectedGenres = new List<Genres>();
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
                Book.Genres = selectedGenres;


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
