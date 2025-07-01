using System.Windows;
using PizzeriaApp.ViewModels;

namespace PizzeriaApp.Views
{
    /// <summary>
    /// Logique d'interaction pour ClientsWindow.xaml
    /// </summary>
    public partial class ClientsWindow : Window
    {
        public ClientsWindow(string dataPath)
        {
            InitializeComponent();
            DataContext = new ClientsViewModel(dataPath);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
