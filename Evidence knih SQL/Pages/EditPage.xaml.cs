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
        public object obj;
        public int ID;

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
            Book item = new Book();
            item.ID = ID;
            item.Name = Name.Text;
            item.Author = Author.Text;
            item.Pages = Pages.Text;
            item.ISBN = ISBN.Text;
            Database.SaveItemAsync(item);
            MessageBox.Show("Záznam byl aktualizován", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainPage Page1 = new MainPage();
            this.NavigationService.Navigate(new MainPage());
        }
    }
}
