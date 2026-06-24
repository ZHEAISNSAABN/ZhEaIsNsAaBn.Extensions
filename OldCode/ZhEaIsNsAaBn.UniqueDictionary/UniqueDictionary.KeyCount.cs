using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using ZhEaIsNsAaBn.Exceptions;

    [Serializable]
    public class UniqueDictionary<Tkey> : UniqueDictionary<Tkey, int>
    {
        public void NewAdd(Tkey key, int value = 1)
        {
            if (key == null)
            {
                throw new UniqueDictionaryException(UniqueDictionaryExceptionType.KeyAndValueCanNotBeNull);
            }
            if (!ContainsKey(key))
            {
                Add(key, value);
            }
            else
            {
                this[key] += value;
            }
        }
        public bool Ok(Tkey key)
        {
            if (!ContainsKey(key))
                return false;
            else
            {
                if (this[key] > 0)
                    return true;
                else
                    return false;
            }
        }
    }
}
