using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pinglator.Core
{
    public class Converter
    {
        #region Constants
        private const string SEPERATOR = ".";
        #endregion


        #region Fields
        private static readonly Dictionary<string, MultipleStateLetter> MultiStateLetter;
        private static readonly Dictionary<string, MultipleStateLetter> MultiPersianLetter;
        private static readonly Dictionary<string, string> SinglePersianLetter;
        #endregion


        #region .ctor
        static Converter()
        {
            MultiStateLetter = new Dictionary<string, MultipleStateLetter>
            {
                {"ck", new MultipleStateLetter("ck", "c.k")},
                {"ch", new MultipleStateLetter("ch", "c.h")},
                {"sh", new MultipleStateLetter("sh", "s.h")},
                {"kh", new MultipleStateLetter("kh", "k.h")},
                {"zh", new MultipleStateLetter("zh", "z.h")},
                {"gh", new MultipleStateLetter("gh", "g.h")},
                {"ph", new MultipleStateLetter("f", "p.h")},
                {"q", new MultipleStateLetter("gh", "k")},
            };

            MultiPersianLetter = new Dictionary<string, MultipleStateLetter>
            {
                {"h", new MultipleStateLetter("ه", "ح")},
                {"t", new MultipleStateLetter("ت", "ط")},
                {"s", new MultipleStateLetter("س", "ث", "ص")},
                {"c", new MultipleStateLetter("س", "ث", "ص", "ک")},
                {"z", new MultipleStateLetter("ز", "ض", "ذ", "ظ")},
                {"q", new MultipleStateLetter("ک", "ق")},
                {"gh", new MultipleStateLetter("غ", "ق")},
                {"x", new MultipleStateLetter("ز", "س")},
                {"eh", new MultipleStateLetter("ه", "")},
            };

            SinglePersianLetter = new Dictionary<string, string>
            {
                {"b", "ب"},
                {"d", "د"},
                {"f", "ف"},
                {"dʒ", "ج"},
                {"l", "ل"},
                {"m", "م"},
                {"n", "ن"},
                {"p", "پ"},
                {"r", "ر"},
                {"v", "و"},
                {"tʃ", "چ"},
                {"ʃ", "ش"},
                {"kh", "خ"},
                {"ʒ", "ژ"},
                {"eyn", "ع"},

                {"k", "ک"},
                {"g", "گ"},

                {"a", "ا"},
                {"A", "آ"},
                {"ye", "ی"},
            };
        }
        #endregion


        private List<string> AddPossibleList(string subInput, List<string> inputList = null)
        {
            if (inputList == null) inputList = new List<string> { string.Empty };
            if (string.IsNullOrEmpty(subInput)) return null;

            var list = new List<string>();

            var currentChar = subInput[0].ToString();
            var nextChar = subInput.Length > 1 ? subInput[1].ToString() : string.Empty;
            var key = currentChar + nextChar;

            var cloneList = new List<string>(inputList);
            if (MultiStateLetter.ContainsKey(currentChar))
            {
                var values = MultiStateLetter[currentChar];
                foreach (var value in values)
                {
                    var tempList = new List<string>(cloneList);

                    for (var listIndex = 0; listIndex < tempList.Count; listIndex++)
                    {
                        var item = cloneList[listIndex] + value + ".";
                        tempList[listIndex] = item;
                    }

                    list.AddRange(tempList);
                }

                var newList = AddPossibleList(subInput.Substring(1), list);
                return newList ?? list;
            }
            if (MultiStateLetter.ContainsKey(key))
            {
                var values = MultiStateLetter[key];
                foreach (var value in values)
                {
                    var tempList = new List<string>(cloneList);

                    for (var listIndex = 0; listIndex < tempList.Count; listIndex++)
                    {
                        var item = cloneList[listIndex] + value + ".";
                        tempList[listIndex] = item;
                    }

                    list.AddRange(tempList);
                }

                var newList = AddPossibleList(subInput.Substring(2), list);
                return newList ?? list;
            }
            else
            {
                list = inputList;
                for (var listIndex = 0; listIndex < list.Count; listIndex++)
                {
                    var item = list[listIndex];
                    item += currentChar + ".";
                    list[listIndex] = item;
                }

                var newList = AddPossibleList(subInput.Substring(1), list);
                return newList ?? list;
            }
        }

        public List<string> GetPossibleWords(string input)
        {
            var list = AddPossibleList(input);

            for (var index = 0; index < list.Count; index++)
            {
                var item = list[index];
                var value = item.Remove(item.Length - 1); // Replcae last '.'
                list[index] = value;
            }

            return list;
        }

        public string GetIntermediateWord(string input)
        {
            var output = string.Empty;
            var array = input.ToCharArray();

            for (var index = 0; index < array.Length; index++)
            {
                var character = array[index];
                output += character;

                if (index == array.Length - 1) break;
                output += SEPERATOR;
            }

            return output;
        }

        public List<string> GetPersianWords(string input)
        {
            var list = new List<string>();

            var seperated = input.Split("{}".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (var part in seperated)
            {
                if (SinglePersianLetter.ContainsKey(part))
                {
                    var value = SinglePersianLetter[part];

                    if (list.Count == 0)
                    {
                        list.Add(value);
                        continue;
                    }

                    for (var index = 0; index < list.Count; index++) list[index] += value;
                    continue;
                }
                if (MultiPersianLetter.ContainsKey(part))
                {
                    var meanings = MultiPersianLetter[part];

                    if (list.Count == 0)
                    {
                        list.AddRange(meanings);
                        continue;
                    }

                    var additionalList = new List<string>();
                    foreach (var item in list)
                    {
                        var item1 = item;
                        additionalList.AddRange(meanings.Select(meaning => item1 + meaning));
                    }

                    list = additionalList;
                }
                else throw new Exception("Forgotten letter: " + part);
            }

            return list;
        }

        public string ConvertIntermediate(string input)
        {
            var inputCharArray = input.ToLower().Split(SEPERATOR.ToCharArray()).ToList();
            var length = inputCharArray.Count;
            var index = 0;
            var builder = new StringBuilder();

            while (index < length)
            {
                var currentChar = inputCharArray[index];
                string validChar;

                switch (currentChar)
                {
                    case "b":
                        validChar = "{b}";
                        break;
                    case "d":
                        validChar = "{d}";
                        break;
                    case "f":
                        validChar = "{f}";
                        break;
                    case "h":
                        validChar = "{h}";
                        break;
                    case "j":
                        validChar = "{dʒ}";
                        break;
                    case "l":
                        validChar = "{l}";
                        break;
                    case "m":
                        validChar = "{m}";
                        break;
                    case "n":
                        validChar = "{n}";
                        break;
                    case "p":
                        validChar = "{p}";
                        break;
                    case "r":
                        validChar = "{r}";
                        break;
                    case "t":
                        validChar = "{t}";
                        break;
                    case "v":
                        validChar = "{v}";
                        break;
                    case "ch":
                        validChar = "{tʃ}";
                        break;
                    case "sh":
                        validChar = "{ʃ}";
                        break;
                    case "kh":
                        validChar = "{kh}";
                        break;
                    case "zh":
                        validChar = "{ʒ}";
                        break;
                    case "gh":
                        validChar = "{gh}";
                        break;
                    case "ph":
                        validChar = "{ph}";
                        break;
                    case "'":
                        validChar = "{eyn}";
                        break;
                    case "q":
                    case "ck":
                    case "k":
                        validChar = "{k}";
                        break;
                    case "c":
                    case "s":
                        validChar = "{s}";
                        break;
                    case "g":
                        validChar = "{g}";
                        break;
                    case "z":
                        validChar = "{z}";
                        break;
                    default:
                        if (currentChar == "a" && index == length - 1) validChar = "{a}";
                        else if (currentChar == "a" && inputCharArray[index + 1] == "a" && index == 0)
                        {
                            validChar = "{A}";
                            index++;
                        }
                        else if (currentChar == "a" && inputCharArray[index + 1] == "a")
                        {
                            validChar = "{a}";
                            index++;
                        }
                        else if (currentChar == "a" && index == 0) validChar = "{a}";
                        else if (currentChar == "a")
                        {
                            index++;
                            continue;
                        }
                        else if (currentChar == "e" && index == length - 1) validChar = "{eh}";
                        else if (currentChar == "e" && inputCharArray[index + 1] == "i")
                        {
                            validChar = "{ye}";
                            index++;
                        }
                        else if (currentChar == "e" && inputCharArray[index + 1] == "e")
                        {
                            validChar = "{ye}" + "{ye}";
                            index++;
                        }
                        else if (currentChar == "e" && inputCharArray[index + 1] != "e" && index == 0) validChar = "{a}";
                        else if (currentChar == "e")
                        {
                            index++;
                            continue;
                        }
                        else if (currentChar == "o" && index == length - 1) validChar = "{v}";
                        else if (currentChar == "o" && inputCharArray[index + 1] == "o" && index == 0)
                        {
                            validChar = "{a}" + "{v}";
                            index++;
                        }
                        else if (currentChar == "o" && inputCharArray[index + 1] == "o")
                        {
                            validChar = "{v}";
                            index++;
                        }
                        else if (currentChar == "o" && inputCharArray[index + 1] == "a")
                        {
                            validChar = "{v}" + "{a}";
                            index++;
                        }
                        else if (currentChar == "o" && inputCharArray[index + 1] == "u")
                        {
                            validChar = "{v}";
                            index++;
                        }
                        else if (currentChar == "o" && index == 0) validChar = "{a}";
                        else if (currentChar == "o")
                        {
                            index++;
                            continue;
                        }
                        else if (currentChar == "i" && index == length - 1) validChar = "{ye}";
                        else if (currentChar == "i" && index == 0) validChar = "{a}" + "{ye}";
                        else
                            switch (currentChar)
                            {
                                case "i":
                                    validChar = "{ye}";
                                    break;
                                case "y":
                                    validChar = "{ye}";
                                    break;
                                default:
                                    if (currentChar == "u" && index == 0)
                                        validChar = "{a}" + "{v}";
                                    else if (currentChar == "u")
                                        validChar = "{v}";
                                    else
                                        validChar = currentChar;
                                    break;
                            }
                        break;
                }
                index = index + 1;

                builder.Append(validChar);
            }

            return builder.ToString();
        }
    }
}