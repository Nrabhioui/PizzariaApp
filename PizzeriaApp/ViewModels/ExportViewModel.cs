using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using PizzeriaApp.Core;
using PizzeriaApp.Services;
using System.Windows.Forms;

namespace PizzeriaApp.ViewModels
{
    /// <summary>
    /// ViewModel pour la fenêtre d'exportation XML.
    /// </summary>
    public class ExportViewModel : ViewModelBase
    {
        private readonly string _dataPath;
        private Pizza _pizza;
        private string _exportPath;

        public event EventHandler<bool> ExportCompleted;

        public ExportViewModel(string dataPath, Pizza pizza)
        {
            _dataPath = dataPath;
            Pizza = pizza;

            // Définir le chemin d'exportation par défaut dans le dossier Saves
            string savesPath = Path.Combine(dataPath, "Saves");
            if (!Directory.Exists(savesPath))
            {
                Directory.CreateDirectory(savesPath);
            }
            ExportPath = Path.Combine(savesPath, $"{pizza.Nom}.xml");
            
            // Initialiser les commandes
            BrowseCommand = new RelayCommand(Browse);
            ExportCommand = new RelayCommand(Export, CanExport);
        }

        #region Propriétés

        /// <summary>
        /// Pizza à exporter.
        /// </summary>
        public Pizza Pizza
        {
            get => _pizza;
            set
            {
                if (_pizza != value)
                {
                    _pizza = value;
                    OnPropertyChanged(nameof(Pizza));
                }
            }
        }

        /// <summary>
        /// Chemin d'exportation du fichier XML.
        /// </summary>
        public string ExportPath
        {
            get => _exportPath;
            set
            {
                if (_exportPath != value)
                {
                    _exportPath = value;
                    OnPropertyChanged(nameof(ExportPath));
                    (ExportCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion

        #region Commandes

        /// <summary>
        /// Commande pour parcourir et sélectionner le chemin d'exportation.
        /// </summary>
        public ICommand BrowseCommand { get; }

        /// <summary>
        /// Commande pour exporter la pizza en XML.
        /// </summary>
        public ICommand ExportCommand { get; }

        #endregion

        #region Méthodes

        /// <summary>
        /// Ouvre une boîte de dialogue pour sélectionner le fichier d'exportation XML.
        /// </summary>
        private void Browse()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Fichiers XML (*.xml)|*.xml",
                Title = "Exporter la pizza au format XML",
                InitialDirectory = Path.GetDirectoryName(ExportPath),
                FileName = Path.GetFileName(ExportPath)
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportPath = saveFileDialog.FileName;
            }
        }

        /// <summary>
        /// Exporte la pizza au format XML.
        /// </summary>
        private void Export()
        {
            try
            {
                // Créer le dossier si nécessaire
                string directory = Path.GetDirectoryName(ExportPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var dataManager = new DataManager();
                dataManager.ExportPizzaToXml(Pizza, ExportPath);
                
                System.Windows.MessageBox.Show($"La pizza {Pizza.Nom} a été exportée avec succès vers {ExportPath}.", 
                    "Exportation réussie", MessageBoxButton.OK, MessageBoxImage.Information);
                
                ExportCompleted?.Invoke(this, true);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors de l'exportation : {ex.Message}", 
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                
                ExportCompleted?.Invoke(this, false);
            }
        }

        private bool CanExport()
        {
            return !string.IsNullOrWhiteSpace(ExportPath) && Pizza != null;
        }

        #endregion
    }
}
