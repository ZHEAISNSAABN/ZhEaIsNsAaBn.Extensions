using System;
using System.Collections.Generic;
using System.Text;

namespace ZhEaIsNsAaBn.Extensions
{
    using System.Linq;

    internal class PrimaryKeyHandler
    {
        public string GetTheKey<T>(
            T TObj,
            string PropertyName)
        {
            try
            {

                return Convert.ToString(
                    TObj.GetType().GetProperties().ToList().Find(PI => PI.Name == PropertyName).GetValue(TObj));

            }
            catch (Exception e)
            {
            }

            return "";
        }
    }
}
