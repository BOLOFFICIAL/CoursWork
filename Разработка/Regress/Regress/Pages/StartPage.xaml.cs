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
            NavigationService.Navigate(new ChosePage());
        }
    }
}
