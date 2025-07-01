using System;
using System.Windows.Input;
using Microsoft.Win32;
using PizzeriaApp.Services;
using System.Windows.Forms;

namespace PizzeriaApp.ViewModels
{
    /// <summary>
    /// ViewModel pour la fenêtre des paramètres.
    /// </summary>
    public class SettingsViewModel : ViewModelBase
    {
        private readonly AppParamManager _appParamManager;
        private string _dataPath;
        private string _theme;
        private string _language;

        public SettingsViewModel(string initialDataPath)
        {
            _appParamManager = new AppParamManager();
            DataPath = initialDataPath;
            Theme = _appParamManager.GetTheme();
            Language = _appParamManager.GetLanguage();
            
            // Commandes
            BrowseDataPathCommand = new RelayCommand(BrowseDataPath);
            SaveSettingsCommand = new RelayCommand(SaveSettings);
        }

        #region Propriétés

        /// <summary>
        /// Chemin de sauvegarde des données.
        /// </summary>
        public string DataPath
        {
            get => _dataPath;
            set
            {
                if (_dataPath != value)
                {
                    _dataPath = value;
                    OnPropertyChanged(nameof(DataPath));
                }
            }
        }

        /// <summary>
        /// Thème de l'application.
        /// </summary>
        public string Theme
        {
            get => _theme;
            set
            {
                if (_theme != value)
                {
                    _theme = value;
                    OnPropertyChanged(nameof(Theme));
                }
            }
        }

        /// <summary>
        /// Langue de l'application.
        /// </summary>
        public string Language
        {
            get => _language;
            set
            {
                if (_language != value)
                {
                    _language = value;
                    OnPropertyChanged(nameof(Language));
                }
            }
        }

        /// <summary>
        /// Liste des thèmes disponibles.
        /// </summary>
        public string[] AvailableThemes => new[] { "Light", "Dark" };

        /// <summary>
        /// Liste des langues disponibles.
        /// </summary>
        public string[] AvailableLanguages => new[] { "fr-FR", "en-US" };

        #endregion

        #region Commandes

        /// <summary>
        /// Commande pour parcourir et sélectionner le chemin de sauvegarde des données.
        /// </summary>
        public ICommand BrowseDataPathCommand { get; }

        /// <summary>
        /// Commande pour sauvegarder les paramètres.
        /// </summary>
        public ICommand SaveSettingsCommand { get; }

        #endregion

        #region Méthodes

        /// <summary>
        /// Ouvre une boîte de dialogue pour sélectionner le répertoire de sauvegarde des données.
        /// </summary>
        private void BrowseDataPath()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Sélectionnez le dossier de données";
                dialog.UseDescriptionForTitle = true;
                dialog.SelectedPath = DataPath;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    DataPath = dialog.SelectedPath;
                }
            }
        }

        /// <summary>
        /// Sauvegarde les paramètres dans le registre.
        /// </summary>
        private void SaveSettings()
        {
            _appParamManager.SaveDataPath(DataPath);
            _appParamManager.SaveTheme(Theme);
            _appParamManager.SaveLanguage(Language);
        }

        #endregion
    }
}
