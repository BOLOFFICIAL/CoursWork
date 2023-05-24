using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Win32;
using Regress.CSV;
using Regress.Model;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Regress
{
    /// <summary>
    /// Логика взаимодействия для ChosePage.xaml
    /// </summary>
    public partial class ChosePage : Page
    {
        public ChosePage()
        {
            InitializeComponent();
            if (ProgramData.fileputh.Length > 0)
            {
                OpenFile(true);
            }
        }

        public void Initialization()
        {
            using (var reader = new StreamReader(ProgramData.fileputh))
            {
                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";"
                };

                using (var csv = new CsvReader(reader, csvConfig))
                {
                    using (var dr = new CsvDataReader(csv))
                    {
                        var dt = new DataTable();
                        dt.Load(dr);
                        DataGridChose.ItemsSource = dt.DefaultView;
                    }
                }
            }
            filename.Content = System.IO.Path.GetFileName(ProgramData.fileputh);
            ProgramData.csv = new CsvData(ProgramData.fileputh);
            ComboBoxResult.ItemsSource = ProgramData.csv.GetNames();
        }

        private void ToAnalisePage(object sender, RoutedEventArgs e)
        {
            if (!(ProgramData.resultcolumn.Length > 0))
            {
                MessageBox.Show("Выберите колонку результатов");
                return;
            }
            if (ProgramData.csv.ColumnCount<2)
            {
                MessageBox.Show("В файле недостаточно колонок.\nМинимальное количество 2");
                return;
            }
            NavigationService.Navigate(new AnalisePage());
        }

        public void HighlightDataGridColumn()
        {
            for (int i = 0; i < DataGridChose.Columns.Count; i++)
            {
                var column = DataGridChose.Columns[i];
                var cellStyle = new Style(typeof(DataGridCell));

                if (column.Header.ToString() == ProgramData.resultcolumn)
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
            ProgramData.resultcolumn = "";
            NavigationService.Navigate(new StartPage());
        }

        private void ComboBoxResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridChose.Columns.Count > 0)
            {
                var select = ComboBoxResult.SelectedValue;

                if (select != null)
                {
                    ProgramData.resultcolumn = select.ToString();
                }

                HighlightDataGridColumn();

                GridChoseResult.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Auto);
            }
        }

        private void Chose(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private void OpenFile(bool oldfile = false)
        {
            if (oldfile)
            {
                Initialization();
                UpdateForm();
            }
            else
            {
                try
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "CSV files (*.csv)|*.csv";
                    if (openFileDialog.ShowDialog() == true)
                    {
                        ComboBoxResult.SelectedValue = null;
                        new ProgramData(openFileDialog.FileName);
                        Initialization();
                        UpdateForm();
                    }
                }
                catch
                {
                    MessageBox.Show("Перепроверьте фаил и повторите попытку", "Ошибка чтения файла", MessageBoxButton.OK, MessageBoxImage.Error);
                    ProgramData.fileputh = "";
                    NavigationService.Navigate(new ChosePage());
                }
            }
        }

        private void UpdateForm()
        {
            GridChose.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Auto);
            GridChose.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
            ChoseFile.HorizontalAlignment = HorizontalAlignment.Left;
        }
    }
}
