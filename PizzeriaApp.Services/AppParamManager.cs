using System;
using Microsoft.Win32;

namespace PizzeriaApp.Services
{
    /// <summary>
    /// Gère les paramètres de l'application en utilisant le registre Windows.
    /// </summary>
    public class AppParamManager
    {
        private const string AppRegistryKeyPath = @"Software\PizzeriaApp";
        private const string DataPathValueName = "DataPath";
        private const string ThemeValueName = "Theme";
        private const string LanguageValueName = "Language";

        /// <summary>
        /// Obtient ou crée la clé de registre de l'application.
        /// </summary>
        /// <returns>Clé de registre de l'application.</returns>
        private RegistryKey GetOrCreateAppRegistryKey()
        {
            return Registry.CurrentUser.CreateSubKey(AppRegistryKeyPath);
        }

        /// <summary>
        /// Récupère tous les paramètres de l'application.
        /// </summary>
        /// <returns>Objet contenant tous les paramètres.</returns>
        public AppParams GetAppParams()
        {
            return new AppParams
            {
                DataPath = GetDataPath(),
                Theme = GetTheme(),
                Language = GetLanguage()
            };
        }

        /// <summary>
        /// Sauvegarde le chemin de sauvegarde des données dans le registre.
        /// </summary>
        /// <param name="dataPath">Chemin de sauvegarde des données.</param>
        public void SaveDataPath(string dataPath)
        {
            using RegistryKey key = GetOrCreateAppRegistryKey();
            key.SetValue(DataPathValueName, dataPath);
        }

        /// <summary>
        /// Récupère le chemin de sauvegarde des données depuis le registre.
        /// </summary>
        /// <param name="defaultPath">Chemin par défaut si aucun chemin n'est trouvé.</param>
        /// <returns>Chemin de sauvegarde des données.</returns>
        public string GetDataPath(string defaultPath = "")
        {
            using RegistryKey key = GetOrCreateAppRegistryKey();
            return key.GetValue(DataPathValueName, defaultPath)?.ToString() ?? defaultPath;
        }

        /// <summary>
        /// Sauvegarde le thème de l'application dans le registre.
        /// </summary>
        /// <param name="theme">Thème de l'application.</param>
        public void SaveTheme(string theme)
        {
            using RegistryKey key = GetOrCreateAppRegistryKey();
            key.SetValue(ThemeValueName, theme);
        }

        /// <summary>
        /// Récupère le thème de l'application depuis le registre.
        /// </summary>
        /// <param name="defaultTheme">Thème par défaut si aucun thème n'est trouvé.</param>
        /// <returns>Thème de l'application.</returns>
        public string GetTheme(string defaultTheme = "Light")
        {
            using RegistryKey key = GetOrCreateAppRegistryKey();
            return key.GetValue(ThemeValueName, defaultTheme)?.ToString() ?? defaultTheme;
        }

        /// <summary>
        /// Sauvegarde la langue de l'application dans le registre.
        /// </summary>
        /// <param name="language">Langue de l'application.</param>
        public void SaveLanguage(string language)
        {
            using RegistryKey key = GetOrCreateAppRegistryKey();
            key.SetValue(LanguageValueName, language);
        }

        /// <summary>
        /// Récupère la langue de l'application depuis le registre.
        /// </summary>
        /// <param name="defaultLanguage">Langue par défaut si aucune langue n'est trouvée.</param>
        /// <returns>Langue de l'application.</returns>
        public string GetLanguage(string defaultLanguage = "fr-FR")
        {
            using RegistryKey key = GetOrCreateAppRegistryKey();
            return key.GetValue(LanguageValueName, defaultLanguage)?.ToString() ?? defaultLanguage;
        }

        /// <summary>
        /// Supprime tous les paramètres de l'application du registre.
        /// </summary>
        public void ClearAllSettings()
        {
            Registry.CurrentUser.DeleteSubKeyTree(AppRegistryKeyPath, false);
        }
    }

    /// <summary>
    /// Classe contenant tous les paramètres de l'application.
    /// </summary>
    public class AppParams
    {
        /// <summary>
        /// Chemin de sauvegarde des données.
        /// </summary>
        public string? DataPath { get; set; }

        /// <summary>
        /// Thème de l'application.
        /// </summary>
        public string? Theme { get; set; }

        /// <summary>
        /// Langue de l'application.
        /// </summary>
        public string? Language { get; set; }
    }
}
