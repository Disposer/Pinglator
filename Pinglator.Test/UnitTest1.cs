using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pinglator.VocabConverter.DataAccess;
using Pinglator.VocabConverter.Model;

namespace Pinglator.Test
{
    using Core;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ComplexConvertPinglish()
        {
            // Should run once
            //VocabConverter.MdbDbReader.ReadVocabDbAndMakeNewWordDb();

            var converter = new Converter();
            const string pinglishWord = "acemilan";
            var persianWord = WordDb.Instance.GetKeyFromWord("آثمیلان");

            var possibles = converter.GetPossibleWords(pinglishWord);
            var output = possibles.Select(converter.ConvertIntermediate).ToList();

            var allPersianPossibles = new List<string>();
            foreach (var item in output) allPersianPossibles.AddRange(converter.GetPersianWords(item));

            var normalized = converter.NormalizePossibleWords(possibles);
            var allInVocab = new List<string>();

            foreach (var item in normalized)
            {
                var criteria = item;
                var values = WordDb.Instance.LoadAll<Word>().Where(w => w.KeyWord.Equals(criteria)).Select(w => w.Values).ToList();

                foreach (var value in values)
                    allInVocab.AddRange(value);
            }

            foreach (var result in allInVocab)
            {
                Debug.WriteLine(result);
            }
        }
    }
}
