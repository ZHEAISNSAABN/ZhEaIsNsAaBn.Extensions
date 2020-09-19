using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class AutoMapAttribute : Attribute
    {
        public string Name { get; }
        public Type Type { get; }
        public String Language { get; }

        public AutoMapAttribute(string name, Type type, string language = null)
        {
            Name = name;
            Type = type;
            Language = language;
        }
    }
}
