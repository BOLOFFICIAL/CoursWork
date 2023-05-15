using OxyPlot;
using OxyPlot.Series;
using Regress.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Regress
{
    internal class CorrelationRegression
    {
        public LineSeries Line { get; private set; }
        private CsvData csv;
        public int TitleIndex { get; private set; }
        private string _filepath;
        private string _parameter;
        private string _result;
        private int _index;
        int bestregression;
        private List<double> X;
        private List<double> Y;
        public List<LineSeries> lines;
        private List<string> equations;
        public string Title { get; private set; }
        public List<string> Results { get; private set; }
        public CorrelationRegression(string filepath, int index, string result, string parameter, bool auto)
        {
            csv = new CsvData(filepath);
            _parameter = parameter;
            _filepath = filepath;
            _result = result;
            _index = index;
            X = csv.GetColumn(_parameter).Value.Select(el => double.TryParse(el.Replace(".", ","), out double element) ? element : 0).ToList();
            Y = csv.GetColumn(_result).Value.Select(el => double.TryParse(el.Replace(".", ","), out double element) ? element : 0).ToList();
            if (auto)
            {
                AutoAnalise();
            }
            else
            {
                Analise(_result, _parameter);
            }
        }
        public async Task Analise(string r, string p)
        {

            Results = new List<string>();
            Line = new LineSeries { Color = OxyColors.Red };
            double x = X.Min();
            double max = X.Max();
            double avr = X.Average();
            int count = X.Count();
            switch (_index)
            {
                case 0:
                    {
                        Title = "Linear";
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
                case 1:
                    {
                        Title = "Power";
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
                                Line.Points.Add(new DataPoint(x, regress.a * Math.Pow(x, regress.b)));
                                x += avr / count;
                            }
                        });
                    }
                    break;
                case 2:
                    {
                        Title = "Quadratic";
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
                case 3:
                    {
                        Title = "Logarithmic";
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
                case 4:
                    {
                        Title = "Hiperbolic";
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
                res = "Сильная";
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
                return "Отрицательная (умеренная)";
            }
            else
            {
                return "Отрицательная (сильная)";
            }
        }

        public void AutoAnalise()
        {
            var Regressions = new List<List<double>>();
            for (int i = 0; i < 5; i++)
            {
                Regressions.Add(new List<double>());
            }
            Results = new List<string>();
            lines = new List<LineSeries>();
            equations = new List<string>();

            {
                Line = new LineSeries { Color = OxyColors.Red };
                double x = X.Min();
                double max = X.Max();
                double avr = X.Average();
                int count = X.Count();
                var regress = new LinearRegression(X, Y);
                equations.Add(regress.Equation);
                Regressions[0].Add(regress.R);
                Regressions[1].Add(regress.R2);
                Regressions[2].Add(regress.AvrA);
                Regressions[3].Add(regress.Ffact);
                Regressions[4].Add(regress.Dfact);
                while (x < max)
                {
                    Line.Points.Add(new DataPoint(x, regress.a * x + regress.b));
                    x += avr / count;
                }
                lines.Add(Line);
            }

            {
                Line = new LineSeries { Color = OxyColors.Red };
                double x = X.Min();
                double max = X.Max();
                double avr = X.Average();
                int count = X.Count();
                var regress = new PowerRegression(X, Y);
                equations.Add(regress.Equation);
                Regressions[0].Add(regress.R);
                Regressions[1].Add(regress.R2);
                Regressions[2].Add(regress.AvrA);
                Regressions[3].Add(regress.Ffact);
                Regressions[4].Add(regress.Dfact);
                while (x < max)
                {
                    Line.Points.Add(new DataPoint(x, regress.a * Math.Pow(x, regress.b)));
                    x += avr / count;
                }
                lines.Add(Line);
            }

            {
                Line = new LineSeries { Color = OxyColors.Red };
                double x = X.Min();
                double max = X.Max();
                double avr = X.Average();
                int count = X.Count();
                var regress = new QuadraticRegression(X, Y);
                equations.Add(regress.Equation);
                Regressions[0].Add(regress.R);
                Regressions[1].Add(regress.R2);
                Regressions[2].Add(regress.AvrA);
                Regressions[3].Add(regress.Ffact);
                Regressions[4].Add(regress.Dfact);
                while (x < max)
                {
                    Line.Points.Add(new DataPoint(x, regress.a * x * x + regress.b * x + regress.c));
                    x += avr / count;
                }
                lines.Add(Line);
            }

            {
                Line = new LineSeries { Color = OxyColors.Red };
                double x = X.Min();
                double max = X.Max();
                double avr = X.Average();
                int count = X.Count();
                var regress = new LogarithmicRegression(X, Y);
                equations.Add(regress.Equation);
                Regressions[0].Add(regress.R);
                Regressions[1].Add(regress.R2);
                Regressions[2].Add(regress.AvrA);
                Regressions[3].Add(regress.Ffact);
                Regressions[4].Add(regress.Dfact);
                while (x < max)
                {
                    Line.Points.Add(new DataPoint(x, regress.a + regress.b * Math.Log(x)));
                    x += avr / count;
                }
                lines.Add(Line);
            }

            {
                Line = new LineSeries { Color = OxyColors.Red };
                double x = X.Min();
                double max = X.Max();
                double avr = X.Average();
                int count = X.Count();
                var regress = new HyperbolicRegression(X, Y);
                equations.Add(regress.Equation);
                Regressions[0].Add(regress.R);
                Regressions[1].Add(regress.R2);
                Regressions[2].Add(regress.AvrA);
                Regressions[3].Add(regress.Ffact);
                Regressions[4].Add(regress.Dfact);
                while (x < max)
                {
                    Line.Points.Add(new DataPoint(x, regress.a + regress.b / x));
                    x += avr / count;
                }
                lines.Add(Line);
            }

            bestregression = BestRegression(Regressions[0], Regressions[2]);
            TitleIndex = bestregression;
            switch (bestregression)
            {
                case 0: Title = "Linear"; break;
                case 1: Title = "Power"; break;
                case 2: Title = "Quadratic"; break;
                case 3: Title = "Logarithmic"; break;
                case 4: Title = "Hiperbolic"; break;
            }
            Results.Add(equations[bestregression]);
            Results.Add((Regressions[0])[bestregression].ToString("0.####"));
            Results.Add(AboutR((Regressions[0])[bestregression]));
            Results.Add((Regressions[1])[bestregression].ToString("0.####"));
            Results.Add((Regressions[2])[bestregression].ToString("0.####"));
            Results.Add((Regressions[3])[bestregression].ToString("0.####"));
            Results.Add((Regressions[4])[bestregression].ToString("0.####"));
            Results.Add(AboutDurbinWatson((Regressions[4])[bestregression]));
            Line = lines[bestregression];

            int BestRegression(List<double> correlationCoefficients, List<double> meanErrors)
            {
                int bestindex = -1;
                var cC = CheckData(correlationCoefficients);
                var mE = CheckData(meanErrors);
                double maxcC = correlationCoefficients.Min();
                double maxmE = meanErrors.Min();
                double max = 0;
                for (int i = 0; i < correlationCoefficients.Count; i++)
                {
                    maxcC = Squeeze(cC, cC[i]);
                    maxmE = Squeeze(mE, 100 - mE[i]);
                    if (maxcC + maxmE > max)
                    {
                        max = maxcC + maxmE;
                        bestindex = i;
                    }

                }
                return bestindex;
            }
            double Squeeze(List<double> values, double value)
            {
                value = double.Parse(value.ToString("0.#####"));
                var max = double.Parse(values.Max().ToString("0.#####"));
                return value / max;
            }
            List<double> CheckData(List<double> values)
            {
                List<double> datas = new List<double>();
                foreach (double data in values)
                {
                    datas.Add(double.IsNaN(data) ? 0 : data);
                }
                return datas;
            }
        }

    }
}
