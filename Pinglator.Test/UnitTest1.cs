using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pinglator.Test
{
    using Core;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ComplexConvertPinglish()
        {
            var converter = new Converter();
            const string input = "teahraan";

            var possibles = converter.GetPossibleWords(input);
            var output = possibles.Select(converter.ConvertIntermediate).ToList();

            var finalList = new List<string>();
            foreach (var item in output)
                finalList.AddRange(converter.GetPersianWords(item));

            foreach (var item in finalList)
                Debug.WriteLine(item);
        }
    }
}
