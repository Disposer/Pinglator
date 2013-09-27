using System.Collections.Generic;
using System.Linq;

namespace Pinglator.VocabConverter.Model
{
    public class Word
    {
        public string KeyWord { get; set; }
        public List<string> Values { get; set; }

        public override string ToString()
        {
            var toString = KeyWord + ": ";
            return toString;
            return this.Values.Aggregate(toString, (current, value) => current + value);
        }
    }
}
