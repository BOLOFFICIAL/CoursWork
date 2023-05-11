using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Regress.CSV;
using System.Collections.Generic;
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
        CsvData csv;
        string _filepath;
        string _resultcolumn;
        CorrelationRegression regression;
        public AnalisePage(string resultcolumn, string filepath)
        {
            InitializeComponent();
            _filepath = filepath;
            csv = new CsvData(filepath);
            _resultcolumn = resultcolumn;
            ComboBoxRegression.SelectedIndex = 0;
            ComboBoxParameter.SelectedIndex = 0;

            Regressions();
            Parameters();
        }

        private void Parameters()
        {
            var parameters = new List<string>();
            foreach (var name in csv.GetNames())
            {
                if (name != _resultcolumn)
                {
                    parameters.Add(name);
                }
            }
            ComboBoxParameter.ItemsSource = parameters;
        }

        private void Regressions()
        {
            ComboBoxRegression.ItemsSource = new List<string>()
            {
                "Линейная",
                "Степенная",
                "Квадратичная",
                "Логарифмическая",
                "Гиперболическая",
            };
        }

        private void PrintOXY(string resultcolumn, string parameter, LineSeries equation)
        {
            var plotModel = new PlotModel();



            var scatterSeries = new ScatterSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.Black,
                MarkerFill = OxyColors.Green,
            };

            var series1 = new LineSeries
            {
                Color = OxyColors.Red,
            };

            csv.SetResultColumn(resultcolumn);

            var X = new List<double>();

            foreach (var el in csv.GetColumn(parameter).Value)
            {
                double element = 0;
                double.TryParse(el.Replace(".", ","), out element);
                X.Add(element);
            }

            var Y = new List<double>();

            foreach (var el in csv.ResultColumn.Value)
            {
                double element = 0;
                double.TryParse(el.Replace(".", ","), out element);
                Y.Add(element);
            }

            for (int i = 0; i < csv.RowCount; i++)
            {
                scatterSeries.Points.Add(new ScatterPoint(X[i], Y[i]));
            }

            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(X.Max() + X.Max() * 0.3, 0));
            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(0, Y.Max() + Y.Max() * 0.3));
            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(X.Min() + X.Min() * 0.3, 0));
            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(0, Y.Min() + Y.Min() * 0.3));

            plotModel.Series.Add(series1);
            plotModel.Series.Add(scatterSeries);
            plotModel.Series.Add(equation);
            plotModel.Title = $"{resultcolumn} - {parameter}";
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = parameter });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = resultcolumn });

            PlotViewAnalise.Model = plotModel;
        }

        private void Button_Back(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ChosePage(_filepath));
        }

        private void Button_Analise(object sender, RoutedEventArgs e)
        {
            Analise(ComboBoxParameter.SelectedIndex);
        }

        private void Button_AutoAnalise(object sender, RoutedEventArgs e)
        {
            return;
            Analise(-1);
        }

        private void Analise(int index)
        {
            regression = new CorrelationRegression(_filepath, ComboBoxRegression.SelectedValue.ToString(), _resultcolumn, index);
            Label1.Content = regression.Results[0].Replace("+ -", "- ");
            Label2.Content = regression.Results[1];
            Label3.Content = regression.Results[2];
            Label4.Content = regression.Results[3];
            Label5.Content = regression.Results[4];
            Label6.Content = regression.Results[5];
            Label7.Content = regression.Results[6];
            Label8.Content = regression.Results[7];
            PrintOXY(_resultcolumn, ComboBoxParameter.SelectedValue.ToString(), regression.Line);
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = regression.Title + "Regression";
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
                exporter.Height = 6000;

                using (var stream = File.Create(saveFileDialog.FileName))
                {
                    exporter.Export(plotModel, stream);
                }
            }
        }



    }
}
