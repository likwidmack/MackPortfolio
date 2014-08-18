using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace MackPortfolio.Extensions
{
    public static class Extensions
    {
        public static string ToUrlString(this string text, string replaceText = "", bool AllowUnderscore = true, bool AllowDash = false)
        {
            if (String.IsNullOrEmpty(text)) return text;

            var str = "A-Za-z0-9";
            text = text.TrimFull();
            if (AllowUnderscore)
            {
                text = text.Replace(" ", "_");
                str += "_";
            }
            if (AllowDash) { str += "-"; }
            var expr = @"[^" + str + "]+";
            return Regex.Replace(text, expr, replaceText);
        }

        public static string ToAlphaNumeric(this string text, bool AllowUnderscore = false)
        {
            if (String.IsNullOrEmpty(text)) return text;
            var expr = (AllowUnderscore) ? @"[^A-Za-z0-9_]+" : @"[^A-Za-z0-9]+";
            return Regex.Replace(text, expr, string.Empty);
        }

        public static int RandomNumber(int max)
        {
            Random rnd = new Random();
            return rnd.Next(max);
        }

        #region Primitive Extensions

        /// <summary>
        /// A shorter string method for String.IsNullOrEmpty with replacement option
        /// </summary>
        /// <param name="text">String variable used</param>
        /// <param name="replace">Optional string replacement if string is empty or null</param>
        /// <param name="extend">Optional string attachment to add to string variable if not null or empty</param>
        /// <returns></returns>
        public static string IsEmpty(this string text, string replace = "", string extend = null)
        {
            var _txt = text;
            if (!String.IsNullOrEmpty(extend)) { _txt += extend; }
            return String.IsNullOrWhiteSpace(text) ? replace : _txt;
        }

        public static string Decode(this string text)
        {
            if (String.IsNullOrWhiteSpace(text)) return text;
            return HttpUtility.HtmlDecode(text);
        }

        public static string TrimFull(this string text)
        {
            if (String.IsNullOrWhiteSpace(text)) return text;
            text = Regex.Replace(text, @"\s+", " ");
            //remove spaces and special Characters
            return text.Trim();
        }

        public static string ReplaceNewLine(this string text)
        {
            if (String.IsNullOrWhiteSpace(text)) return text;
            text = Regex.Replace(text, @"([\r\n]+|\n|\r)/gm", "<br/>");
            //remove spaces and special Characters
            return text.Trim();
        }

        /// <summary>
        /// Nullable intreger to string in currency format
        /// </summary>
        /// <param name="num">number</param>
        /// <param name="dec">decimal count</param>
        /// <returns></returns>
        public static string ToMoney(this int? num, int dec = 0)
        {
            if (!num.HasValue) num = 0;
            return String.Format("{0:C" + dec + "}", num);
        }
        public static string ToMoney(this int num, int dec = 0)
        {
            return String.Format("{0:C" + dec + "}", num);
        }
        #endregion
    }
}
