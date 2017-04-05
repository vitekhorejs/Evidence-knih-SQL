using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Evidence_knih_SQL
{
    public class BookDatabase
    {
        private SQLiteAsyncConnection database;
        public BookDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Book>().Wait();
        }
        public Task<List<Book>> GetItemsAsync()
        {
            return database.Table<Book>().ToListAsync();
        }

        //public Task<Book> GetItemAsync(string ISBN)
        public Task<List<Book>> GetItemAsync(string ISBN)
        {
            //return database.Table<Book>().Where(i => i.ISBN.Contains(ISBN)).FirstOrDefaultAsync();
            return database.Table<Book>().Where(i => i.ISBN.Contains(ISBN)).ToListAsync();
        }

        public Task<int> SaveItemAsync(Book item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }
        public Task<int> DeleteItemAsync(Book item)
        {
            return database.DeleteAsync(item);
        }
    }
}
