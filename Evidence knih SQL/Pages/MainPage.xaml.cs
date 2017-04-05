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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Evidence_knih_SQL
{
    /// <summary>
    /// Interakční logika pro MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            var itemsFromDb = Database.GetItemsAsync().Result;
            listwiew.ItemsSource = itemsFromDb;
        }

        int x = 0;
        long x2 = 0;

        private static BookDatabase _database;
        public static BookDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    var fileHelper = new FileHelper();
                    _database = new BookDatabase(fileHelper.GetLocalFilePath("Evidence_knih.db3"));
                }
                return _database;
            }
        }

        private void Searching(object sender, RoutedEventArgs e)
        {
            if (SearchText.Text.Length == 0)
            {
                DisplayResults();
            }
            else
            {
                if (SearchText.Text.Length >= 3)
                {
                    if (SearchText.Text.Substring(0, 3) == "978")
                    {
                        string neco = SearchText.Text.Substring(3, SearchText.Text.Length - 3);
                        var itemsFromDb = Database.GetItemAsync(neco).Result;
                        //List<Book> search = new List<Book>();
                        //search.Add(itemsFromDb);
                        listwiew.ItemsSource = itemsFromDb;
                    }
                    else
                    {
                        var itemsFromDb = Database.GetItemAsync(SearchText.Text).Result;
                        //List<Book> search = new List<Book>();
                        //search.Add(itemsFromDb);
                        listwiew.ItemsSource = itemsFromDb;
                    }
                }
                else
                {
                    var itemsFromDb = Database.GetItemAsync(SearchText.Text).Result;
                    //List<Book> search = new List<Book>();
                    //search.Add(itemsFromDb);
                    listwiew.ItemsSource = itemsFromDb;
                }
            }
        }

        private void listViewItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Delete))
            {
                MessageBoxResult result = MessageBox.Show("Smazat záznam?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        ListViewItem item = sender as ListViewItem;
                        object obj = item.Content;
                        Database.DeleteItemAsync(obj as Book);
                        DisplayResults();
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            } else
            {
            }
        }

        private void DisplayResults()
        {
            var itemsFromDb = Database.GetItemsAsync().Result;

            listwiew.ItemsSource = itemsFromDb;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "" || Name.Text == null)
            {
                MessageBox.Show("Zadejte název", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (Name.Text.Any(c => char.IsDigit(c)) == true)
            {
                MessageBox.Show("Neplatné název.", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            else if (Author.Text == "" || Author.Text == null)
            {
                MessageBox.Show("Zadejte jméno.", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (Author.Text.Any(c => char.IsDigit(c)) == true)
            {
                MessageBox.Show("Neplatné jméno.", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            else if (Int64.TryParse(ISBN.Text, out x2) == false || ISBN.Text.Length != 10)
            {
                MessageBox.Show("Neplatné ISBN číslo.", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            else if (Int32.TryParse(Pages.Text, out x) == false)
            {
                MessageBox.Show("Zadejte počet stránek.", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Book item = new Book();
                item.Author = Author.Text;
                item.Name = Name.Text;
                item.ISBN = ISBN.Text;
                item.Pages = Pages.Text;
                Database.SaveItemAsync(item);

                DisplayResults();
            }
        }

        public void listViewItem_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            object obj = item.Content;
            EditPage Page1 = new EditPage();
            this.NavigationService.Navigate(new EditPage(obj as Book));
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            SearchText.Text = "";
        }
    }
}
