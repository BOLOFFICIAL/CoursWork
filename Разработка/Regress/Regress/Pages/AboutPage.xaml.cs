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
                "«RiverCorr» - О программе.\n\n" +
                "\tДобро пожаловать на страницу \"О программе\" «RiverCorr»! Ниже представлена информация о нашей программе, ее функциях, особенностях и целях.\n" +
                "\nЦель программы:\n\n" +
                "\tГлавная цель нашей программы – это выявление влияния загрязнений водоемов на численность рыбных популяций. «RiverCorr» решает проблему информирования о значимости влияния загрязнений на количество рыбного сырья. Данная программа является полезным и уникальным решением, так как на рынке нет альтернатив проекта, что делает проблему загрязнений значительнее.\n" +
                "\nОписание программы:\n\n" +
                "\tВ наше время существует проблема загрязненности водоемов. Каждый год все больше и больше мусора оказывается в водоемах. Наша программа позволяет пользователям наглядно увидеть влияние загрязнения водоемов на численность рыбных популяций. Также с помощью корреляционно-регрессионного анализа - построение и анализ экономико - математической модели в форме уравнений регрессии(корреляционная связь), характеризующих зависимость признака от его определяющих факторов - данная программа позволяет выявлять связи между изменением состояния водной среды и изменением численности рыбных ресурсов.\n";

            return about;
        }

        private void ToStartPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartPage());
        }
    }
}
