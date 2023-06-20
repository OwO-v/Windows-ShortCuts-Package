using System;
using System.Collections;
using System.Windows.Input;

namespace WIN_SHORTCUTS_CL.Structure
{
    internal class ShortCutsPair
    {
        public string Alias { get; set; }
        public ModiKey ModifierKey { get; set; }
        public Key DataKey { get; set; }
        public ScAction Action { get; set; }
        public string Target { get; set; }

        internal ShortCutsPair(string _Alias, ModiKey _ModifierKey, Key _DataKey, ScAction _Action, string _Target) 
        {
            Alias = _Alias; 
            ModifierKey = _ModifierKey; 
            DataKey = _DataKey;
            Action = _Action; 
            Target = _Target; 
        }
    }

    internal class ShortCutsPairList : IEnumerable
    {
        private ShortCutsPair[] pairs;
        
        public ShortCutsPairList(ShortCutsPair[] _pairs)
        {
            pairs = new ShortCutsPair[pairs.Length];
            for(int i = 0; i < _pairs.Length; i++)
            {
                pairs[i] = _pairs[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public ShortCutsPairEnum GetEnumerator()
        {
            return new ShortCutsPairEnum(pairs);
        }
    }

    internal class ShortCutsPairEnum : IEnumerator
    {
        public ShortCutsPair[] pairs;
        int currentIndex = -1;

        public ShortCutsPairEnum(ShortCutsPair[] _pairs)
        {
            pairs = _pairs;
        }

        public bool MoveNext()
        {
            currentIndex++;
            return (currentIndex < pairs.Length);
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public ShortCutsPair Current
        {
            get
            {
                try
                {
                    return pairs[currentIndex];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Reset()
        {
            currentIndex = -1;
        }
    }
}
