using System;
using System.Globalization;
using System.Windows.Data;

namespace PizzeriaApp.Converters
{
    /// <summary>
    /// Convertisseur pour afficher un prix avec le symbole €.
    /// </summary>
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal price)
            {
                return string.Format("{0:N2} €", price);
            }
            return "0,00 €";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string priceString)
            {
                // Supprimer le symbole € et les espaces
                priceString = priceString.Replace("€", "").Trim();
                
                // Essayer de convertir en decimal
                if (decimal.TryParse(priceString, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal result))
                {
                    return result;
                }
            }
            return 0m;
        }
    }
}
