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
    /// Interakční logika pro EditPage.xaml
    /// </summary>
    public partial class EditPage : Page
    {
        public Book obj;
        public int ID;
        int x = 0;
        long x2 = 0;

        public EditPage()
        {
            InitializeComponent();
        }

        public EditPage(Book book)
        {
            InitializeComponent();
            obj = book;
            ID = book.ID;
            Name.Text = book.Name;
            Author.Text = book.Author;
            Pages.Text = book.Pages;
            ISBN.Text = book.ISBN;
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Smazat záznam?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    await Database.DeleteItemAsync(obj as Book);
                    MainPage Page1 = new MainPage();
                    this.NavigationService.Navigate(new MainPage());
                    break;
                case MessageBoxResult.No:
                    break;
            }

        }

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

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "" || Name.Text == null)
            {
                MessageBox.Show("Zadejte název knihy", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (Name.Text.Any(c => char.IsDigit(c)) == true)
            {
                MessageBox.Show("Neplatný název knihy.", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            else if (Author.Text == "" || Author.Text == null)
            {
                MessageBox.Show("Zadejte jméno autora.", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (Author.Text.Any(c => char.IsDigit(c)) == true)
            {
                MessageBox.Show("Neplatné jméno autora.", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Information);
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
                if (obj.ISBN == ISBN.Text && obj.Name == Name.Text && obj.Author == Author.Text && obj.Pages == Pages.Text)
                {
                    MessageBox.Show("Nebyla provedena žádná změna", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                } else
                {
                    if (obj.ISBN == ISBN.Text)
                    {
                        Book item = new Book();
                        item.ID = ID;
                        item.Name = Name.Text;
                        item.Author = Author.Text;
                        item.Pages = Pages.Text;
                        item.ISBN = ISBN.Text;
                        Database.SaveItemAsync(item);
                        MessageBox.Show("Záznam byl aktualizován", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        var itemsFromDatabase = Database.GetItemAsync(ISBN.Text).Result;
                        if (itemsFromDatabase == null)
                        {
                            Book item = new Book();
                            item.ID = ID;
                            item.Name = Name.Text;
                            item.Author = Author.Text;
                            item.Pages = Pages.Text;
                            item.ISBN = ISBN.Text;
                            Database.SaveItemAsync(item);
                            MessageBox.Show("Záznam byl aktualizován", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Kniha s tímto ISBN je již v databázi", "Upozornění", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }      
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage Page1 = new MainPage();
            this.NavigationService.Navigate(new MainPage());
        }
    }
}
