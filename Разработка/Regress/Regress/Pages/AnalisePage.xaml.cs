using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Regress.CSV;
using System.Collections.Generic;
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
                "Степенной",
                "Квадратичной",
                "Логарифмической",
                "Гиперболической",
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
            series1.Points.Add(new DataPoint(X.Max() + 10, 0));
            series1.Points.Add(new DataPoint(0, 0));
            series1.Points.Add(new DataPoint(0, Y.Max() + 10));

            plotModel.Series.Add(series1);
            plotModel.Series.Add(scatterSeries);
            //plotModel.Series.Add(equation);
            plotModel.Title = $"График зависимостей {resultcolumn} От {parameter}";
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = parameter });
            plotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = resultcolumn });

            PlotViewAnalise.Model = plotModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ChosePage(_filepath));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var r = _resultcolumn;
            var p = ComboBoxParameter.SelectedValue.ToString();
            CorrelationRegression regression = new CorrelationRegression(r, p);
            PrintOXY(r, p, regression.Line);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(ComboBoxParameter.SelectedValue.ToString());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }
    }
}
