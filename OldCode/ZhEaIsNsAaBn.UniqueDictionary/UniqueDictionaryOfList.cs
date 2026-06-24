using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using ZhEaIsNsAaBn.Exceptions;

    [Serializable]
    public class UniqueDictionaryOfList<FirstTkey, SecondTkey> : UniqueDictionary<FirstTkey, List<SecondTkey>>
    {
        internal object NewInsert = new object();

        public new void Add(FirstTkey Firstkey, SecondTkey Secondkey)
        {
            lock (NewInsert)
            {
                if (Firstkey == null || Secondkey == null)
                {
                    throw new UniqueDictionaryException(UniqueDictionaryExceptionType.KeyAndValueCanNotBeNull);
                }

                if (!ContainsKey(Firstkey))
                {
                    List<SecondTkey> FirstUniqueDictionary = new List<SecondTkey>();
                    FirstUniqueDictionary.Add(Secondkey);
                    base.Add(Firstkey, FirstUniqueDictionary);
                }
                else
                {
                    this[Firstkey].Add(Secondkey);
                }
            }
        }
    }
}
