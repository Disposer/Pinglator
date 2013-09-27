using System;
using System.IO;

namespace Pinglator.VocabConverter
{
    public static class ConnectionString
    {
        public static string SdfDbConnectionString { get; private set; }
        public static string MdbDbConnectionString { get; private set; }
        public static string WordDbConnectionString { get; private set; }


        static ConnectionString()
        {
            SdfDbConnectionString = "Data Source =appdata:/VocabDb/Data_password.sdf;File Mode = read only;Password='6208323';";
            MdbDbConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=./VocabDb/FLEXICON.mdb";
            WordDbConnectionString = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VocabDb\\Vocabulary.lex");
        }
    }
}
