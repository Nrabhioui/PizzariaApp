using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using PizzeriaApp.Core;
using PizzeriaApp.Services;
using PizzeriaApp.Views;

namespace PizzeriaApp.ViewModels
{
    /// <summary>
    /// ViewModel principal pour la fenêtre principale de l'application.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<Pizza> _pizzas;
        private Pizza _selectedPizza;
        private Pizza _newPizza;
        private string _dataPath;
        private bool _isEditMode = false;
        private int _editIndex = -1;

        public MainViewModel()
        {
            // Initialiser les collections
            Pizzas = new ObservableCollection<Pizza>();
            NewPizza = new Pizza();

            // Définir le chemin de données par défaut
            DataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "PizzeriaApp");

            // Créer le répertoire de données s'il n'existe pas
            if (!Directory.Exists(DataPath))
            {
                Directory.CreateDirectory(DataPath);
            }

            // Initialiser les commandes
            AddPizzaCommand = new RelayCommand(AddPizza, CanAddPizza);
            DeletePizzaCommand = new RelayCommand(DeletePizza, CanDeletePizza);
            OpenSettingsCommand = new RelayCommand(OpenSettings);
            OpenClientsCommand = new RelayCommand(OpenClients);
            OpenExportCommand = new RelayCommand(OpenExport, CanExport);
            EditPizzaCommand = new RelayCommand(EditPizza, CanEditPizza);
            UpdatePizzaCommand = new RelayCommand(UpdatePizza, CanUpdatePizza);
            SelectImageCommand = new RelayCommand(SelectImage);

