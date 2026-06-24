using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using ZhEaIsNsAaBn.Exceptions;

    [Serializable]
    public class UniqueDictionaryOf<FirstTkey, SecondTkey> : Dictionary<FirstTkey, UniqueDictionary<SecondTkey>>
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
                    UniqueDictionary<SecondTkey> FirstUniqueDictionary = new UniqueDictionary<SecondTkey>();
                    FirstUniqueDictionary.NewAdd(Secondkey);
                    base.Add(Firstkey, FirstUniqueDictionary);
                }
                else
                {
                    this[Firstkey].NewAdd(Secondkey);
                }
            }
        }
    }
}
