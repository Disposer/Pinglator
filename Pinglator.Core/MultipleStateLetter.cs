using System.Collections.Generic;

namespace Pinglator.Core
{
    public class MultipleStateLetter : List<string>
    {
        #region Properties
        public List<string> States { get; set; }
        #endregion

        #region .ctor
        public MultipleStateLetter(params string[] states)
        {
            foreach (var state in states) base.Add(state);
        }
        #endregion

        public override string ToString()
        {
            var toString = string.Empty;

            for (var index = 0; index < this.States.Count; index++)
            {
                var state = this.States[index];
                toString += state;

                if (index == this.States.Count - 1) break;
                toString += ", ";
            }

            return toString;
        }
    }
}
