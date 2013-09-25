using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Pinglator.Core;

namespace Pinglator.Test.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var converter = new Converter();
            const string input = "teahraan";

            var possibles = converter.GetPossibleWords(input);
            var output = possibles.Select(converter.ConvertIntermediate).ToList();

            var finalList = new List<string>();
            foreach (var item in output)
                finalList.AddRange(converter.GetPersianWords(item));

            foreach (var item in output)
                Debug.WriteLine(item);
        }
    }
}
