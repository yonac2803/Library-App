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
    /// Interaction logic for CreateJournal.xaml
    /// </summary>
    public partial class CreateJournal : Window
    {
        public CreateJournal()
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
        private bool IsNumericInput(string input)
        {
            return double.TryParse(input, out _);
        }

        private void AddJournalClick(object sender, RoutedEventArgs e)
        {
            string name = "";
            double price = 0;
            double discount = 0;
            DateTime publicationDate = DateTime.Today;
            DateTime entryDate = DateTime.Today;
            string publisher = "";
            List<JournalGenres> selectedGenres = new List<JournalGenres>();
            Frequency frequency = Frequency.None;

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
                    if(publicationDate > DateTime.Today)
                    {
                        throw new NoInputException("Invalid input for: Publication Date - DAte bigger then Today", "Publication Date");
                    }
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

                if (PublisherTextBox.Text != string.Empty)
                {
                    publisher = PublisherTextBox.Text;
                }
                else
                {
                    throw new NoInputException("Invalid input for: Publisher", "Publisher");
                }

                if (Nat.IsChecked == true) selectedGenres.Add(JournalGenres.Nature);
                if (Geo.IsChecked == true) selectedGenres.Add(JournalGenres.Geography);
                if (Sci.IsChecked == true) selectedGenres.Add(JournalGenres.science);
                if (His.IsChecked == true) selectedGenres.Add(JournalGenres.history);
                if (Wcu.IsChecked == true) selectedGenres.Add(JournalGenres.World_Culture);
                if (New.IsChecked == true) selectedGenres.Add(JournalGenres.News);
                if (Vid.IsChecked == true) selectedGenres.Add(JournalGenres.Video_Game);

                if (Daily.IsChecked == true) frequency = Frequency.Daily;
                else if (Weekly.IsChecked == true) frequency = Frequency.Weekly;
                else if (BiWeekly.IsChecked == true) frequency = Frequency.BiWeekly;
                else if (Monthly.IsChecked == true) frequency = Frequency.Monthly;
                else if (BiMonthly.IsChecked == true) frequency = Frequency.BiMonthly;
                else if (Quarterly.IsChecked == true) frequency = Frequency.Quarterly;
                else if (SemiAnnually.IsChecked == true) frequency = Frequency.SemiAnnually;
                else if (Annually.IsChecked == true) frequency = Frequency.Annually;
                else if (BiAnnually.IsChecked == true) frequency = Frequency.BiAnnually;


                Journal NewJournal = new Journal(name, publicationDate, entryDate, price, discount);

                NewJournal.EditionsNumber++;
                NewJournal.Genres = selectedGenres;
                NewJournal.Publisher = publisher;
                NewJournal.Frequency = frequency;

                ItemCollection.Instance.AddItem(NewJournal);

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
