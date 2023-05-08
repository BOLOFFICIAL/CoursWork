namespace ParserCSV
{
    internal class LinearRegression
    {
        public List<double> X { get; private set; }
        public double SumX { get; private set; }
        public double AvrX { get; private set; }
        public List<double> XAvrX { get; private set; }
        public List<double> XAvrX2 { get; private set; }
        public double SumXAvrX2 { get; private set; }

        public List<double> Y { get; private set; }
        public double SumY { get; private set; }
        public List<double> SettlementY { get; private set; }

        public List<double> XY { get; private set; }
        public double SumXY { get; private set; }

        public List<double> X2 { get; private set; }
        public double SumX2 { get; private set; }

        public List<double> Y2 { get; private set; }
        public double SumY2 { get; private set; }

        public List<double> E { get; private set; }
        public List<double> DE { get; private set; }
        public List<double> DE2 { get; private set; }
        public double SumDE2 { get; private set; }
        public List<double> E2 { get; private set; }
        public double SumE2 { get; private set; }

        public List<double> A { get; private set; }
        public double SumA { get; private set; }

        public double AvrA { get; private set; }

        public int Count { get; private set; }
        public double a { get; private set; }
        public double ma { get; private set; }
        public double ta { get; private set; }

        public double b { get; private set; }
        public double mb { get; private set; }
        public double tb { get; private set; }

        public double R { get; private set; }
        public double mR { get; private set; }
        public double tR { get; private set; }

        public double R2 { get; private set; }
        public double Ffact { get; private set; }
        public double Dfact { get; private set; }

        public LinearRegression(List<double> x, List<double> y)
        {
            X = x;
            Y = y;
            AuxiliaryValues();
            LinearRegressionEquations();
            LinearPairCorrelation();
            ApproximationError();
            Fischer();
            RandomParameterError();
            Studentstatistics();
            DurbinWatsonCriteria();
        }
        private void AuxiliaryValues()
        {
            AvrX = X.Average();
            XY = new List<double>();
            X2 = new List<double>();
            Y2 = new List<double>();
            XAvrX = new List<double>();
            XAvrX2 = new List<double>();
            Count = X.Count;
            for (int i = 0; i < Count; i++)
            {
                XY.Add(X[i] * Y[i]);
                X2.Add(X[i] * X[i]);
                Y2.Add(Y[i] * Y[i]);
                XAvrX.Add(X[i] - AvrX);
                XAvrX2.Add(XAvrX[i] * XAvrX[i]);
            }
            SumXAvrX2 = XAvrX2.Sum();
            SumX = X.Sum();
            SumY = Y.Sum();
            SumXY = XY.Sum();
            SumX2 = X2.Sum();
            SumY2 = Y2.Sum();
        }
        private void LinearRegressionEquations()
        {
            a = (SumX * SumY - Count * SumXY) / (SumX * SumX - Count * SumX2);
            b = (SumX * SumXY - SumX2 * SumY) / (SumX * SumX - Count * SumX2);
            SettlementY = new List<double>();
            E = new List<double>();
            E2 = new List<double>();
            A = new List<double>();
            DE = new List<double>();
            DE2 = new List<double>();
            for (int i = 0; i < Count; i++)
            {
                SettlementY.Add(a * X[i] + b);
                E.Add(Y[i] - SettlementY[i]);
                E2.Add(E[i] * E[i]);
                A.Add(Math.Abs((E[i]) / (Y[i])));
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
            SumE2 = E2.Sum();
            SumA = A.Sum();
            SumDE2 = DE2.Sum();
        }
        private void LinearPairCorrelation()
        {
            R = (Count * SumXY - SumX * SumY) / (Math.Sqrt((Count * SumX2 - SumX * SumX) * (Count * SumY2 - SumY * SumY)));
            R2 = R * R;
        }
        private void ApproximationError()
        {
            AvrA = ((SumA / Count)) * 100;
        }
        public void Fischer()
        {
            Ffact = R2 / (1 - R2) * (Count - 2);
        }
        public void RandomParameterError()
        {
            ma = Math.Sqrt((1) / (SumXAvrX2) * (SumE2) / (Count - 2));
            mb = Math.Sqrt((SumE2) / (Count - 2) * (SumX2) / (Count * SumXAvrX2));
            mR = Math.Sqrt((1 - R2) / (Count - 2));
        }
        public void Studentstatistics()
        {
            ta = a / ma;
            tb = b / mb;
            tR = R / mR;
        }
        public void DurbinWatsonCriteria()
        {
            Dfact = SumDE2 / SumE2;
        }
    }
}
