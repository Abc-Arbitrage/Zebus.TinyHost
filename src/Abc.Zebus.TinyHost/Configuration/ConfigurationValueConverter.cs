using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Abc.Zebus.TinyHost.Configuration
{
    public static class ConfigurationValueConverter
    {
        private static readonly string[] _itemSeparators = { ",", ";", Environment.NewLine, "\n" };

        public static object ConvertValue(object value, Type expectedType)
        {
            if (value == null)
                return null;

            if (expectedType.IsInstanceOfType(value))
                return value;

            return ConvertValueString(value.ToString(), expectedType);
        }

        private static object ConvertValueString(string value, Type expectedType)
        {
            var underlyingType = Nullable.GetUnderlyingType(expectedType);

            if (underlyingType != null)
                return string.IsNullOrEmpty(value) ? null : ConvertValue(value, underlyingType);

            if (expectedType == typeof(TimeSpan))
                return TimeSpan.Parse(value, CultureInfo.InvariantCulture);

            if (expectedType == typeof(string[]))
                return SplitItems(value).ToArray();

            if (expectedType == typeof(int[]))
                return SplitItems(value).Select(int.Parse).ToArray();

            if (expectedType == typeof(long[]))
                return SplitItems(value).Select(long.Parse).ToArray();

            if (expectedType.IsEnum)
                return Enum.Parse(expectedType, value);

            if (expectedType == typeof(Guid))
                return Guid.Parse(value);

            return Convert.ChangeType(value, expectedType, CultureInfo.InvariantCulture);
        }

        private static IEnumerable<string> SplitItems(string value)
        {
            return value.Split(_itemSeparators, StringSplitOptions.RemoveEmptyEntries)
                        .Select(str => str.Trim())
                        .Where(str => !string.IsNullOrEmpty(str));
        }
    }
}