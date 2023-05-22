using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Regress
{
    /// <summary>
    /// Логика взаимодействия для AboutPage.xaml
    /// </summary>
    public partial class AboutPage : Page
    {
        public AboutPage()
        {
            InitializeComponent();
            TextBlockAbout.Text = TextAbout();
        }

        private string TextAbout()
        {
            string about = "";
            about += "" +
                "RiverCorr\r\n" +
                "Создано ООО \"МИКС\" в следующем составе:\r\n" +
                "Болонин М.А – ведущий программист, дизайнер\r\n" +
                "Высотин И.С. – руководитель проекта, младший программист, юрист\r\n" +
                "Добрецов Н.В. – тестировщик, контроль качества\r\n\r\n" +
                "Программа \"RiverCorr\" может быть использована специалистами в области экологии и управления рыбными ресурсами для анализа влияния загрязнения водоемов на численность рыбных популяций. С помощью корреляционно-регрессионного анализа, программа позволяет выявлять связи между изменением состояния водной среды и изменением численности рыбных ресурсов.\r\n\r\n" +
                "При нажатии на кнопку \"Анализ\" вы перейдете на следующую страницу, на которой вам будет необходимо выбрать файл в разрешении \"*.csv\", затем, выбрать результирующий столбец, тип регрессии и столбец по которому она будет производиться. После чего вам будут предоставлены результаты анализа, которые также можно сохранить в отчет";

            return about;
        }

        private void ToStartPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartPage());
        }
    }
}
