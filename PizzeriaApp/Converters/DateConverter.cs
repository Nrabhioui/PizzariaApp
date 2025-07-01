using System;
using System.Globalization;
using System.Windows.Data;

namespace PizzeriaApp.Converters
{
    /// <summary>
    /// Convertisseur pour afficher une date dans un format personnalisé.
    /// </summary>
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                // Format par défaut : jour de la semaine, jour mois année, heure:minute
                string format = parameter as string ?? "dddd d MMMM yyyy à HH:mm";
                return date.ToString(format, new CultureInfo("fr-FR"));
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string dateString)
            {
                if (DateTime.TryParse(dateString, new CultureInfo("fr-FR"), DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
            }
            return DateTime.Now;
        }
    }
}
