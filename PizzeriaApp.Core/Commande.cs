using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PizzeriaApp.Core
{
    /// <summary>
    /// Représente une commande dans le système.
    /// </summary>
    public class Commande : INotifyPropertyChanged
    {
        private int _numero;
        private DateTime _dateCommande = DateTime.Now;
        private ObservableCollection<Pizza> _pizzas = new ObservableCollection<Pizza>();

        /// <summary>
        /// Obtient ou définit le numéro de la commande.
        /// </summary>
        public int Numero
        {
            get => _numero;
            set
            {
                if (_numero != value)
                {
                    _numero = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Obtient ou définit la date de la commande.
        /// </summary>
        public DateTime DateCommande
        {
            get => _dateCommande;
            set
            {
                if (_dateCommande != value)
                {
                    _dateCommande = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Obtient ou définit la liste des pizzas dans la commande.
        /// </summary>
        public ObservableCollection<Pizza> Pizzas
        {
            get => _pizzas;
            set
            {
                if (_pizzas != value)
                {
                    _pizzas = value;
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
