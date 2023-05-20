using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Regress.Model;
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
        private CorrelationRegression _regression;

        public AnalisePage()
        {
            InitializeComponent();

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

            ComboBoxParameter.ItemsSource = ProgramData.csv.GetNames().Where(name => name != ProgramData.resultcolumn).ToList();
        }

        private void PrintOXY(LineSeries equation)
        {
            var plotModel = new PlotModel();

            var scatterSeries = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                MarkerFill = OxyColor.FromRgb(50, 50, 50),
                Color = OxyColor.FromRgb(50, 50, 50),
                LineStyle = LineStyle.Dot,
                StrokeThickness = 2
            };

            for (int i = 0; i < ProgramData.csv.RowCount; i++)
            {
                scatterSeries.Points.Add(new DataPoint(ProgramData.X[i], ProgramData.Y[i]));
            }

            plotModel.Series.Add(scatterSeries);
            plotModel.Series.Add(equation);
            plotModel.Title = $"Dependence {ProgramData.resultcolumn} - {ProgramData.parametercolumn}";

            plotModel.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Bottom,
                    Title = ProgramData.parametercolumn.ToUpper(),
                    TitleFontWeight = OxyPlot.FontWeights.Bold,
                    TitleColor = OxyColor.FromRgb(50, 50, 50),
                    TitleFontSize = 15,
                    FontWeight = OxyPlot.FontWeights.Bold
                });

            plotModel.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = ProgramData.resultcolumn.ToUpper(),
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
            ProgramData.parametercolumn = ComboBoxParameter.SelectedValue.ToString();

            ProgramData.X = ProgramData.csv.GetColumn(ProgramData.parametercolumn).Value
                .Select(el => double.TryParse(el.Replace(".", ","), out double element) ? element : 0)
                .ToList();
            ProgramData.Y = ProgramData.csv.GetColumn(ProgramData.resultcolumn).Value
                .Select(el => double.TryParse(el.Replace(".", ","), out double element) ? element : 0)
                .ToList();

            var data = ProgramData.X.Zip(ProgramData.Y, (x, y) => new { X = x, Y = y }).ToList();

            data.Sort((a, b) => a.X.CompareTo(b.X));

            ProgramData.X = data.Select(pair => pair.X).ToList();
            ProgramData.Y = data.Select(pair => pair.Y).ToList();

            _regression = new CorrelationRegression(ComboBoxRegression.SelectedIndex, auto);

            List<string> results = _regression.Results;

            Label1.Content = results[0].Replace("+ -", "- ");
            Label2.Content = results[1];
            Label3.Content = results[2];
            Label4.Content = results[3];
            Label5.Content = $"{results[4]}%";
            Label6.Content = results[5];
            Label7.Content = results[6];
            Label8.Content = results[7];

            PrintOXY(_regression.Line);

            if (auto)
            {
                ComboBoxRegression.SelectedIndex = _regression.TitleIndex;
            }

            GridData.Height = double.NaN;
            ButtonSave.Visibility = Visibility.Visible;
            BorderAnalize.Visibility = Visibility.Visible;

            GridAnalize.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
            GridAnalize.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Auto);
            GridDataAnalize.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Auto);
            GridDataAnalize.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
            GridDataAnalize.RowDefinitions[2].Height = new GridLength(0, GridUnitType.Auto);
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
                new ProgramData(openFileDialog.FileName);
                NavigationService.Navigate(new ChosePage());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartPage());
        }
    }
}
