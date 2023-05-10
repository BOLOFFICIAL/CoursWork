using OxyPlot;
using OxyPlot.Series;
using Regress.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input.Manipulations;

namespace Regress
{
    internal class CorrelationRegression
    {
        public LineSeries Line { get; private set; }
        private CsvData csv;
        private string _filepath;
        private string _regression;
        private string _result;
        private int _index;
        public List<string> Results { get; private set; }
        public CorrelationRegression(string filepath, string regression, string result, int index)
        {
            csv = new CsvData(filepath);
            _regression = regression;
            _filepath = filepath;
            _result = result;
            _index = index;
            if (_index == -1)
            {
                AutoAnalise();
            }
            else
            {
                AnaliseAsync(_result, csv.GetNames()[index]);
            }
        }
        public async Task AnaliseAsync(string r, string p)
        {
            var X = new List<double>();
            foreach (var el in csv.GetColumn(p).Value)
            {
                double element = 0;
                double.TryParse(el.Replace(".", ","), out element);
                X.Add(element);
            }
            var Y = new List<double>();
            foreach (var el in csv.GetColumn(r).Value)
            {
                double element = 0;
                double.TryParse(el.Replace(".", ","), out element);
                Y.Add(element);
            }
            Results = new List<string>();
            Line = new LineSeries();
            Line.Color = OxyColors.Red;
            double x = X.Min();
            double max = X.Max();
            double avr = X.Average();
            int count = X.Count();
            switch (_regression)
            {
                case "Линейная":
                    {
                        var regress = new LinearRegression(X, Y);
                        Results.Add(regress.Equation);
                        Results.Add(regress.R.ToString("0.####"));
                        Results.Add(AboutR(regress.R));
                        Results.Add(regress.R2.ToString("0.####"));
                        Results.Add(regress.AvrA.ToString("0.####"));
                        Results.Add(regress.Ffact.ToString("0.####"));
                        Results.Add(regress.Dfact.ToString("0.####"));
                        Results.Add(AboutDurbinWatson(regress.Dfact));
                        await Task.Run(() =>
                        {
                            while (x < max)
                            {
                                Line.Points.Add(new DataPoint(x, regress.a * x + regress.b));
                                x += avr / count;
                            }
                        });
                    }
                    break;
                case "Степенная":
                    {
                        var regress = new PowerRegression(X, Y);
                        Results.Add(regress.Equation);
                        Results.Add(regress.R.ToString("0.####"));
                        Results.Add(AboutR(regress.R));
                        Results.Add(regress.R2.ToString("0.####"));
                        Results.Add(regress.AvrA.ToString("0.####"));
                        Results.Add(regress.Ffact.ToString("0.####"));
                        Results.Add(regress.Dfact.ToString("0.####"));
                        Results.Add(AboutDurbinWatson(regress.Dfact));
                        await Task.Run(() =>
                        {
                            while (x < max)
                            {
                                Line.Points.Add(new DataPoint(x, regress.a * Math.Pow( x, regress.b)));
                                x += avr / count;
                            }
                        });
                    }
                    break;
                case "Квадратичная":
                    {
                        var regress = new QuadraticRegression(X, Y);
                        Results.Add(regress.Equation);
                        Results.Add(regress.R.ToString("0.####"));
                        Results.Add(AboutR(regress.R));
                        Results.Add(regress.R2.ToString("0.####"));
                        Results.Add(regress.AvrA.ToString("0.####"));
                        Results.Add(regress.Ffact.ToString("0.####"));
                        Results.Add(regress.Dfact.ToString("0.####"));
                        Results.Add(AboutDurbinWatson(regress.Dfact));
                        await Task.Run(() =>
                        {
                            while (x < max)
                            {
                                Line.Points.Add(new DataPoint(x, regress.a * x * x + regress.b * x + regress.c));
                                x += avr / count;
                            }
                        });
                    }
                    break;
                case "Логарифмическая":
                    {
                        var regress = new LogarithmicRegression(X, Y);
                        Results.Add(regress.Equation);
                        Results.Add(regress.R.ToString("0.####"));
                        Results.Add(AboutR(regress.R));
                        Results.Add(regress.R2.ToString("0.####"));
                        Results.Add(regress.AvrA.ToString("0.####"));
                        Results.Add(regress.Ffact.ToString("0.####"));
                        Results.Add(regress.Dfact.ToString("0.####"));
                        Results.Add(AboutDurbinWatson(regress.Dfact));
                        await Task.Run(() =>
                        {
                            while (x < max)
                            {
                                Line.Points.Add(new DataPoint(x, regress.a + regress.b * Math.Log(x)));
                                x += avr / count;
                            }
                        });
                    }
                    break;
                case "Гиперболическая":
                    {
                        var regress = new HyperbolicRegression(X, Y);
                        Results.Add(regress.Equation);
                        Results.Add(regress.R.ToString("0.####"));
                        Results.Add(AboutR(regress.R));
                        Results.Add(regress.R2.ToString("0.####"));
                        Results.Add(regress.AvrA.ToString("0.####"));
                        Results.Add(regress.Ffact.ToString("0.####"));
                        Results.Add(regress.Dfact.ToString("0.####"));
                        Results.Add(AboutDurbinWatson(regress.Dfact));

                        await Task.Run(() =>
                        {
                            while (x < max)
                            {
                                Line.Points.Add(new DataPoint(x, regress.a + regress.b / x));
                                x += avr / count;
                            }
                        });
                    }
                    break;
            }
        }

        private string AboutR(double r)
        {
            string res = "";
            if (Math.Abs(r) > 0.9)
            {
                res= "Сильная";
            }
            else if (Math.Abs(r) > 0.7)
            {
                res = "Умеренная";
            }
            else if (Math.Abs(r) > 0.5)
            {
                res = "Слабая";
            }
            else if (Math.Abs(r) > 0.3)
            {
                res = "Слабая";
            }
            else
            {
                res = "Отсутствующая";
            }
            if (r < 0)
            {
                res += " (отрицаптельная)";
            }
            else { res += " (положительная)"; }
            return res;
        }

        private string AboutDurbinWatson(double dw)
        {
            if (dw < 1.0)
            {
                return "Положительная (сильная)";
            }
            else if (dw < 1.5)
            {
                return "Положительная (умеренная)";
            }
            else if (dw < 2.5)
            {
                return "Отсутствует";
            }
            else if (dw < 3.0)
            {
                return "Отрицательная автокорреляция (умеренная)";
            }
            else
            {
                return "Отрицательная автокорреляция (сильная)";
            }
        }

        public void AutoAnalise()
        {

        }
    }
}
