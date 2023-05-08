using CsvHelper;
using Microsoft.Win32;
using Regress.CSV;
using System.Collections.Generic;
using System.Data;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Regress
{
    /// <summary>
    /// Логика взаимодействия для ChosePage.xaml
    /// </summary>
    public partial class ChosePage : Page
    {
        private string filepath;
        private CsvData csv;
        private CheckBox[] checkBoxes;

        public ChosePage(string filepath)
        {
            InitializeComponent();
            this.filepath = filepath;
            Initialization();
        }
        public void Initialization()
        {
            filename.Content = System.IO.Path.GetFileName(filepath);

            using (var reader = new StreamReader(filepath))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    using (var dr = new CsvDataReader(csv))
                    {
                        var dt = new DataTable();
                        dt.Load(dr);
                        DataGridChose.ItemsSource = dt.DefaultView;
                    }
                }
            }
            
            csv = new CsvData(filepath);
            var names = new List<string>();
            for (int i=0;i< csv.ColumnCount;i++) 
            {
                names.Add(csv.Columns[i].Name);
            }

            StackPanel stackPanel = new StackPanel();
            checkBoxes = new CheckBox[names.Count];

            for (int i = 0; i < names.Count; i++)
            {
                Label label = new Label();
                label.Content = names[i];
                CheckBox checkBox = new CheckBox();
                checkBoxes[i] = checkBox;
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.Children.Add(label);
                grid.Children.Add(checkBox);
                Grid.SetColumn(label, 0);
                Grid.SetColumn(checkBox, 1);
                stackPanel.Children.Add(grid);
            }

            ScrollViewerData.Content = stackPanel;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var checks = new List<int>();
            for (int i=0;i< checkBoxes.Length;i++) 
            {
                if (checkBoxes[i].IsChecked==true) 
                {
                    checks.Add(i);
                }
            }
            NavigationService.Navigate(new Page1(checks));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartPage());
        }
    }
}
