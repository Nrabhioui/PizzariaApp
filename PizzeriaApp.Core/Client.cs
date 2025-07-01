using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PizzeriaApp.Core
{
    /// <summary>
    /// Représente un client dans le système.
    /// </summary>
    public class Client : INotifyPropertyChanged
    {
        private string _nom = string.Empty;
        private string _adresse = string.Empty;
        private string _telephone = string.Empty;
        private ObservableCollection<Commande> _commandes = new ObservableCollection<Commande>();

        /// <summary>
        /// Obtient ou définit le nom du client.
        /// </summary>
        public string Nom
        {
            get => _nom;
            set
            {
                if (_nom != value)
                {
                    _nom = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Obtient ou définit l'adresse du client.
        /// </summary>
        public string Adresse
        {
            get => _adresse;
            set
            {
                if (_adresse != value)
                {
                    _adresse = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Obtient ou définit le numéro de téléphone du client.
        /// </summary>
        public string Telephone
        {
            get => _telephone;
            set
            {
                if (_telephone != value)
                {
                    _telephone = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Obtient ou définit la liste des commandes du client.
        /// </summary>
        public ObservableCollection<Commande> Commandes
        {
            get => _commandes;
            set
            {
                if (_commandes != value)
                {
                    _commandes = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Événement déclenché lorsqu'une propriété change.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Méthode pour déclencher l'événement PropertyChanged.
        /// </summary>
        /// <param name="propertyName">Nom de la propriété qui a changé.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
