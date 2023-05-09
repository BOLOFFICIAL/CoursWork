using System;
using System.Collections.Generic;
using System.Linq;

namespace Regress
{
    internal class LogarithmicRegression
    {
        public List<double> X { get; private set; }
        public double SumX { get; private set; }
        public List<double> lnX { get; private set; }
        public double SumlnX { get; private set; }
        public List<double> lnX2 { get; private set; }
        public List<double> YlnX { get; private set; }
        public double SumlnX2 { get; private set; }
        public double SumYlnX { get; private set; }
        public List<double> Y { get; private set; }
        public double AvrY { get; private set; }
        public double SumY { get; private set; }
        public List<double> SettlementY { get; private set; }

        public List<double> YAvrY { get; private set; }
        public List<double> YAvrY2 { get; private set; }
        public double SumYAvrY2 { get; private set; }

        public List<double> E { get; private set; }
        public List<double> E2 { get; private set; }
        public double SumE2 { get; private set; }
        public List<double> DE { get; private set; }
        public List<double> DE2 { get; private set; }
        public double SumDE2 { get; private set; }
        public List<double> A { get; private set; }
        public double SumA { get; private set; }
        public double AvrA { get; private set; }

        public double a { get; private set; }
        public double b { get; private set; }

        public int Count { get; private set; }

        public double R { get; private set; }
        public double R2 { get; private set; }
        public double Ffact { get; private set; }
        public double Dfact { get; private set; }

        public string Equation { get; private set; }

        public LogarithmicRegression(List<double> x, List<double> y)
        {
            X = x;
            Y = y;
            AuxiliaryValues();
            LogarithmicRegressionEquations();
            PowerPairCorrelation();
            ApproximationError();
            Fischer();
            DurbinWatsonCriteria();
        }

        private void AuxiliaryValues()
        {
            Count = X.Count();
            lnX = new List<double>();
            lnX2 = new List<double>();
            YlnX = new List<double>();
            for (int i = 0; i < Count; i++)
            {
                lnX.Add(Math.Log(X[i]));
                lnX2.Add(lnX[i] * lnX[i]);
                YlnX.Add(Y[i] * lnX[i]);
            }

            SumX = X.Sum();
            SumY = Y.Sum();
            AvrY = Y.Average();
            SumlnX = lnX.Sum();
            SumlnX2 = lnX2.Sum();
            SumYlnX = YlnX.Sum();
        }

        private void LogarithmicRegressionEquations()
        {
            b = (Count * SumYlnX - SumlnX * SumY) / (Count * SumlnX2 - SumlnX * SumlnX);
            a = SumY / Count - b / Count * SumlnX;
            Equation = $"Y = {a.ToString("0.####")} + {b.ToString("0.####")}lnX";
            SettlementY = new List<double>();
            YAvrY = new List<double>();
            YAvrY2 = new List<double>();
            E = new List<double>();
            E2 = new List<double>();
            A = new List<double>();
            DE = new List<double>();
            DE2 = new List<double>();

            for (int i = 0; i < Count; i++)
            {
                SettlementY.Add(a + b * Math.Log(X[i]));
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
            SumA = A.Sum();
            SumDE2 = DE2.Sum();
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
            Ffact = R2 / (1.0001 - R2) * ((double)(Count - 2) / 1);
        }

        public void DurbinWatsonCriteria()
        {
            Dfact = SumDE2 / SumE2;
        }
    }
}
