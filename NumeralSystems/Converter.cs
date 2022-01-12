using System;
using System.Globalization;

namespace NumeralSystems
{
    /// <summary>
    /// Converts a string representations of a numbers to its integer equivalent.
    /// </summary>
    public static class Converter
    {
        private static readonly char[] ArrayOfHexNumbers = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        /// <summary>
        /// Converts the string representation of a positive number in the octal numeral system to its 32-bit signed integer equivalent.
        /// </summary>
        /// <param name="source">The string representation of a positive number in the octal numeral system.</param>
        /// <returns>A positive decimal value.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if source string presents a negative number
        /// - or
        /// contains invalid symbols (non-octal alphabetic characters).
        /// Valid octal alphabetic characters: 0,1,2,3,4,5,6,7.
        /// </exception>
        public static int ParsePositiveFromOctal(this string source) => source.ParsePositiveByRadix(8);

        /// <summary>
        /// Converts the string representation of a positive number in the decimal numeral system to its 32-bit signed integer equivalent.
        /// </summary>
        /// <param name="source">The string representation of a positive number in the decimal numeral system.</param>
        /// <returns>A positive decimal value.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if source string presents a negative number
        /// - or
        /// contains invalid symbols (non-decimal alphabetic characters).
        /// Valid decimal alphabetic characters: 0,1,2,3,4,5,6,7,8,9.
        /// </exception>
        public static int ParsePositiveFromDecimal(this string source) => source.ParsePositiveByRadix(10);

        /// <summary>
        /// Converts the string representation of a positive number in the hex numeral system to its 32-bit signed integer equivalent.
        /// </summary>
        /// <param name="source">The string representation of a positive number in the hex numeral system.</param>
        /// <returns>A positive decimal value.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if source string presents a negative number
        /// - or
        /// contains invalid symbols (non-hex alphabetic characters).
        /// Valid hex alphabetic characters: 0,1,2,3,4,5,6,7,8,9,A(or a),B(or b),C(or c),D(or d),E(or e),F(or f).
        /// </exception>
        public static int ParsePositiveFromHex(this string source) => source.ParsePositiveByRadix(16);

        /// <summary>
        /// Converts the string representation of a positive number in the octal, decimal or hex numeral system to its 32-bit signed integer equivalent.
        /// </summary>
        /// <param name="source">The string representation of a positive number in the the octal, decimal or hex numeral system.</param>
        /// <param name="radix">The radix.</param>
        /// <returns>A positive decimal value.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if source string presents a negative number
        /// - or
        /// contains invalid for given numeral system symbols
        /// -or-
        /// the radix is not equal 8, 10 or 16.
        /// </exception>
        public static int ParsePositiveByRadix(this string source, int radix)
        {
            int parsePositiveByRadix = source.ParseByRadix(radix);
            if (parsePositiveByRadix < 0)
            {
                throw new ArgumentException($"{nameof(source)} does not represent a positive number in numeral system.");
            }

            return parsePositiveByRadix;
        }

        /// <summary>
        /// Converts the string representation of a signed number in the octal, decimal or hex numeral system to its 32-bit signed integer equivalent.
        /// </summary>
        /// <param name="source">The string representation of a signed number in the the octal, decimal or hex numeral system.</param>
        /// <param name="radix">The radix.</param>
        /// <returns>A signed decimal value.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if source contains invalid for given numeral system symbols
        /// -or-
        /// the radix is not equal 8, 10 or 16.
        /// </exception>
        public static int ParseByRadix(this string source, int radix)
        {
            if (source is null)
            {
                throw new ArgumentException("source value is null.", nameof(source));
            }

            if (radix != 8 && radix != 10 && radix != 16)
            {
                throw new ArgumentException($"{nameof(radix)} is 8, 10 and 16 only.");
            }

            source = source.ToUpper(CultureInfo.InvariantCulture);
            int len = source.Length;
            uint parseByRadix = 0;
            int decSign = 1;
            if (source[0] == '-' && radix == 10)
            {
                decSign = -1;
                len--;
            }

            while (len > 0)
            {
                if (radix == 8 && Array.IndexOf(ArrayOfHexNumbers[..8], source[^len]) == -1)
                {
                    throw new ArgumentException($"{nameof(source)} does not represent a signed number in the octal, decimal or hex numeral system.");
                }
                else if (radix == 10 && Array.IndexOf(ArrayOfHexNumbers[..10], source[^len]) == -1)
                {
                    throw new ArgumentException($"{nameof(source)} does not represent a signed number in the octal, decimal or hex numeral system.");
                }
                else if (radix == 16 && Array.IndexOf(ArrayOfHexNumbers, source[^len]) == -1)
                {
                    throw new ArgumentException($"{nameof(source)} does not represent a signed number in the octal, decimal or hex numeral system.");
                }

                int dijit = source[^len];
                dijit = (dijit < 58) ? dijit - 48 : dijit - 55;
                parseByRadix += (uint)(dijit * Math.Pow(radix, len-- - 1));
            }

            parseByRadix -= (uint)(Math.Pow(2, 32) * (parseByRadix / int.MaxValue));

            return decSign * (int)parseByRadix;
        }

