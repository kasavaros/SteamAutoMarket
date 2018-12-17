﻿namespace SteamAutoMarket.Core
{
    using System.Threading;

    public static class NumberUtils
    {
        public static readonly string DoubleDelimiter =
            Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        public static bool TryParseDouble(string s, out double result)
        {
            if (s.Contains(DoubleDelimiter))
            {
                return double.TryParse(s, out result);
            }

            if (s.Contains(","))
            {
                return double.TryParse(s.Replace(",", DoubleDelimiter), out result);
            }

            if (s.Contains("."))
            {
                return double.TryParse(s.Replace(".", DoubleDelimiter), out result);
            }

            return double.TryParse(s, out result);
        }
    }
}