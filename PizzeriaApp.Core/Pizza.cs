using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace PizzeriaApp.Core
{
    /// <summary>
    /// Représente une pizza dans le système.
    /// </summary>
    [Serializable]
    public class Pizza : INotifyPropertyChanged
    {
        private string _nom = string.Empty;
        private decimal _prix;
        private string _imagePath = string.Empty;
        private string _description = string.Empty;

        /// <summary>
        /// Obtient ou définit le nom de la pizza.
        /// </summary>
        [XmlElement("Nom")]
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
        /// Obtient ou définit le prix de la pizza.
        /// </summary>
        [XmlElement("Prix")]
        public decimal Prix
        {
            get => _prix;
            set
            {
                if (_prix != value)
                {
                    _prix = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Obtient ou définit le chemin de l'image de la pizza.
        /// </summary>
        [XmlElement("ImagePath")]
        public string ImagePath
        {
            get => _imagePath;
            set
            {
                if (_imagePath != value)
                {
                    _imagePath = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Obtient ou définit la description de la pizza.
        /// </summary>
        [XmlElement("Description")]
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
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
