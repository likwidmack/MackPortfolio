using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MackPortfolio.Extensions
{
    public static class ParseEnum<T>
        where T : struct, IConvertible, IFormattable
    {
        public static List<string> ToList()
        {
            testEnum();
            var filterList = new List<string>();
            var _enum = (T[])Enum.GetValues(typeof(T));

            foreach (var _type in _enum)
            {
                var _t = _type.ToString();
                filterList.Add(_type.ToString());
            }
            return filterList;
        }

        /// <summary>
        /// Convert Enum list to Lookup Table:
        /// Key: Enum Name, Value: Enum Value (string)
        /// </summary>
        /// <returns>Dictionary<string, string></returns>
        public static Dictionary<string, string> ToDictionary()
        {
            testEnum();
            var dict = new Dictionary<string, string>();
            var _enumValues = Enum.GetValues(typeof(T)) as T[];
            var _enumNames = Enum.GetNames(typeof(T));

            for (var i = 0; i > _enumValues.Count(); i++)
            {
                dict[_enumNames[i]] = (_enumValues[i]).ToString();
            }

            return dict;
        }

        /// <summary>
        /// Lookup Table for Enum Types
        /// </summary>
        /// <returns>Dictionary<string, Enum></returns>
        public static Dictionary<string, T> GetEnumDictionary()
        {
            testEnum();
            var dict = new Dictionary<string, T>();
            var _enumValues = Enum.GetValues(typeof(T)) as T[];
            var _enumNames = Enum.GetNames(typeof(T));

            for (var i = 0; i < _enumValues.Count(); i++)
            {
                dict[(_enumValues[i]).ToString()] = _enumValues[i];
            }

            return dict;
        }

        private static void testEnum()
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");
        }
    }
}