            // Charger les pizzas
            _ = LoadPizzasAsync();
        }

        #region Propriétés

        /// <summary>
        /// Liste des pizzas.
        /// </summary>
        public ObservableCollection<Pizza> Pizzas
        {
            get => _pizzas;
            set
            {
                if (_pizzas != value)
                {
                    _pizzas = value;
                    OnPropertyChanged(nameof(Pizzas));
                }
            }
        }

        /// <summary>
        /// Pizza sélectionnée dans la liste.
        /// </summary>
        public Pizza SelectedPizza
        {
            get => _selectedPizza;
            set
            {
                if (_selectedPizza != value)
                {
                    _selectedPizza = value;
                    OnPropertyChanged(nameof(SelectedPizza));
                    // Mettre à jour l'état des commandes
                    (DeletePizzaCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (OpenExportCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (EditPizzaCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Nouvelle pizza à ajouter.
        /// </summary>
        public Pizza NewPizza
        {
            get => _newPizza;
            set
            {
                if (_newPizza != value)
                {
                    _newPizza = value;
                    OnPropertyChanged(nameof(NewPizza));
                    // Mettre à jour l'état de la commande d'ajout
                    (AddPizzaCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (UpdatePizzaCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Chemin de stockage des données.
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
        /// Indique si on est en mode édition.
        /// </summary>
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                if (_isEditMode != value)
                {
                    _isEditMode = value;
                    OnPropertyChanged(nameof(IsEditMode));
                    (AddPizzaCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (UpdatePizzaCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion

        #region Commandes

        /// <summary>
        /// Commande pour ajouter une pizza.
        /// </summary>
        public ICommand AddPizzaCommand { get; }

        /// <summary>
        /// Commande pour supprimer une pizza.
        /// </summary>
        public ICommand DeletePizzaCommand { get; }

        /// <summary>
        /// Commande pour ouvrir la fenêtre des paramètres.
        /// </summary>
        public ICommand OpenSettingsCommand { get; }

        /// <summary>
        /// Commande pour ouvrir la fenêtre des clients.
        /// </summary>
        public ICommand OpenClientsCommand { get; }

        /// <summary>
        /// Commande pour ouvrir la fenêtre d'exportation XML.
        /// </summary>
        public ICommand OpenExportCommand { get; }

        /// <summary>
        /// Commande pour modifier la pizza sélectionnée.
        /// </summary>
        public ICommand EditPizzaCommand { get; }

        /// <summary>
        /// Commande pour mettre à jour une pizza existante.
        /// </summary>
        public ICommand UpdatePizzaCommand { get; }

        /// <summary>
        /// Commande pour sélectionner une image.
        /// </summary>
        public ICommand SelectImageCommand { get; }

        #endregion

        #region Méthodes

        /// <summary>
        /// Charge les pizzas depuis le fichier JSON.
        /// </summary>
        private async Task LoadPizzasAsync()
        {
            try
            {
                var dataManager = new DataManager();
                var pizzas = await dataManager.LoadPizzasAsync(DataPath);
                if (pizzas != null)
                {
                    Pizzas.Clear();
                    foreach (var pizza in pizzas)
                    {
                        Pizzas.Add(pizza);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors du chargement des pizzas : {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Sauvegarde les pizzas dans un fichier JSON.
        /// </summary>
        private async Task SavePizzasAsync()
        {
            try
            {
                var dataManager = new DataManager();
                await dataManager.SavePizzasAsync(Pizzas, DataPath);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors de la sauvegarde des pizzas : {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Ajoute une nouvelle pizza à la liste.
        /// </summary>
        private void AddPizza()
        {
            // Créer une copie de la nouvelle pizza
            var pizza = new Pizza
            {
                Nom = NewPizza.Nom,
                Prix = NewPizza.Prix,
                Description = NewPizza.Description,
                ImagePath = NewPizza.ImagePath
            };

            // Ajouter à la liste
            Pizzas.Add(pizza);

            // Réinitialiser la nouvelle pizza
            NewPizza = new Pizza();

            // Sauvegarder les modifications
            _ = SavePizzasAsync();
        }

        /// <summary>
        /// Vérifie si une pizza peut être ajoutée.
        /// </summary>
        private bool CanAddPizza()
        {
            return !IsEditMode && !string.IsNullOrWhiteSpace(NewPizza.Nom) && NewPizza.Prix > 0;
        }

        /// <summary>
        /// Supprime la pizza sélectionnée.
        /// </summary>
        private void DeletePizza()
        {
            if (SelectedPizza == null) return;

            var result = System.Windows.MessageBox.Show($"Êtes-vous sûr de vouloir supprimer la pizza '{SelectedPizza.Nom}' ?",
                "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Pizzas.Remove(SelectedPizza);
                SelectedPizza = null;
                _ = SavePizzasAsync();
            }
        }

        /// <summary>
        /// Vérifie si une pizza peut être supprimée.
        /// </summary>
        private bool CanDeletePizza()
        {
            return SelectedPizza != null;
        }

        /// <summary>
        /// Prépare l'édition d'une pizza existante.
        /// </summary>
        private void EditPizza()
        {
            if (SelectedPizza == null) return;

            // Copier les valeurs de la pizza sélectionnée dans NewPizza
            NewPizza = new Pizza
            {
                Nom = SelectedPizza.Nom,
                Prix = SelectedPizza.Prix,
                Description = SelectedPizza.Description,
                ImagePath = SelectedPizza.ImagePath
            };

            // Enregistrer l'index de la pizza en cours d'édition
            _editIndex = Pizzas.IndexOf(SelectedPizza);
            IsEditMode = true;
        }

        /// <summary>
        /// Vérifie si une pizza peut être éditée.
        /// </summary>
        private bool CanEditPizza()
        {
            return SelectedPizza != null && !IsEditMode;
        }

        /// <summary>
        /// Met à jour une pizza existante.
        /// </summary>
        private void UpdatePizza()
        {
            if (_editIndex < 0 || _editIndex >= Pizzas.Count) return;

            // Mettre à jour la pizza
            var pizza = Pizzas[_editIndex];
            pizza.Nom = NewPizza.Nom;
            pizza.Prix = NewPizza.Prix;
            pizza.Description = NewPizza.Description;
            pizza.ImagePath = NewPizza.ImagePath;

            // Réinitialiser
            NewPizza = new Pizza();
            IsEditMode = false;
            _editIndex = -1;

            // Sauvegarder les modifications
            _ = SavePizzasAsync();

            System.Windows.MessageBox.Show("Pizza mise à jour avec succès !", "Mise à jour", 
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Vérifie si une pizza peut être mise à jour.
        /// </summary>
        private bool CanUpdatePizza()
        {
            return IsEditMode && !string.IsNullOrWhiteSpace(NewPizza.Nom) && NewPizza.Prix > 0;
        }

        /// <summary>
        /// Ouvre la fenêtre des paramètres.
        /// </summary>
        private void OpenSettings()
        {
            var settingsWindow = new SettingsWindow(DataPath)
            {
                Owner = System.Windows.Application.Current.MainWindow
            };

            if (settingsWindow.ShowDialog() == true)
            {
                // Recharger les pizzas
                _ = LoadPizzasAsync();
            }
        }

        /// <summary>
        /// Ouvre la fenêtre de gestion des clients.
        /// </summary>
        private void OpenClients()
        {
            var clientsWindow = new ClientsWindow(DataPath)
            {
                Owner = System.Windows.Application.Current.MainWindow
            };

            clientsWindow.ShowDialog();
        }

        /// <summary>
        /// Ouvre la fenêtre d'exportation XML.
        /// </summary>
        private void OpenExport()
        {
            if (SelectedPizza == null) return;

            var exportWindow = new ExportWindow(DataPath, SelectedPizza)
            {
                Owner = System.Windows.Application.Current.MainWindow
            };

            exportWindow.ShowDialog();
        }

        /// <summary>
        /// Vérifie si une pizza peut être exportée.
        /// </summary>
        private bool CanExport()
        {
            return SelectedPizza != null;
        }

        /// <summary>
        /// Ouvre une boîte de dialogue pour sélectionner une image.
        /// </summary>
        private void SelectImage()
        {
            // Créer une boîte de dialogue de sélection de fichier
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Sélectionner une image",
                Filter = "Fichiers image|*.jpg;*.jpeg;*.png;*.gif;*.bmp|Tous les fichiers|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            // Afficher la boîte de dialogue
            if (openFileDialog.ShowDialog() == true)
            {
                // Mettre à jour le chemin de l'image
                NewPizza.ImagePath = openFileDialog.FileName;
            }
        }
        #endregion
    }

    /// <summary>
    /// Classe simple pour implémenter ICommand.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

        public void Execute(object? parameter) => _execute();

        /// <summary>
        /// Force la réévaluation de la condition CanExecute.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
