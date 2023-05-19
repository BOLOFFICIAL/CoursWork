using OxyPlot;
using OxyPlot.Series;
using Regress.CSV;
using Regress.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Regress
{
    internal class CorrelationRegression
    {
        public LineSeries Line { get; private set; }
        public int TitleIndex { get; private set; }
        private int _index;
        private int _bestregression;
        public List<LineSeries> lines;
        private List<string> _equations;
        public string Title { get; private set; }
        public List<string> Results { get; private set; }

        public CorrelationRegression(int index, bool auto)
        {
            _index = index;
            if (auto) { AutoAnalise(); }
            else { Analise(); }
        }

        public async Task Analise()
        {
            Results = new List<string>();
            Line = new LineSeries { Color = OxyColors.Red };
            double x = ProgramData.X.Min();
            double max = ProgramData.X.Max();
            double avr = ProgramData.X.Average();
            int count = ProgramData.X.Count();
            switch (_index)
            {
                case 0:
                    {
                        Title = "Linear";
                        var regress = new LinearRegression(ProgramData.X, ProgramData.Y);
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
                            foreach (var x in ProgramData.X) 
                            {
                                Line.Points.Add(new DataPoint(x, regress.a * x + regress.b));
                            }
                        });
                    }
                    break;
                case 1:
                    {
                        Title = "Power";
                        var regress = new PowerRegression(ProgramData.X, ProgramData.Y);
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
                            foreach (var x in ProgramData.X)
                            {
                                Line.Points.Add(new DataPoint(x, regress.a * Math.Pow(x, regress.b)));
                            }
                        });
                    }
                    break;
                case 2:
                    {
                        Title = "Quadratic";
                        var regress = new QuadraticRegression(ProgramData.X, ProgramData.Y);
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
                            foreach (var x in ProgramData.X)
                            {
                                Line.Points.Add(new DataPoint(x, regress.a * x * x + regress.b * x + regress.c));
                            }
                        });
                    }
                    break;
                case 3:
                    {
                        Title = "Logarithmic";
                        var regress = new LogarithmicRegression(ProgramData.X, ProgramData.Y);
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
                            foreach (var x in ProgramData.X)
                            {
                                Line.Points.Add(new DataPoint(x, regress.a + regress.b * Math.Log(x)));
                            }
                        });
                    }
                    break;
                case 4:
                    {
                        Title = "Hiperbolic";
                        var regress = new HyperbolicRegression(ProgramData.X, ProgramData.Y);
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
                            foreach (var x in ProgramData.X)
                            {
                                Line.Points.Add(new DataPoint(x, regress.a + regress.b / x));
                            }
                        });
                    }
                    break;
            }
        }

        public void AutoAnalise()
        {
            var Regressions = new List<List<double>>();
            for (var i = 0; i < 5; i++)
            {
                Regressions.Add(new List<double>());
            }

            Results = new List<string>();
            lines = new List<LineSeries>();
            _equations = new List<string>();

            {
                Line = new LineSeries { Color = OxyColors.Red };
                var regress = new LinearRegression(ProgramData.X, ProgramData.Y);
                _equations.Add(regress.Equation);
                Regressions[0].Add(regress.R);
                Regressions[1].Add(regress.R2);
                Regressions[2].Add(regress.AvrA);
                Regressions[3].Add(regress.Ffact);
                Regressions[4].Add(regress.Dfact);
                for (int i = 0; i < ProgramData.X.Count; i++)
                {
                    Line.Points.Add(new DataPoint(ProgramData.X[i], regress.a * ProgramData.X[i] + regress.b));
                }
                lines.Add(Line);
            }

            {
                Line = new LineSeries { Color = OxyColors.Red };
                var regress = new PowerRegression(ProgramData.X, ProgramData.Y);
                _equations.Add(regress.Equation);
                Regressions[0].Add(regress.R);
                Regressions[1].Add(regress.R2);
                Regressions[2].Add(regress.AvrA);
                Regressions[3].Add(regress.Ffact);
                Regressions[4].Add(regress.Dfact);
                for (int i = 0; i < ProgramData.X.Count; i++)
                {
                    Line.Points.Add(new DataPoint(ProgramData.X[i], regress.a * Math.Pow(ProgramData.X[i], regress.b)));
                }
                lines.Add(Line);
            }

            {
                Line = new LineSeries { Color = OxyColors.Red };
                var regress = new QuadraticRegression(ProgramData.X, ProgramData.Y);
                _equations.Add(regress.Equation);
                Regressions[0].Add(regress.R);
                Regressions[1].Add(regress.R2);
                Regressions[2].Add(regress.AvrA);
                Regressions[3].Add(regress.Ffact);
                Regressions[4].Add(regress.Dfact);
                for (int i = 0; i < ProgramData.X.Count; i++)
                {
                    Line.Points.Add(new DataPoint(ProgramData.X[i], regress.a * ProgramData.X[i] * ProgramData.X[i] + regress.b * ProgramData.X[i] + regress.c));
                }
                lines.Add(Line);
            }

            {
                Line = new LineSeries { Color = OxyColors.Red };
                var regress = new LogarithmicRegression(ProgramData.X, ProgramData.Y);
                _equations.Add(regress.Equation);
                Regressions[0].Add(regress.R);
                Regressions[1].Add(regress.R2);
                Regressions[2].Add(regress.AvrA);
                Regressions[3].Add(regress.Ffact);
                Regressions[4].Add(regress.Dfact);
                for (int i = 0; i < ProgramData.X.Count; i++)
                {
                    Line.Points.Add(new DataPoint(ProgramData.X[i], regress.a + regress.b * Math.Log(ProgramData.X[i])));
                }
                lines.Add(Line);
            }

            {
                Line = new LineSeries { Color = OxyColors.Red };
                var regress = new HyperbolicRegression(ProgramData.X, ProgramData.Y);
                _equations.Add(regress.Equation);
                Regressions[0].Add(regress.R);
                Regressions[1].Add(regress.R2);
                Regressions[2].Add(regress.AvrA);
                Regressions[3].Add(regress.Ffact);
                Regressions[4].Add(regress.Dfact);
                for (int i = 0; i < ProgramData.X.Count; i++)
                {
                    Line.Points.Add(new DataPoint(ProgramData.X[i], regress.a + regress.b / ProgramData.X[i]));
                }
                lines.Add(Line);
            }


            _bestregression = BestRegression(Regressions[0], Regressions[2]);

            TitleIndex = _bestregression;

            Title = _bestregression switch
            {
                0 => "Linear",
                1 => "Power",
                2 => "Quadratic",
                3 => "Logarithmic",
                4 => "Hiperbolic",
                _ => Title // Handle unexpected case, if necessary
            };

            Results.AddRange(new[]
            {
                    _equations[_bestregression],
                    (Regressions[0])[_bestregression].ToString("0.####"),
                    AboutR((Regressions[0])[_bestregression]),
                    (Regressions[1])[_bestregression].ToString("0.####"),
                    (Regressions[2])[_bestregression].ToString("0.####"),
                    (Regressions[3])[_bestregression].ToString("0.####"),
                    (Regressions[4])[_bestregression].ToString("0.####"),
                    AboutDurbinWatson((Regressions[4])[_bestregression])
                });

            Line = lines[_bestregression];

            int BestRegression(List<double> correlationCoefficients, List<double> meanErrors)
            {
                var bestindex = -1;
                var cC = CheckData(correlationCoefficients);
                var mE = CheckData(meanErrors);
                var maxcC = correlationCoefficients.Min();
                var maxmE = meanErrors.Min();
                var max = 0.0;
                for (var i = 0; i < correlationCoefficients.Count; i++)
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

                double Squeeze(List<double> values, double value)
                {
                    value = double.Parse(value.ToString("0.#####"));
                    var max = double.Parse(values.Max().ToString("0.#####"));
                    return value / max;
                }
                List<double> CheckData(List<double> values)
                {
                    var datas = new List<double>();
                    foreach (var data in values)
                    {
                        datas.Add(double.IsNaN(data) ? 0 : data);
                    }
                    return datas;
                }
            }
        }

        private string AboutR(double r)
        {
            var res = "";
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
                res = "Очень слабая";
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
    }
}
