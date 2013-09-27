using System;
using System.Data.OleDb;
using Pinglator.VocabConverter.DataAccess;

namespace Pinglator.VocabConverter
{
    public static class MdbDbReader
    {
        public static void ReadVocabDbAndMakeNewWordDb()
        {
            var connection = new OleDbConnection(ConnectionString.MdbDbConnectionString);
            connection.Open();

            var command = new OleDbCommand("Select * from [Entries]", connection);
            var reader = command.ExecuteReader();
            var db = new WordDb(ConnectionString.WordDbConnectionString);

            Console.WriteLine("Reading from mdb vocab database.");

            while (reader != null && reader.Read())
            {
                var word = reader[1] as string;
                db.AddMeaning(word);                
            }

            Console.WriteLine("Vocab shared database created.");
        }
    }
}
