using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PizzeriaApp.Converters
{
    /// <summary>
    /// Convertisseur pour transformer une valeur booléenne en Visibility.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
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
                        string visibilityValue = boolValue ? parts[0] : parts[1];
                        
                        if (Enum.TryParse<Visibility>(visibilityValue, out Visibility result))
                        {
                            return result;
                        }
                    }
                }
                
                // Valeur par défaut si aucun paramètre n'est spécifié
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                if (parameter is string paramString)
                {
                    string[] parts = paramString.Split('|');
                    if (parts.Length == 2)
                    {
                        if (Enum.TryParse<Visibility>(parts[0], out Visibility visibleValue) && 
                            visibility == visibleValue)
                        {
                            return true;
                        }
                        else if (Enum.TryParse<Visibility>(parts[1], out Visibility collapsedValue) && 
                                 visibility == collapsedValue)
                        {
                            return false;
                        }
                    }
                }
                
                return visibility == Visibility.Visible;
            }
            
            return false;
        }
    }
}
