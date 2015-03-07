using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Extensions
{
    public static class GenericExtensions
    {
        private static readonly Dictionary<ValueType, Int32> _toInt32Cache = new Dictionary<ValueType, int>();
        public static int ToInt32<T>(this T value)
            where T : struct
        {
            if (!_toInt32Cache.ContainsKey(value))
                _toInt32Cache[value] = Convert.ToInt32(value);

            return _toInt32Cache[value];
        }
    }
}
