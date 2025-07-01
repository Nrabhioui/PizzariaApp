using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PizzeriaApp.ViewModels
{
    /// <summary>
    /// Classe de base pour tous les ViewModels, implémentant INotifyPropertyChanged
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Méthode pour notifier que la propriété a changé
        /// </summary>
        /// <param name="propertyName">Nom de la propriété</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Méthode utilitaire pour définir une propriété et notifier le changement
        /// </summary>
        /// <typeparam name="T">Type de la propriété</typeparam>
        /// <param name="field">Référence au champ de stockage</param>
        /// <param name="value">Nouvelle valeur</param>
        /// <param name="propertyName">Nom de la propriété</param>
        /// <returns>True si la valeur a changé, sinon false</returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
