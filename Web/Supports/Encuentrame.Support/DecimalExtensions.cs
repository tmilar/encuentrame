using System.Globalization;

namespace Encuentrame.Support
{
    public static class DecimalExtensions
    {
        public static string AsCurrencyString(this decimal value)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:###,###,##0}", value);
        }

        public static string AsCurrencyString(this decimal value, string currencySymbol)
        {
            return string.Format("{0} {1}", currencySymbol, value.AsCurrencyString());
        }
        public static string ToDisplay(this decimal value)
        {
            return value.ToString(CultureInfo.CurrentCulture);
        }

        public static int AsInt(this decimal value)
        {
            return decimal.ToInt32(value);
        }

    }
}