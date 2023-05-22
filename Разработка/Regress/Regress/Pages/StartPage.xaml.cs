using Regress.Model;
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
            if (ProgramData.regressionindex > -1)
            {
                NavigationService.Navigate(new AnalisePage(true));
            }
            else
            {
                NavigationService.Navigate(new ChosePage());
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AboutPage());
        }
    }
}
