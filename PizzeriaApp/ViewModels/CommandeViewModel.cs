using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PizzeriaApp.Core;
using PizzeriaApp.Services;

namespace PizzeriaApp.ViewModels
{
    public class CommandeViewModel : ViewModelBase
    {
        private ObservableCollection<Pizza> _disponiblePizzas;
        private ObservableCollection<Pizza> _commandePizzas;
        private Commande _commande;
        private Pizza _selectedPizza;
        private Pizza _selectedCommandePizza;
        private readonly string _dataPath;
        private decimal _prixTotal;
        private bool _isEditMode;
        private Client _client;

        public CommandeViewModel(string dataPath)
        {
            _dataPath = dataPath;
            
            // Initialiser les collections
            DisponiblePizzas = new ObservableCollection<Pizza>();
            CommandePizzas = new ObservableCollection<Pizza>();
            
            // Créer une nouvelle commande
            Commande = new Commande
            {
                Numero = new Random().Next(10000, 99999),
                DateCommande = DateTime.Now
            };
            
            // Initialiser les commandes
            AddPizzaToCommandeCommand = new RelayCommand(AddPizzaToCommande, CanAddPizzaToCommande);
            RemovePizzaFromCommandeCommand = new RelayCommand(RemovePizzaFromCommande, CanRemovePizzaFromCommande);
            SaveCommandeCommand = new RelayCommand(SaveCommande, CanSaveCommande);
            
            // Charger les pizzas disponibles
            _ = LoadPizzasAsync();
        }

        public CommandeViewModel(string dataPath, Commande commande, Client client) : this(dataPath)
        {
            _isEditMode = true;
            _client = client;
            
            // Utiliser la commande existante
            Commande = commande;
            
            // Charger les pizzas de la commande
            if (commande.Pizzas != null)
            {
                foreach (var pizza in commande.Pizzas)
                {
                    CommandePizzas.Add(pizza);
                }
            }
            
            // Recalculer le prix total
            CalculateTotalPrice();
        }

        #region Propriétés

        public ObservableCollection<Pizza> DisponiblePizzas
        {
            get => _disponiblePizzas;
            set
            {
                if (_disponiblePizzas != value)
                {
                    _disponiblePizzas = value;
                    OnPropertyChanged(nameof(DisponiblePizzas));
                }
            }
        }

        public ObservableCollection<Pizza> CommandePizzas
        {
            get => _commandePizzas;
            set
            {
                if (_commandePizzas != value)
                {
                    _commandePizzas = value;
                    OnPropertyChanged(nameof(CommandePizzas));
                    CalculateTotalPrice();
                }
            }
        }

        public Commande Commande
        {
            get => _commande;
            set
            {
                if (_commande != value)
                {
                    _commande = value;
                    OnPropertyChanged(nameof(Commande));
                }
            }
        }

        public Pizza SelectedPizza
        {
            get => _selectedPizza;
            set
            {
                if (_selectedPizza != value)
                {
                    _selectedPizza = value;
                    OnPropertyChanged(nameof(SelectedPizza));
                    (AddPizzaToCommandeCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public Pizza SelectedCommandePizza
        {
            get => _selectedCommandePizza;
            set
            {
                if (_selectedCommandePizza != value)
                {
                    _selectedCommandePizza = value;
                    OnPropertyChanged(nameof(SelectedCommandePizza));
                    (RemovePizzaFromCommandeCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public decimal PrixTotal
        {
            get => _prixTotal;
            set
            {
                if (_prixTotal != value)
                {
                    _prixTotal = value;
                    OnPropertyChanged(nameof(PrixTotal));
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
                }
            }
        }

        public string WindowTitle => IsEditMode ? "Modification d'une Commande" : "Création d'une Commande";

        #endregion

        #region Commandes

        public ICommand AddPizzaToCommandeCommand { get; }
        public ICommand RemovePizzaFromCommandeCommand { get; }
        public ICommand SaveCommandeCommand { get; }

        #endregion

        #region Méthodes

        private async Task LoadPizzasAsync()
        {
            try
            {
                var dataManager = new DataManager();
                var pizzas = await dataManager.LoadPizzasAsync(_dataPath);
                if (pizzas != null)
                {
                    DisponiblePizzas.Clear();
                    foreach (var pizza in pizzas)
                    {
                        DisponiblePizzas.Add(pizza);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Erreur lors du chargement des pizzas : {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalculateTotalPrice()
        {
            PrixTotal = CommandePizzas.Sum(p => p.Prix);
        }

        private void AddPizzaToCommande()
        {
            if (SelectedPizza == null) return;

            // Ajouter une copie de la pizza à la commande
            var pizza = new Pizza
            {
                Nom = SelectedPizza.Nom,
                Prix = SelectedPizza.Prix,
                Description = SelectedPizza.Description,
                ImagePath = SelectedPizza.ImagePath
            };

            CommandePizzas.Add(pizza);
            Commande.Pizzas.Add(pizza);
            
            // Mettre à jour le prix total
            CalculateTotalPrice();
        }

        private bool CanAddPizzaToCommande()
        {
            return SelectedPizza != null;
        }

        private void RemovePizzaFromCommande()
        {
            if (SelectedCommandePizza == null) return;

            CommandePizzas.Remove(SelectedCommandePizza);
            Commande.Pizzas.Remove(SelectedCommandePizza);
            
            // Mettre à jour le prix total
            CalculateTotalPrice();
        }

        private bool CanRemovePizzaFromCommande()
        {
            return SelectedCommandePizza != null;
        }

        private void SaveCommande()
        {
            if (CommandePizzas.Count == 0)
            {
                System.Windows.MessageBox.Show("Veuillez ajouter au moins une pizza à la commande.", "Information",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            
            // La sauvegarde sera effectuée par le ClientsViewModel
        }

        private bool CanSaveCommande()
        {
            return CommandePizzas.Count > 0;
        }

        #endregion
    }
}
