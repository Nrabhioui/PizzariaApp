using System.Windows;
using PizzeriaApp.ViewModels;

namespace PizzeriaApp.Views
{
    /// <summary>
    /// Logique d'interaction pour SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private readonly SettingsViewModel _viewModel;

        public SettingsWindow(string initialDataPath)
        {
            InitializeComponent();
            _viewModel = new SettingsViewModel(initialDataPath);
            DataContext = _viewModel;
        }

        public string DataPath => _viewModel.DataPath;

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveSettingsCommand.Execute(null);
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
