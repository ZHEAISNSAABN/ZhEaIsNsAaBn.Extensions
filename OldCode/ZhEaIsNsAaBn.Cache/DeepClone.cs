using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.Linq;
    using System.Reflection;

    internal class DeepClone
    {

        public IEnumerable<T> ListEntity<T>(List<T> TList)
            where T : class, new()
        {
            List<T> NewTList = new List<T>();
            foreach (var item in TList)
            {
                var NewItem = Entity(item);

                if (NewItem != null)
                {
                    NewTList.Add(NewItem);
                }
            }

            return NewTList;
        }

        public T Entity<T>(T entity)
            where T : class, new()
        {
            var Properties = typeof(T).GetProperties().Select(Property => Property.Name).ToList();
            T tempObj = (T)Activator.CreateInstance(typeof(T));
            foreach (var Property in Properties)
            {
                try
                {
                    var propertyInfo = tempObj.GetType().GetProperty(Property,
                        BindingFlags.Public | BindingFlags.Instance);

                    if (null != propertyInfo && propertyInfo.CanWrite)
                    {
                        propertyInfo.SetValue(tempObj,
                            Convert.ChangeType(entity.GetType().GetProperty(Property)?.GetValue(entity),
                                GetType(propertyInfo)),
                            null);
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return tempObj;
        }


        public IEnumerable<TValue> ListEntity<TKey, TValue>(IDictionary<TKey, TValue> TDictionary)
            where TValue : class, new()
        {
            List<TValue> NewTList = new List<TValue>();
            foreach (var item in TDictionary)
            {
                NewTList.Add(Entity(item.Value));
            }
            return NewTList;
        }

        public IEnumerable<TValue> ListEntity<TKey, TValue>(IDictionary<TKey, List<TValue>> TDictionary)
            where TValue : class, new()
        {
            List<TValue> NewTList = new List<TValue>();
            foreach (var item in TDictionary)
            {
                NewTList.AddRange(ListEntity(item.Value).ToList());
            }
            return NewTList;
        }


        public Dictionary<TKey, List<TValue>> DictionaryEntity<TKey, TValue>(IDictionary<TKey, List<TValue>> TDictionary)
            where TKey : class
            where TValue : class, new()
        {
            Dictionary<TKey, List<TValue>> NewTDictionary = new Dictionary<TKey, List<TValue>>();
            foreach (var item in TDictionary)
            {
                NewTDictionary.Add(item.Key, ListEntity(item.Value).ToList());
            }
            return NewTDictionary;
        }
        public Dictionary<TKey, TValue> DictionaryEntity<TKey, TValue>(IDictionary<TKey, TValue> TDictionary)
            where TKey : class
            where TValue : class, new()
        {
            Dictionary<TKey, TValue> NewTDictionary = new Dictionary<TKey, TValue>();
            foreach (var item in TDictionary)
            {
                NewTDictionary.Add(item.Key, Entity(item.Value));
            }
            return NewTDictionary;
        }


        private Type GetType(PropertyInfo PropertyInfo)
        {
        var t = PropertyInfo.PropertyType;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
        {
            return Nullable.GetUnderlyingType(t);
        }

        return PropertyInfo.PropertyType;
        }
    }
}
