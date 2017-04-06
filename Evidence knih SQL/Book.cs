using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Evidence_knih_SQL
{
    public class Book
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        public string Pages { get; set; }

        public override string ToString()
        {
            return Author + " - " + Name + "  " + "  ISBN 978" + ISBN;
        }
    }
}
