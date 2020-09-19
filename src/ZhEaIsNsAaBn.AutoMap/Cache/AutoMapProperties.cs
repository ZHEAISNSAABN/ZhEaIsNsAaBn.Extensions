using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    internal class AutoMapProperties
    {
        public UniqueDictionary<string, string> propertiesDictionary { get; set; } = new UniqueDictionary<string, string>();

        public UniqueDictionaryOfList<string, string> LocalizedPropertieDictionary { get; set; } = new UniqueDictionaryOfList<string, string>();
        public string TypeName { get; set; }
    }
}