        /// <summary>
        /// Converts the string representation of a positive number in the octal numeral system to its 32-bit signed integer equivalent.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="source">The string representation of a positive number in the octal numeral system.</param>
        /// <param name="value">A positive decimal value.</param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        public static bool TryParsePositiveFromOctal(this string source, out int value) => source.TryParsePositiveByRadix(8, out value);

        /// <summary>
        /// Converts the string representation of a positive number in the decimal numeral system to its 32-bit signed integer equivalent.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="source">The string representation of a positive number in the decimal numeral system.</param>
        /// <returns>A positive decimal value.</returns>
        /// <param name="value"> Positive decimal value.</param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        public static bool TryParsePositiveFromDecimal(this string source, out int value) => source.TryParsePositiveByRadix(10, out value);

        /// <summary>
        /// Converts the string representation of a positive number in the hex numeral system to its 32-bit signed integer equivalent.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="source">The string representation of a positive number in the hex numeral system.</param>
        /// <returns>A positive decimal value.</returns>
        /// <param name="value">Positive decimal value.</param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        public static bool TryParsePositiveFromHex(this string source, out int value) => source.TryParsePositiveByRadix(16, out value);

        /// <summary>
        /// Converts the string representation of a positive number in the octal, decimal or hex numeral system to its 32-bit signed integer equivalent.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="source">The string representation of a positive number in the the octal, decimal or hex numeral system.</param>
        /// <param name="radix">The radix.</param>
        /// <returns>A positive decimal value.</returns>
        /// <param name="value">Positive decimal value.</param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        /// <exception cref="ArgumentException">Thrown the radix is not equal 8, 10 or 16.</exception>
        public static bool TryParsePositiveByRadix(this string source, int radix, out int value)
        {
            bool isParsePositiveByRadix = true;
            try
            {
                value = source.ParsePositiveByRadix(radix);
            }
            catch (ArgumentException e) when (e.Message == $"{nameof(radix)} is 8, 10 and 16 only.")
            {
                throw new ArgumentException($"{nameof(radix)} is 8, 10 and 16 only.");
            }
            catch (ArgumentException)
            {
                value = 0;
                isParsePositiveByRadix = false;
            }

            return isParsePositiveByRadix;
        }

        /// <summary>
        /// Converts the string representation of a signed number in the octal, decimal or hex numeral system to its 32-bit signed integer equivalent.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="source">The string representation of a signed number in the the octal, decimal or hex numeral system.</param>
        /// <param name="radix">The radix.</param>
        /// <returns>A positive decimal value.</returns>
        /// <param name="value">Positive decimal value.</param>
        /// <returns>true if s was converted successfully; otherwise, false.</returns>
        /// <exception cref="ArgumentException">Thrown the radix is not equal 8, 10 or 16.</exception>
        public static bool TryParseByRadix(this string source, int radix, out int value)
        {
            bool isParseByRadix = true;
            try
            {
                value = source.ParseByRadix(radix);
            }
            catch (ArgumentException e) when (e.Message == $"{nameof(radix)} is 8, 10 and 16 only.")
            {
                throw new ArgumentException($"{nameof(radix)} is 8, 10 and 16 only.");
            }
            catch (ArgumentException)
            {
                value = 0;
                isParseByRadix = false;
            }

            return isParseByRadix;
        }
    }
}
