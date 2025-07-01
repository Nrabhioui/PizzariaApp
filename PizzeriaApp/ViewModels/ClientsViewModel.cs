using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PizzeriaApp.Core;
using PizzeriaApp.Services;
using PizzeriaApp.Views;

namespace PizzeriaApp.ViewModels
{
    public class ClientsViewModel : ViewModelBase
    {
        private ObservableCollection<Client> _clients;
        private ObservableCollection<Commande> _commandes;
        private Client _selectedClient;
        private Commande _selectedCommande;
        private Client _newClient;
        private readonly string _dataPath;
        private bool _isEditMode = false;
        private int _editIndex = -1;

        public ClientsViewModel(string dataPath)
        {
            _dataPath = dataPath;
            
            // Initialiser les collections
            Clients = new ObservableCollection<Client>();
            Commandes = new ObservableCollection<Commande>();
            NewClient = new Client();
            
            // Initialiser les commandes
            AddClientCommand = new RelayCommand(AddClient, CanAddClient);
            DeleteClientCommand = new RelayCommand(DeleteClient, CanDeleteClient);
            AddCommandeCommand = new RelayCommand(AddCommande, CanAddCommande);
            DeleteCommandeCommand = new RelayCommand(DeleteCommande, CanDeleteCommande);
            EditClientCommand = new RelayCommand(EditClient, CanEditClient);
            UpdateClientCommand = new RelayCommand(UpdateClient, CanUpdateClient);
            EditCommandeCommand = new RelayCommand(EditCommande, CanEditCommande);
            
            // Charger les clients
            _ = LoadClientsAsync();
        }

        #region Propriétés

        public ObservableCollection<Client> Clients
        {
            get => _clients;
            set
            {
                if (_clients != value)
                {
                    _clients = value;
                    OnPropertyChanged(nameof(Clients));
                }
            }
        }

