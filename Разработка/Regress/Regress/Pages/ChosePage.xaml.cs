using CsvHelper;
using Regress.CSV;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
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

            ComboBoxResult.ItemsSource = csv.GetNames();
            var names = new List<string>();
            for (int i = 0; i < csv.ColumnCount; i++)
            {
                names.Add(csv.Columns[i].Name);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var resultname = ComboBoxResult.SelectedValue;
            if (!(resultname is null))
            {
                var name = resultname.ToString();
                if (name.Length > 0)
                {
                    NavigationService.Navigate(new AnalisePage(ComboBoxResult.SelectedValue.ToString(), filepath));
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartPage());
        }
    }
}
