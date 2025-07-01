using System.Windows;
using PizzeriaApp.Core;
using PizzeriaApp.ViewModels;

namespace PizzeriaApp.Views
{
    /// <summary>
    /// Logique d'interaction pour ExportWindow.xaml
    /// </summary>
    public partial class ExportWindow : Window
    {
        private readonly ExportViewModel _viewModel;

        public ExportWindow(string dataPath, Pizza pizza)
        {
            InitializeComponent();
            _viewModel = new ExportViewModel(dataPath, pizza);
            DataContext = _viewModel;

            // S'abonner à l'événement d'exportation réussie
            _viewModel.ExportCompleted += (s, success) =>
            {
                if (success)
                {
                    Close();
                }
            };
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
