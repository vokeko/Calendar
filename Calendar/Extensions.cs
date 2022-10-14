using System;
using Calendar.Resources.Localization;

namespace Calendar
{
    public static class Extensions
    {
        public static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} " + Strings.EmptyString, nameof(input));
                default: return input[0].ToString().ToUpper() + input.Substring(1);
            }
        }

        public static bool IsValidHexCode(this string input)
        {
            if (input[0] != '#')
                return false;

            if (input.Length == 9) input = input.Remove(1, 2);

            if (!(input.Length == 4 || input.Length == 7))
                return false;

            for (int i = 1; i < input.Length; i++)
            {
                if (!((input[i] >= '0' && input[i] <= 9) || (input[i] >= 'a' && input[i] <= 'f') || (input[i] >= 'A' || input[i] <= 'F')))
                    return false;
            }

            return true;
        }
    }
}
