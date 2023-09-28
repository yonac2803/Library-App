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
    /// Interaction logic for EditJournal.xaml
    /// </summary>
    public partial class EditJournal : Window
    {
        private Journal Journal { get; set; }
        public EditJournal(Journal journal)
        {
            InitializeComponent();
            Journal = journal;

            NameTextBox.Text = Journal.Name;
            PublisherTextBox.Text = Journal.Publisher;
            PriceTextBox.Text = Journal.Price.ToString();
            PublicationDatePicker.Text = Journal.ReleaseDate.ToString();
            foreach (var item in GenreListBox.Children)
            {
                if (item is CheckBox checkBox)
                {
                    if (Enum.TryParse(checkBox.Content.ToString(), out JournalGenres result) && journal.Genres.Contains(result))
                    {
                        checkBox.IsChecked = true;
                    }
                }
            }
            discountBox.Text = journal.DiscountPercentage.ToString();
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


        private void EditJournalClick(object sender, RoutedEventArgs e)
        {
            Journal.Price = 0;
            Journal.DiscountPercentage = 0;
            try
            {
                if (NameTextBox.Text != string.Empty)
                {
                    Journal.Name = NameTextBox.Text;
                }
                else
                {
                    throw new NoInputException("Invalid input for: Name", "Name");

                }

                if (PublicationDatePicker.SelectedDate != null)
                {
                    Journal.ReleaseDate = PublicationDatePicker.SelectedDate.Value;
                }
                else
                {
                    throw new NoInputException("Invalid input for: publication Date", "Publication Date");

                }

                if (IsNumericInput(PriceTextBox.Text))
                {
                    Journal.Price = double.Parse(PriceTextBox.Text);
                }
                else
                {
                    throw new NoInputException("Invalid input for: price.", "Price");
                }

                if (IsNumericInput(discountBox.Text))
                {
                    Journal.DiscountPercentage = double.Parse(discountBox.Text);
                    Journal.ActualPrice = Journal.CalcDiscount();
                }
                else
                {
                    throw new NoInputException("Invalid input for: discount.", "Discount");
                }

                if (PublisherTextBox.Text != string.Empty)
                {
                    Journal.Publisher = PublisherTextBox.Text;
                }
                else
                {
                    throw new NoInputException("Invalid input for: Publisher", "Publisher");
                }

                Journal.Genres.Clear();
                List<JournalGenres> selectedGenres = new List<JournalGenres>();
                if (Nat.IsChecked == true) selectedGenres.Add(JournalGenres.Nature);
                if (Geo.IsChecked == true) selectedGenres.Add(JournalGenres.Geography);
                if (His.IsChecked == true) selectedGenres.Add(JournalGenres.history);
                if (Sci.IsChecked == true) selectedGenres.Add(JournalGenres.science);
                if (Wcu.IsChecked == true) selectedGenres.Add(JournalGenres.World_Culture);
                if (New.IsChecked == true) selectedGenres.Add(JournalGenres.News);
                if (Vid.IsChecked == true) selectedGenres.Add(JournalGenres.Video_Game);
                Journal.Genres = selectedGenres;

                Frequency frequency = Frequency.None;
                if (Daily.IsChecked == true) frequency = Frequency.Daily;
                else if (Weekly.IsChecked == true) frequency = Frequency.Weekly;
                else if (BiWeekly.IsChecked == true) frequency = Frequency.BiWeekly;
                else if (Monthly.IsChecked == true) frequency = Frequency.Monthly;
                else if (BiMonthly.IsChecked == true) frequency = Frequency.BiMonthly;
                else if (Quarterly.IsChecked == true) frequency = Frequency.Quarterly;
                else if (SemiAnnually.IsChecked == true) frequency = Frequency.SemiAnnually;
                else if (Annually.IsChecked == true) frequency = Frequency.Annually;
                else if (BiAnnually.IsChecked == true) frequency = Frequency.BiAnnually;
                Journal.Frequency = frequency;




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
