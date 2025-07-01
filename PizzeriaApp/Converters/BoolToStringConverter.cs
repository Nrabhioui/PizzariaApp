using System;
using System.Globalization;
using System.Windows.Data;

namespace PizzeriaApp.Converters
{
    /// <summary>
    /// Convertisseur pour transformer une valeur booléenne en chaîne de caractères.
    /// </summary>
    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                if (parameter is string paramString)
                {
                    // Le paramètre doit être au format "ValeurSiVrai|ValeurSiFaux"
                    string[] parts = paramString.Split('|');
                    if (parts.Length == 2)
                    {
                        return boolValue ? parts[0] : parts[1];
                    }
                }
                
                // Valeurs par défaut si aucun paramètre n'est spécifié
                return boolValue ? "Oui" : "Non";
            }
            
            return "Non spécifié";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue && parameter is string paramString)
            {
                string[] parts = paramString.Split('|');
                if (parts.Length == 2)
                {
                    if (stringValue.Equals(parts[0], StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    else if (stringValue.Equals(parts[1], StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }
                }
            }
            
            return false;
        }
    }
}
