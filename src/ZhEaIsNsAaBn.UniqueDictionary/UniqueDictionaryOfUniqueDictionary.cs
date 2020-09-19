using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using ZhEaIsNsAaBn.Exceptions;

    [Serializable]
    public class UniqueDictionaryOf<FirstTkey, SecondTkey, Tvalue> : Dictionary<FirstTkey, UniqueDictionary<SecondTkey, Tvalue>>
    {
        internal object NewInsert = new object();

        public new void Add(FirstTkey Firstkey, SecondTkey Secondkey, Tvalue value)
        {
            lock (NewInsert)
            {
                if (Firstkey == null || Secondkey == null || value == null)
                {
                    throw new UniqueDictionaryException(UniqueDictionaryExceptionType.KeyAndValueCanNotBeNull);
                }

                if (!ContainsKey(Firstkey))
                {
                    UniqueDictionary<SecondTkey, Tvalue> FirstUniqueDictionary = new UniqueDictionary<SecondTkey, Tvalue>();
                    FirstUniqueDictionary.Add(Secondkey, value);
                    base.Add(Firstkey, FirstUniqueDictionary);
                }
                else
                {
                    this[Firstkey].Add(Secondkey, value);
                }
            }
        }
    }
}
