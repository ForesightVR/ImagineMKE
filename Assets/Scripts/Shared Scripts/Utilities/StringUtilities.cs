using System;
using System.Text.RegularExpressions;

namespace Foresight.Utilities
{
    public class StringUtilities
    {
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
    }
}
