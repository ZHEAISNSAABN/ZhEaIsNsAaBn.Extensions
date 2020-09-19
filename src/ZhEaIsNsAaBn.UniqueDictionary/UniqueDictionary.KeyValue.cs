using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using ZhEaIsNsAaBn.Exceptions;

    [Serializable]
    public class UniqueDictionary<Tkey, Tvalue> : Dictionary<Tkey, Tvalue>
    {
        internal object NewInsert = new object();
        public new void Add(Tkey key, Tvalue value)
        {
            lock (NewInsert)
            {
                if (key == null || value == null)
                {
                    throw new UniqueDictionaryException(UniqueDictionaryExceptionType.KeyAndValueCanNotBeNull);
                }

                if (!ContainsKey(key))
                {
                    base.Add(key, value);
                }
                else
                {
                    this[key] = value;
                }
            }
        }
    }
}
