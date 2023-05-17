using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Regress.CSV;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Regress
{
    /// <summary>
    /// Логика взаимодействия для AnalisePage.xaml
    /// </summary>
    public partial class AnalisePage : Page
    {
        private CsvData _csv;
        private string _filepath;
        private string _resultcolumn;
        private CorrelationRegression _regression;

        public AnalisePage(string resultcolumn, string filepath)
        {
            InitializeComponent();
            _filepath = filepath;
            _csv = new CsvData(filepath);
            _resultcolumn = resultcolumn;

            ComboBoxRegression.SelectedIndex = 0;
            ComboBoxParameter.SelectedIndex = 0;

            ComboBoxRegression.ItemsSource = new List<string>()
            {
                "Линейная",
                "Степенная",
                "Квадратичная",
                "Логарифмическая",
                "Гиперболическая",
            };

            ComboBoxParameter.ItemsSource = _csv.GetNames().Where(name => name != _resultcolumn).ToList();
        }

        private void PrintOXY(string resultcolumn, string parameter, LineSeries equation)
        {
            var plotModel = new PlotModel();

            var scatterSeries = new ScatterSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 3,
                MarkerStrokeThickness = 2,
                MarkerStroke = OxyColor.FromRgb(50, 50, 50),
                MarkerFill = OxyColor.FromRgb(50, 50, 50),
            };

            var series1 = new LineSeries
            {
                Color = OxyColors.Red
            };

            //_csv.SetResultColumn(resultcolumn);

            var X = _csv.GetColumn(parameter).Value.Select(el => double.TryParse(el.Replace(".", ","), out double element) ? element : 0).ToList();
            var Y = _csv.GetColumn(resultcolumn).Value.Select(el => double.TryParse(el.Replace(".", ","), out double element) ? element : 0).ToList();

            for (int i = 0; i < _csv.RowCount; i++)
            {
                scatterSeries.Points.Add(new ScatterPoint(X[i], Y[i]));
            }

            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(X.Max() * 1.3, 0));
            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(0, Y.Max() * 1.3));
            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(X.Min() * 1.3, 0));
            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(0, Y.Min() * 1.3));

            plotModel.Series.Add(series1);
            plotModel.Series.Add(scatterSeries);
            plotModel.Series.Add(equation);
            plotModel.Title = $"Dependence {resultcolumn} - {parameter}";

            plotModel.Axes.Add(
                new LinearAxis 
                { 
                    Position = AxisPosition.Bottom, 
                    Title = parameter.ToUpper(), 
                    TitleFontWeight = OxyPlot.FontWeights.Bold, 
                    TitleColor = OxyColor.FromRgb(50, 50, 50), 
                    TitleFontSize = 15, 
                    FontWeight = OxyPlot.FontWeights.Bold 
                });

            plotModel.Axes.Add(
                new LinearAxis 
                { 
                    Position = AxisPosition.Left, 
                    Title = resultcolumn.ToUpper(), 
                    TitleColor = OxyColor.FromRgb(50, 50, 50), 
                    TitleFontWeight = OxyPlot.FontWeights.Bold, 
                    TitleFontSize = 15, 
                    FontWeight = OxyPlot.FontWeights.Bold 
                });

            PlotViewAnalise.Model = plotModel;
        }

        private void ToEditFile(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ChosePage());
        }

        private void Analize(object sender, RoutedEventArgs e)
        {
            Analize(false);
        }

        private void AutoAnalize(object sender, RoutedEventArgs e)
        {
            Analize(true);
        }

        private void Analize(bool auto)
        {
            _regression = new CorrelationRegression(_filepath, ComboBoxRegression.SelectedIndex, _resultcolumn, ComboBoxParameter.SelectedValue.ToString(), auto);

            List<string> results = _regression.Results;

            Label1.Content = results[0].Replace("+ -", "- ");
            Label2.Content = results[1];
            Label3.Content = results[2];
            Label4.Content = results[3];
            Label5.Content = $"{results[4]}%";
            Label6.Content = results[5];
            Label7.Content = results[6];
            Label8.Content = results[7];

            PrintOXY(_resultcolumn, ComboBoxParameter.SelectedValue.ToString(), _regression.Line);

            if (auto)
            {
                ComboBoxRegression.SelectedIndex = _regression.TitleIndex;
            }

            GridData.Height = double.NaN;
            ButtonSave.Visibility = Visibility.Visible;
        }

        private void SavePdf(object sender, RoutedEventArgs e)
        {
            if (!(_regression?.Title is null))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = _regression.Title + "Regression";
                saveFileDialog.DefaultExt = ".pdf";
                saveFileDialog.Filter = "Regressions (.pdf)|*.pdf";

                if (saveFileDialog.ShowDialog() == true)
                {
                    var plotModel = PlotViewAnalise.Model;

                    // добавляем текст сверху графика
                    var textAnnotationTop = new OxyPlot.Annotations.TextAnnotation
                    {
                        Text = "UHFABE",
                        TextPosition = new DataPoint(0.5, plotModel.PlotArea.Top - 400),
                        FontSize = 18,
                        FontWeight = OxyPlot.FontWeights.Bold,
                        TextHorizontalAlignment = OxyPlot.HorizontalAlignment.Center
                    };
                    plotModel.Annotations.Add(textAnnotationTop);

                    // добавляем текст под графиком
                    var textAnnotationBottom = new OxyPlot.Annotations.TextAnnotation
                    {
                        Text = "BOLOFFICIAL",
                        TextPosition = new DataPoint(0.5, plotModel.PlotArea.Bottom + 200),
                        FontSize = 18,
                        FontWeight = OxyPlot.FontWeights.Bold,
                        TextHorizontalAlignment = OxyPlot.HorizontalAlignment.Center
                    };
                    plotModel.Annotations.Add(textAnnotationBottom);

                    var exporter = new PdfExporter();

                    exporter.Width = 800;
                    exporter.Height = 600;

                    using (var stream = File.Create(saveFileDialog.FileName))
                    {
                        exporter.Export(plotModel, stream);
                    }
                    var result = MessageBox.Show("Отчет успешно сохранен.\nХотите открыть сохраненный файл?", "Сохранение завершено", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        Process.Start("cmd", "/c start \"\" \"" + saveFileDialog.FileName + "\"");
                    }
                }
            }
            else
            {
                MessageBox.Show("Нет данных для формирования отчета");
            }
        }

        private void ToChosePage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                NavigationService.Navigate(new ChosePage());
            }
        }
    }
}
