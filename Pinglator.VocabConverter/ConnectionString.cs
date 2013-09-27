namespace Pinglator.VocabConverter
{
    public static class ConnectionString
    {
        public static string SdfDbConnectionString { get; private set; }
        public static string MdbDbConnectionString { get; private set; }
        public static string LexDbConnectionString { get; private set; }


        static ConnectionString()
        {
            SdfDbConnectionString = "Data Source =appdata:/VocabDb/Data_password.sdf;File Mode = read only;Password='6208323';";
            MdbDbConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=./VocabDb/FLEXICON.mdb";
            LexDbConnectionString = @".\VocabDb\Vocabualry.lex";
        }
    }
}
