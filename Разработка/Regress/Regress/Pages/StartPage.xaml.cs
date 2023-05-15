using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Regress
{
    /// <summary>
    /// Логика взаимодействия для StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "CSV files (*.csv)|*.csv";
                if (openFileDialog.ShowDialog() == true)
                {
                    NavigationService.Navigate(new ChosePage(openFileDialog.FileName));
                }
            }
            catch
            {
                MessageBox.Show("Перепроверьте фаил и повторите попытку", "Ошибка чтения файла", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
