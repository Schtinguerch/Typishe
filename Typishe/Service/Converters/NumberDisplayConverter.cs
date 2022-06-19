using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Linq;

using Typishe.Setup;

namespace Typishe.Service
{
    public class NumberDisplayConverter : IValueConverter
    {
        private Func<double, string>[] _numberToDisplayFuncs;
        private Func<string, double>[] _displayToNumberFuncs;

        public object Convert(object value, Type targetType = null, object parameter = null, CultureInfo culture = null)
        {
            double number;
            if (value is double)
            {
                number = (double)value;
            }
            else
            {
                var stringParameter = value.ToString();
                number = ArabicToNumber(stringParameter);
            }

            var selectedDisplay = (int) ApplicationSetup.FunctionalSetup.NumberDisplay;
            return _numberToDisplayFuncs[selectedDisplay](number);
        }

        public object ConvertBack(object value, Type targetType = null, object parameter = null, CultureInfo culture = null)
        {
            var stringNumber = value.ToString();
            return ArabicToNumber(stringNumber);
        }

        public NumberDisplayConverter()
        {
            _numberToDisplayFuncs = new[]
            {
                NumberToArabic,
                NumberToRome,
                NumberToKanji,
            };

            _displayToNumberFuncs = new[]
            {
                ArabicToNumber,
                RomeToNumber,
                KanjiToNumber,
            };
        }

        #region NumberToDisplayConverters
        public string NumberToArabic(double number)
        {
            return number.ToString("N2");
        }

        public string NumberToRome(double number)
        {
            var romanLetters = new[] { "M", "CM", "D","CD","C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
            var numbers = new[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };

            var romanFloatLetters = new[] { "S", "::", "∴", ":", "." };
            var romanFloatNumbers = new[] { 1 / 2d, 1 / 3d, 1 / 4d, 1 / 6d, 1 / 12d };

            var integerPart = (int) number;
            var floatPart = number - integerPart;

            string romanInteger = "", romanFloat = "";
            int i = 0;

            while (integerPart != 0)
            {
                if (integerPart >= numbers[i])
                {
                    integerPart -= numbers[i];
                    romanInteger += romanLetters[i];
                }

                else
                {
                    i += 1;
                }
            }

            i = 0;
            while (floatPart >= romanFloatNumbers[^1])
            {
                if (floatPart >= romanFloatNumbers[i])
                {
                    floatPart -= romanFloatNumbers[i];
                    romanFloat += romanFloatLetters[i];
                }

                else
                {
                    i += 1;
                }
            }

            string result;
            if (string.IsNullOrWhiteSpace(romanInteger))
            {
                result = romanFloat;
            }

            else if (string.IsNullOrEmpty(romanFloat))
            {
                result = romanInteger;
            }

            else
            {
                result = $"{romanInteger} | {romanFloat}";
            }

            return result;
        }

        public string NumberToKanji(double number)
        {
            var decimalsKanji = new[] { "万", "千", "百", "十" };
            var decimals = new[] { 10000, 1000, 100, 10 };

            var digits = new[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };

            var integerPart = (int)number;
            var floatPart = number - integerPart;

            string kanjiInteger = "", kanjiFloat = "";

            for (int i = 0; i < decimalsKanji.Length; i++)
            {
                var decimalCount = integerPart / decimals[i];
                if (decimalCount == 0)
                {
                    continue;
                }

                if (decimalCount == 1)
                {
                    kanjiInteger += decimalsKanji[i];
                }

                else
                {
                    kanjiInteger += digits[decimalCount] + decimalsKanji[i];
                }

                integerPart -= decimals[i] * decimalCount;
            }

            if (number != 0)
            {
                kanjiInteger += digits[integerPart];
            }

            if (floatPart >= 0.01)
            {
                kanjiFloat += "点";

                var digit = (int)(floatPart * 10);
                kanjiFloat += digits[digit];

                digit = (int)(floatPart * 100) % 10;
                kanjiFloat += digits[digit];
            }

            string result = kanjiInteger;
            if (string.IsNullOrEmpty(kanjiInteger))
            {
                result = digits[0];
            }

            result += kanjiFloat;
            return result;
        }
        #endregion

        #region DisplayToNumberConverters
        public double ArabicToNumber(string number)
        {
            var success = double.TryParse(
                number, 
                NumberStyles.Any, 
                CultureInfo.InvariantCulture, 
                out double result);

            return result;
        }

        public double RomeToNumber(string number)
        {
            throw new NotImplementedException();
        }

        public double KanjiToNumber(string number)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
