using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Lex.Db;
using Pinglator.VocabConverter.Model;

namespace Pinglator.VocabConverter.DataAccess
{
    public class WordDb : DbInstance
    {
        #region Fields
        private DbTable<Word> _words;
        #endregion


        #region Properties
        public static WordDb Instance { get; private set; }
        public DbTable<Word> Words { get { return _words ?? (_words = base.Table<Word>()); } }
        #endregion


        #region .ctor
        static WordDb()
        {
            Instance = new WordDb(ConnectionString.WordDbConnectionString);
        }

        public WordDb(string path)
            : base(path)
        {
            base.Map<Word>().Automap(w => w.KeyWord);
            //.WithIndex("Values", w => w.Values);   
            base.Initialize();
        }
        #endregion

        public void AddMeaning(string word)
        {
            var validWord = this.GetValidWord(word);
            var key = this.GetKeyFromWord(validWord);
            var item = base.LoadByKey<Word, string>(key);
            if (item == null)
            {
                var wordItem = new Word { KeyWord = key, Values = new List<string> { validWord } };
                base.Save(wordItem);
            }
            else
            {
                if (!item.Values.Contains(validWord)) item.Values.Add(validWord);
            }
        }

        private string GetValidWord(string word)
        {
            var output = word.Replace("ة", "ه");
            return output;
        }

        public string GetKeyFromWord(string word)
        {
            var output =
                //word.Replace("ا", "ا")
                //    .Replace("آ", "ا")
                //    .Replace("أ", "ا")
                //    .Replace("إ", "ا")
                //    .Replace("ی", "ی")
                //    .Replace("ئ", "ی")
                //    .Replace("ي", "ی")
                //    .Replace("ء", string.Empty)
                //    .Replace("و", "و")
                //    .Replace("ؤ", "و")
                word.Replace("ا", string.Empty)
                    .Replace("آ", string.Empty)
                    .Replace("أ", string.Empty)
                    .Replace("إ", string.Empty)
                    .Replace("ی", string.Empty)
                    .Replace("ئ", string.Empty)
                    .Replace("ي", string.Empty)
                    .Replace("ء", string.Empty)
                    .Replace("و", string.Empty)
                    .Replace("ؤ", string.Empty)
                    .Replace("‌", string.Empty)
                    .Replace(" ", string.Empty)
                    .Replace("+", string.Empty)
                    .Replace("-", string.Empty)
                    .Replace(((char)8207).ToString(CultureInfo.InvariantCulture), string.Empty)
                    .Replace(((char)8205).ToString(CultureInfo.InvariantCulture), string.Empty)
                    .Replace(((char)1600).ToString(CultureInfo.InvariantCulture), string.Empty)
                    .Replace(((char)1618).ToString(CultureInfo.InvariantCulture), string.Empty)
                    .Replace(((char)1614).ToString(CultureInfo.InvariantCulture), string.Empty)
                    .Replace(((char)1616).ToString(CultureInfo.InvariantCulture), string.Empty)
                    .Replace(((char)1611).ToString(CultureInfo.InvariantCulture), string.Empty);


            var intermediate = string.Empty;
            foreach (var character in output.ToCharArray())
            {
                switch (character)
                {
                    case 'ب':
                        intermediate += "{b}";
                        break;
                    case 'د':
                        intermediate += "{d}";
                        break;
                    case 'ف':
                        intermediate += "{f}";
                        break;
                    case 'ج':
                        intermediate += "{dʒ}";
                        break;
                    case 'ل':
                        intermediate += "{l}";
                        break;
                    case 'م':
                        intermediate += "{m}";
                        break;
                    case 'ن':
                        intermediate += "{n}";
                        break;
                    case 'پ':
                        intermediate += "{p}";
                        break;
                    case 'ر':
                        intermediate += "{c}";
                        break;
                    case 'و':
                        intermediate += "{v}";
                        break;
                    case 'چ':
                        intermediate += "{tʃ}";
                        break;
                    case 'ش':
                        intermediate += "{ʃ}";
                        break;
                    case 'خ':
                        intermediate += "{kh}";
                        break;
                    case 'ژ':
                        intermediate += "{ʒ}";
                        break;
                    case 'ع':
                        intermediate += "{eyn}";
                        break;
                    case 'ك':
                    case 'ک':
                        intermediate += "{k}";
                        break;
                    case 'گ':
                        intermediate += "{g}";
                        break;
                    case 'ه':
                    case 'ح':
                    case 'ة':
                        intermediate += "{h}";
                        break;
                    case 'ت':
                    case 'ط':
                        intermediate += "{t}";
                        break;
                    case 'س':
                    case 'ص':
                    case 'ث':
                        intermediate += "{s}";
                        break;
                    case 'ز':
                    case 'ض':
                    case 'ظ':
                    case 'ذ':
                        intermediate += "{z}";
                        break;
                    case 'ق':
                    case 'غ':
                        intermediate += "{gh}";
                        break;
                    default:
                        Console.WriteLine("Skip char:" + (int)character);
                        break;
                }
            }

            return intermediate;
        }
    }
}