        public ObservableCollection<Commande> Commandes
        {
            get => _commandes;
            set
            {
                if (_commandes != value)
                {
                    _commandes = value;
                    OnPropertyChanged(nameof(Commandes));
                }
            }
        }

        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                if (_selectedClient != value)
                {
                    _selectedClient = value;
                    OnPropertyChanged(nameof(SelectedClient));
                    
                    // Mettre à jour la liste des commandes
                    UpdateCommandes();
                    
                    // Mettre à jour l'état des commandes
                    (DeleteClientCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (AddCommandeCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (EditClientCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public Commande SelectedCommande
        {
            get => _selectedCommande;
            set
            {
                if (_selectedCommande != value)
                {
                    _selectedCommande = value;
                    OnPropertyChanged(nameof(SelectedCommande));
                    (DeleteCommandeCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (EditCommandeCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public Client NewClient
        {
            get => _newClient;
            set
            {
                if (_newClient != value)
                {
                    _newClient = value;
                    OnPropertyChanged(nameof(NewClient));
                    (AddClientCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (UpdateClientCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                if (_isEditMode != value)
                {
                    _isEditMode = value;
                    OnPropertyChanged(nameof(IsEditMode));
                    (AddClientCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (UpdateClientCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        #endregion

        #region Commandes

        public ICommand AddClientCommand { get; }
        public ICommand DeleteClientCommand { get; }
        public ICommand AddCommandeCommand { get; }
        public ICommand DeleteCommandeCommand { get; }
        public ICommand EditClientCommand { get; }
        public ICommand UpdateClientCommand { get; }
        public ICommand EditCommandeCommand { get; }

        #endregion

        #region Méthodes

        private async Task LoadClientsAsync()
        {
            try
            {
                var dataManager = new DataManager();
                var clients = await dataManager.LoadClientsAsync(_dataPath);
                if (clients != null)
                {
                    Clients.Clear();
                    foreach (var client in clients)
                    {
                        Clients.Add(client);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors du chargement des clients : {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task SaveClientsAsync()
        {
            try
            {
                var dataManager = new DataManager();
                await dataManager.SaveClientsAsync(Clients, _dataPath);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors de la sauvegarde des clients : {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateCommandes()
        {
            Commandes.Clear();
            if (SelectedClient != null && SelectedClient.Commandes != null)
            {
                foreach (var commande in SelectedClient.Commandes)
                {
                    Commandes.Add(commande);
                }
            }
        }

        private void AddClient()
        {
            // Créer une copie du nouveau client
            var client = new Client
            {
                Nom = NewClient.Nom,
                Adresse = NewClient.Adresse,
                Telephone = NewClient.Telephone
            };

            // Ajouter à la liste
            Clients.Add(client);

            // Réinitialiser le nouveau client
            NewClient = new Client();

            // Sauvegarder les modifications
            _ = SaveClientsAsync();
        }

        private bool CanAddClient()
        {
            return !string.IsNullOrWhiteSpace(NewClient.Nom) && 
                   !string.IsNullOrWhiteSpace(NewClient.Adresse) && 
                   !string.IsNullOrWhiteSpace(NewClient.Telephone) && !IsEditMode;
        }

        private void DeleteClient()
        {
            if (SelectedClient == null) return;

            var result = System.Windows.MessageBox.Show($"Êtes-vous sûr de vouloir supprimer le client '{SelectedClient.Nom}' et toutes ses commandes ?",
                "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Clients.Remove(SelectedClient);
                SelectedClient = null;
                Commandes.Clear();
                _ = SaveClientsAsync();
            }
        }

        private bool CanDeleteClient()
        {
            return SelectedClient != null;
        }

        private void AddCommande()
        {
            if (SelectedClient == null) return;
            
            // Créer la fenêtre de commande
            var commandeViewModel = new CommandeViewModel(_dataPath);
            var commandeWindow = new CommandeWindow(commandeViewModel);
            
            // Afficher la fenêtre
            var result = commandeWindow.ShowDialog();
            
            // Si l'utilisateur a validé la commande
            if (result == true)
            {
                // Ajouter la commande au client
                SelectedClient.Commandes.Add(commandeViewModel.Commande);
                
                // Mettre à jour la liste des commandes
                UpdateCommandes();
                
                // Sauvegarder les modifications
                _ = SaveClientsAsync();
            }
        }

        private bool CanAddCommande()
        {
            return SelectedClient != null;
        }

        private void DeleteCommande()
        {
            if (SelectedClient == null || SelectedCommande == null) return;

            var result = System.Windows.MessageBox.Show($"Êtes-vous sûr de vouloir supprimer la commande n°{SelectedCommande.Numero} ?",
                "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Supprimer la commande du client
                SelectedClient.Commandes.Remove(SelectedCommande);
                
                // Mettre à jour la liste des commandes
                UpdateCommandes();
                
                // Sauvegarder les modifications
                _ = SaveClientsAsync();
            }
        }

        private bool CanDeleteCommande()
        {
            return SelectedClient != null && SelectedCommande != null;
        }

        private void EditClient()
        {
            if (SelectedClient == null) return;

            IsEditMode = true;
            _editIndex = Clients.IndexOf(SelectedClient);
            NewClient = new Client
            {
                Nom = SelectedClient.Nom,
                Adresse = SelectedClient.Adresse,
                Telephone = SelectedClient.Telephone
            };
        }

        private bool CanEditClient()
        {
            return SelectedClient != null && !IsEditMode;
        }

        private void UpdateClient()
        {
            if (SelectedClient == null || _editIndex == -1) return;

            Clients[_editIndex] = new Client
            {
                Nom = NewClient.Nom,
                Adresse = NewClient.Adresse,
                Telephone = NewClient.Telephone
            };

            IsEditMode = false;
            _editIndex = -1;
            NewClient = new Client();

            // Sauvegarder les modifications
            _ = SaveClientsAsync();
        }

        private bool CanUpdateClient()
        {
            return IsEditMode && !string.IsNullOrWhiteSpace(NewClient.Nom) && 
                   !string.IsNullOrWhiteSpace(NewClient.Adresse) && 
                   !string.IsNullOrWhiteSpace(NewClient.Telephone);
        }

        private void EditCommande()
        {
            if (SelectedClient == null || SelectedCommande == null) return;
            
            // Créer la fenêtre de commande en mode édition
            var commandeViewModel = new CommandeViewModel(_dataPath, SelectedCommande, SelectedClient);
            var commandeWindow = new CommandeWindow(commandeViewModel);
            
            // Afficher la fenêtre
            var result = commandeWindow.ShowDialog();
            
            // Si l'utilisateur a validé la commande
            if (result == true)
            {
                // Mettre à jour la liste des commandes
                UpdateCommandes();
                
                // Sauvegarder les modifications
                _ = SaveClientsAsync();
            }
        }

        private bool CanEditCommande()
        {
            return SelectedClient != null && SelectedCommande != null;
        }

        #endregion
    }
}
