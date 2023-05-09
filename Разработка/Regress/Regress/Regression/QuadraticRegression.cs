using System;
using System.Collections.Generic;
using System.Linq;

namespace Regress
{
    internal class QuadraticRegression
    {
        public List<double> X { get; private set; }
        public double SumX { get; private set; }
        public List<double> X2 { get; private set; }
        public double SumX2 { get; private set; }
        public List<double> X3 { get; private set; }
        public double SumX3 { get; private set; }
        public List<double> X4 { get; private set; }
        public double SumX4 { get; private set; }

        public List<double> Y { get; private set; }
        public double AvrY { get; private set; }
        public double SumY { get; private set; }
        public List<double> YAvrY { get; private set; }
        public List<double> YAvrY2 { get; private set; }
        public double SumYAvrY2 { get; private set; }
        public List<double> SettlementY { get; private set; }

        public List<double> XY { get; private set; }
        public double SumXY { get; private set; }
        public List<double> X2Y { get; private set; }
        public double SumX2Y { get; private set; }

        public List<double> E { get; private set; }
        public List<double> E2 { get; private set; }
        public double SumE2 { get; private set; }
        public List<double> DE { get; private set; }
        public List<double> DE2 { get; private set; }
        public double SumDE2 { get; private set; }

        public List<double> A { get; private set; }
        public double SumA { get; private set; }
        public int Count { get; private set; }

        public double a { get; private set; }
        public double b { get; private set; }
        public double c { get; private set; }

        public double R { get; private set; }
        public double R2 { get; private set; }
        public double AvrA { get; private set; }
        public double Ffact { get; private set; }
        public double Dfact { get; private set; }

        public string Equation { get; private set; }

        public QuadraticRegression(List<double> x, List<double> y)
        {
            X = x;
            Y = y;
            AuxiliaryValues();
            QuadraticRegressionEquations();
            PowerPairCorrelation();
            ApproximationError();
            Fischer();
            DurbinWatsonCriteria();
        }
        private void AuxiliaryValues()
        {
            Count = X.Count();
            X2 = new List<double>();
            X3 = new List<double>();
            X4 = new List<double>();
            XY = new List<double>();
            X2Y = new List<double>();
            for (int i = 0; i < Count; i++)
            {
                X2.Add(X[i] * X[i]);
                X3.Add(X2[i] * X[i]);
                X4.Add(X3[i] * X[i]);
                XY.Add(X[i] * Y[i]);
                X2Y.Add(X2[i] * Y[i]);
            }
            SumX = X.Sum();
            SumY = Y.Sum();
            SumX2 = X2.Sum();
            SumX3 = X3.Sum();
            SumX4 = X4.Sum();
            SumXY = XY.Sum();
            SumX2Y = X2Y.Sum();
        }

        private void QuadraticRegressionEquations()
        {
            double[,] Xs = { { SumX2, SumX, Count }, { SumX3, SumX2, SumX }, { SumX4, SumX3, SumX2 } };
            double[] Ys = { SumY, SumXY, SumX2Y };
            var equations = GaussJordan(Xs, Ys);

            a = equations[0];
            b = equations[1];
            c = equations[2];

            Equation = $"Y = {a.ToString("0.####")}X^2 + {b.ToString("0.####")}X + {c.ToString("0.####")}";

            AvrY = Y.Average();
            SettlementY = new List<double>();
            YAvrY = new List<double>();
            YAvrY2 = new List<double>();
            E = new List<double>();
            E2 = new List<double>();
            DE = new List<double>();
            DE2 = new List<double>();
            A = new List<double>();

            for (int i = 0; i < Count; i++)
            {
                SettlementY.Add(a * X2[i] + b * X[i] + c);
                YAvrY.Add(Y[i] - AvrY);
                YAvrY2.Add(YAvrY[i] * YAvrY[i]);
                E.Add(Y[i] - SettlementY[i]);
                E2.Add(E[i] * E[i]);
                A.Add(Math.Abs(E[i] / Y[i]));
                if (i > 0)
                {
                    DE.Add(E[i] - E[i - 1]);
                }
                else
                {
                    DE.Add(0);
                }
                DE2.Add(DE[i] * DE[i]);
            }

            SumYAvrY2 = YAvrY2.Sum();
            SumE2 = E2.Sum();
            SumDE2 = DE2.Sum();
            SumA = A.Sum();

            double[] GaussJordan(double[,] A, double[] b)
            {
                int n = b.Length;
                double[] x = new double[n];
                double[,] Augmented = new double[n, n + 1];

                // Создаем расширенную матрицу
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        Augmented[i, j] = A[i, j];
                    }
                    Augmented[i, n] = b[i];
                }

                // Прямой ход метода Гаусса-Джордана
                for (int k = 0; k < n; k++)
                {
                    double max = Math.Abs(Augmented[k, k]);
                    int maxIndex = k;

                    // Находим максимальный элемент в столбце k
                    for (int i = k + 1; i < n; i++)
                    {
                        if (Math.Abs(Augmented[i, k]) > max)
                        {
                            max = Math.Abs(Augmented[i, k]);
                            maxIndex = i;
                        }
                    }

                    // Обменять строки местами
                    for (int i = k; i < n + 1; i++)
                    {
                        double temp = Augmented[k, i];
                        Augmented[k, i] = Augmented[maxIndex, i];
                        Augmented[maxIndex, i] = temp;
                    }

                    // Приводим к треугольному виду
                    for (int i = k + 1; i < n; i++)
                    {
                        double factor = Augmented[i, k] / Augmented[k, k];
                        for (int j = k + 1; j < n + 1; j++)
                        {
                            Augmented[i, j] = Augmented[i, j] - factor * Augmented[k, j];
                        }
                        Augmented[i, k] = 0;
                    }
                }

                // Обратный ход метода Гаусса-Джордана
                for (int i = n - 1; i >= 0; i--)
                {
                    double sum = 0;
                    for (int j = i + 1; j < n; j++)
                    {
                        sum += Augmented[i, j] * x[j];
                    }
                    x[i] = (Augmented[i, n] - sum) / Augmented[i, i];
                }

                return x;
            }
        }

        private void PowerPairCorrelation()
        {
            R = Math.Sqrt(1.0001 - ((SumE2) / (SumYAvrY2)));
            R2 = R * R;
        }

        private void ApproximationError()
        {
            AvrA = (SumA) / Count * 100;
        }
        public void Fischer()
        {
            Ffact = R2 / (1.0001 - R2) * ((double)(Count - 3)/2);
        }
        public void DurbinWatsonCriteria()
        {
            Dfact = SumDE2 / SumE2;
        }
    }
}
