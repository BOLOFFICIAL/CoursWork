using CsvHelper;
using Microsoft.Win32;
using Regress.CSV;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Regress
{
    /// <summary>
    /// Логика взаимодействия для ChosePage.xaml
    /// </summary>
    public partial class ChosePage : Page
    {
        private string _filepath;
        private CsvData csv;
        private CheckBox[] checkBoxes;

        public ChosePage()
        {
            InitializeComponent();
        }

        public void Initialization()
        {
            filename.Content = System.IO.Path.GetFileName(_filepath);

            using (var reader = new StreamReader(_filepath))
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

            csv = new CsvData(_filepath);
            ComboBoxResult.ItemsSource = csv.GetNames();
        }

        private void ToAnalisePage(object sender, RoutedEventArgs e)
        {
            var resultname = ComboBoxResult.SelectedValue;
            if (!(resultname is null))
            {
                var name = resultname.ToString();
                if (name.Length > 0)
                {
                    NavigationService.Navigate(new AnalisePage(ComboBoxResult.SelectedValue.ToString(), _filepath));
                }
            }
            else
            {
                MessageBox.Show("Выберите колонку результатов");
            }
        }

        public void HighlightDataGridColumn(DataGrid dataGrid, int columnIndex)
        {
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                var column = dataGrid.Columns[i];
                var cellStyle = new Style(typeof(DataGridCell));
                if (i == columnIndex)
                {
                    cellStyle.Setters.Add(new Setter(DataGridCell.ForegroundProperty, Brushes.Red));
                    cellStyle.Setters.Add(new Setter(DataGridCell.FontWeightProperty, FontWeights.Bold));
                }
                else
                {
                    cellStyle.Setters.Add(new Setter(DataGridCell.ForegroundProperty, Brushes.Black));
                    cellStyle.Setters.Add(new Setter(DataGridCell.FontWeightProperty, FontWeights.Normal));
                }
                column.CellStyle = cellStyle;
            }
        }

        private void ToStartPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartPage());
        }

        private void ComboBoxResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridChose.Columns.Count > 0)
            {
                HighlightDataGridColumn(DataGridChose, ComboBoxResult.SelectedIndex);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "CSV files (*.csv)|*.csv";
                if (openFileDialog.ShowDialog() == true)
                {
                    _filepath = openFileDialog.FileName;
                    Initialization();
                    GridDataResults.Height = double.NaN;
                }
            }
            catch
            {
                MessageBox.Show("Перепроверьте фаил и повторите попытку", "Ошибка чтения файла", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
