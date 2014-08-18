using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MackPortfolio.Prototyping
{
    class Extensions
    {
    }

    public static class Config
    {
        private static char _separator = ';';
        public static char Separator { get { return _separator; } set { _separator = value; } }
    }

    /// <summary>
    /// Provide access to random number generator.
    /// </summary>
    public static class RandomNumber
    {
        private static readonly Random _rnd = new Random();

        public static int Next()
        {
            return _rnd.Next();
        }

        public static int Next(int max)
        {
            return _rnd.Next(max);
        }

        public static int Next(int min, int max)
        {
            return _rnd.Next(min, max);
        }
    }

    public static class ArrayExtensions
    {
        /// <summary>
        /// Select a random element from the array.
        /// </summary>
        public static string Random(this string[] list)
        {
            if (list.Count() == 0) throw new InvalidOperationException("Array must contain at least one item");

            return list[RandomNumber.Next(0, list.Length)];
        }

        /// <summary>
        /// Select a random string from the Enumerable list.
        /// </summary>
        public static string Random(this IEnumerable<Func<string>> list)
        {
            if (list.Count() == 0) throw new InvalidOperationException("Enumerable list must contain at least one item");

            return list.ElementAt(RandomNumber.Next(0, list.Count())).Invoke();
        }

        /// <summary>
        /// Select a random string array from the Enumerable list.
        /// </summary>
        public static string[] Random(this IEnumerable<Func<string[]>> list)
        {
            if (list.Count() == 0) throw new InvalidOperationException("Enumerable list must contain at least one item");

            return list.ElementAt(RandomNumber.Next(0, list.Count())).Invoke();
        }
    }

    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Times<T>(this int count, Func<int, T> func)
        {
            for (var i = 0; i < count; i++)
                yield return func.Invoke(i);
        }
    }

    public static class StringExtensions
    {
        private static readonly string[] _alphabet = "a b c d e f g h i j k l m n o p q r s t u v w x y z".Split(' ');
        private static CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
        private static TextInfo textInfo = cultureInfo.TextInfo;

        /// <summary>
        /// Get a string with every occurence of '#' replaced with a random number.
        /// </summary>
        public static string Numerify(this string s)
        {
            return Regex.Replace(s, "#", new MatchEvaluator((m) => RandomNumber.Next(0, 9).ToString()), RegexOptions.Compiled);
        }

        /// <summary>
        /// Get a string with every '?' replaced with a random character from the alphabet.
        /// </summary>
        public static string Letterify(this string s)
        {
            return Regex.Replace(s, @"\?", new MatchEvaluator((m) => _alphabet.Random()), RegexOptions.Compiled);
        }

        public static string AlphanumericOnly(this string s)
        {
            return Regex.Replace(s, @"\W", "");
        }

        /// <summary>
        /// Capitalise the first letter of the given string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Capitalise(this string s, bool eachWord = false)
        {
            if (eachWord)
            {
                return textInfo.ToTitleCase(s);
            }
            return Regex.Replace(s, "^[a-z]", new MatchEvaluator(x => x.Value.ToUpperInvariant()));
        }
    }
}
