using System;
using System.Data.OleDb;
using Lex.Db;
using Lex.Db.Serialization;

namespace Pinglator.VocabConverter
{
    public static class MdbDbReader
    {
        public static void Read()
        {
            var connection = new OleDbConnection(ConnectionString.MdbDbConnectionString);
            connection.Open();

            var db = new DbInstance(ConnectionString.LexDbConnectionString);
            db.Table<objetc>()
            var command = new OleDbCommand("Select * from [Entries]", connection);
            var reader = command.ExecuteReader();


            while (reader != null && reader.Read())
            {
                Console.WriteLine(reader[1]);
            }
        }
    }
}
