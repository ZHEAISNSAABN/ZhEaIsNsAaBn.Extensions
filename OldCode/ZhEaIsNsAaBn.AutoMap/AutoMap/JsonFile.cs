using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.IO;
    using System.Threading.Tasks;

    using Newtonsoft.Json.Linq;

    public partial class AutoMap
    {
        public async Task<T> JsonFile<T>(
            StreamReader JSONFile,
            string language = null,
            Func<JObject, T, object[], T> overrideFunc = null,
            params object[] paramsObject)
            where T : class, new()
        {
            try
            {
                var JsonString = await JSONFile.ReadToEndAsync();

                if (!string.IsNullOrEmpty(JsonString))
                    return await this.JsonString(JsonString,
                               language,
                               overrideFunc,
                               paramsObject);

            }
            catch (Exception e)
            {
            }
            return default;
        }
        public async Task<T> JsonFile<T>(
            string JSONFileName,
            string JSONFilePath,
            string language = null,
            Func<JObject, T, object[], T> overrideFunc = null,
            params object[] paramsObject)
            where T : class, new()
        {


            try
            {
                string JsonString = File.ReadAllText(JSONFilePath + JSONFileName);

                if (!string.IsNullOrEmpty(JsonString))
                    return await this.JsonString(JsonString,
                               language,
                               overrideFunc,
                               paramsObject);

            }
            catch (Exception e)
            {
            }

            return default;
        }
    }
}
