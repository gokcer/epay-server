using System;
using System.Collections.Generic;
using System.Linq;

namespace Epay3.Common
{
    public static class CommonExtensions
    {
        public static string NormalizePhoneNumber(this string number, string prefix, int minimumLength)
        {
            string correctNumber = null;
            if (number != null)
            {
                var numbers = number.Where(c => char.IsDigit(c)).ToArray();
                if (numbers.Length >= minimumLength)
                {
                    correctNumber = prefix + new string(numbers.Skip(numbers.Length - minimumLength).ToArray());
                }
            }

            return correctNumber;
        }

        public static string GenerateRandomNumberCode(int digits)
        {
            string code = "";
            var random = new Random();
            while (digits>0)
            {
                code = code + random.Next(0, 9);
                digits--;
            }

            return code;
        }

        public static string Mask(this string original,int unmaskedLength=1, int maskLength = 3, char maskCharacter = '*')
        {
            string mask = new string(maskCharacter,maskLength);
            var strings = original
                .Trim()
                .Split()
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Substring(0,unmaskedLength) + mask);
            return string.Join(" ", strings);
        }

    }
}