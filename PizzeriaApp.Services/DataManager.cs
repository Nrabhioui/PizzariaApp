using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PizzeriaApp.Core;

namespace PizzeriaApp.Services
{
    /// <summary>
    /// Gère la sérialisation et désérialisation des données de l'application.
    /// </summary>
    public class DataManager
    {
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        #region JSON Serialization

        /// <summary>
        /// Sauvegarde une liste de clients au format JSON.
        /// </summary>
        /// <param name="clients">Liste des clients à sauvegarder.</param>
        /// <param name="filePath">Chemin du fichier de sauvegarde.</param>
        public async Task SaveClientsToJsonAsync(List<Client> clients, string filePath)
        {
            using FileStream fs = new FileStream(filePath, FileMode.Create);
            await JsonSerializer.SerializeAsync(fs, clients, _jsonOptions);
        }

        /// <summary>
        /// Charge une liste de clients depuis un fichier JSON.
        /// </summary>
        /// <param name="filePath">Chemin du fichier à charger.</param>
        /// <returns>Liste des clients chargés.</returns>
        public async Task<List<Client>> LoadClientsFromJsonAsync(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<Client>();

            using FileStream fs = new FileStream(filePath, FileMode.Open);
            return await JsonSerializer.DeserializeAsync<List<Client>>(fs, _jsonOptions) ?? new List<Client>();
        }

        /// <summary>
        /// Sauvegarde une liste de pizzas au format JSON.
        /// </summary>
        /// <param name="pizzas">Liste des pizzas à sauvegarder.</param>
        /// <param name="filePath">Chemin du fichier de sauvegarde.</param>
        public async Task SavePizzasToJsonAsync(List<Pizza> pizzas, string filePath)
        {
            using FileStream fs = new FileStream(filePath, FileMode.Create);
            await JsonSerializer.SerializeAsync(fs, pizzas, _jsonOptions);
        }

        /// <summary>
        /// Charge une liste de pizzas depuis un fichier JSON.
        /// </summary>
        /// <param name="filePath">Chemin du fichier à charger.</param>
        /// <returns>Liste des pizzas chargées.</returns>
        public async Task<List<Pizza>> LoadPizzasFromJsonAsync(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<Pizza>();

            using FileStream fs = new FileStream(filePath, FileMode.Open);
            return await JsonSerializer.DeserializeAsync<List<Pizza>>(fs, _jsonOptions) ?? new List<Pizza>();
        }

        /// <summary>
        /// Sauvegarde une liste de commandes au format JSON.
        /// </summary>
        /// <param name="commandes">Liste des commandes à sauvegarder.</param>
        /// <param name="filePath">Chemin du fichier de sauvegarde.</param>
        public async Task SaveCommandesToJsonAsync(List<Commande> commandes, string filePath)
        {
            using FileStream fs = new FileStream(filePath, FileMode.Create);
            await JsonSerializer.SerializeAsync(fs, commandes, _jsonOptions);
        }

        /// <summary>
        /// Charge une liste de commandes depuis un fichier JSON.
        /// </summary>
        /// <param name="filePath">Chemin du fichier à charger.</param>
        /// <returns>Liste des commandes chargées.</returns>
        public async Task<List<Commande>> LoadCommandesFromJsonAsync(string filePath)
        {
            if (!File.Exists(filePath))
                return new List<Commande>();

            using FileStream fs = new FileStream(filePath, FileMode.Open);
            return await JsonSerializer.DeserializeAsync<List<Commande>>(fs, _jsonOptions) ?? new List<Commande>();
        }

        /// <summary>
        /// Sauvegarde les clients dans un répertoire spécifié.
        /// </summary>
        /// <param name="clients">Collection de clients à sauvegarder.</param>
        /// <param name="dataPath">Répertoire de sauvegarde.</param>
        public async Task SaveClientsAsync(IEnumerable<Client> clients, string dataPath)
        {
            // Créer le répertoire s'il n'existe pas
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            string clientsFilePath = Path.Combine(dataPath, "clients.json");
            await SaveClientsToJsonAsync(new List<Client>(clients), clientsFilePath);
        }

        /// <summary>
        /// Charge les clients depuis un répertoire spécifié.
        /// </summary>
        /// <param name="dataPath">Répertoire de chargement.</param>
        /// <returns>Liste des clients chargés.</returns>
        public async Task<ObservableCollection<Client>> LoadClientsAsync(string dataPath)
        {
            string clientsFilePath = Path.Combine(dataPath, "clients.json");
            var clients = await LoadClientsFromJsonAsync(clientsFilePath);
            return new ObservableCollection<Client>(clients);
        }

        /// <summary>
        /// Sauvegarde les pizzas dans un répertoire spécifié.
        /// </summary>
        /// <param name="pizzas">Collection de pizzas à sauvegarder.</param>
        /// <param name="dataPath">Répertoire de sauvegarde.</param>
        public async Task SavePizzasAsync(IEnumerable<Pizza> pizzas, string dataPath)
        {
            // Créer le répertoire s'il n'existe pas
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            string pizzasFilePath = Path.Combine(dataPath, "pizzas.json");
            await SavePizzasToJsonAsync(new List<Pizza>(pizzas), pizzasFilePath);
        }

        /// <summary>
        /// Charge les pizzas depuis un répertoire spécifié.
        /// </summary>
        /// <param name="dataPath">Répertoire de chargement.</param>
        /// <returns>Liste des pizzas chargées.</returns>
        public async Task<ObservableCollection<Pizza>> LoadPizzasAsync(string dataPath)
        {
            string pizzasFilePath = Path.Combine(dataPath, "pizzas.json");
            var pizzas = await LoadPizzasFromJsonAsync(pizzasFilePath);
            return new ObservableCollection<Pizza>(pizzas);
        }

        #endregion

        #region XML Serialization

        /// <summary>
        /// Exporte une pizza au format XML.
        /// </summary>
        /// <param name="pizza">Pizza à exporter.</param>
        /// <param name="filePath">Chemin du fichier d'exportation.</param>
        public void ExportPizzaToXml(Pizza pizza, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Pizza));
            using FileStream fs = new FileStream(filePath, FileMode.Create);
            serializer.Serialize(fs, pizza);
        }

        /// <summary>
        /// Importe une pizza depuis un fichier XML.
        /// </summary>
        /// <param name="filePath">Chemin du fichier à importer.</param>
        /// <returns>Pizza importée.</returns>
        public Pizza ImportPizzaFromXml(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Le fichier XML spécifié n'existe pas.", filePath);

            XmlSerializer serializer = new XmlSerializer(typeof(Pizza));
            using FileStream fs = new FileStream(filePath, FileMode.Open);
            return (Pizza)serializer.Deserialize(fs);
        }

        #endregion
    }
}
