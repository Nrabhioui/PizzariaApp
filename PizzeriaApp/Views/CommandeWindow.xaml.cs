using System.Windows;
using PizzeriaApp.Core;
using PizzeriaApp.ViewModels;

namespace PizzeriaApp.Views
{
    /// <summary>
    /// Logique d'interaction pour CommandeWindow.xaml
    /// </summary>
    public partial class CommandeWindow : Window
    {
        private readonly CommandeViewModel _viewModel;

        public CommandeWindow(string dataPath)
        {
            InitializeComponent();
            _viewModel = new CommandeViewModel(dataPath);
            DataContext = _viewModel;
            Owner = System.Windows.Application.Current.MainWindow;
        }

        public CommandeWindow(CommandeViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            Owner = System.Windows.Application.Current.MainWindow;
        }

        public Commande Commande => _viewModel.Commande;

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
