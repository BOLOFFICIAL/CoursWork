using ParserCSV;

internal class Program
{
    public static double kx = 0;
    public static double bx = 0;

    private static void Main(string[] args)
    {
        var fileputh = "D:\\YandexDisk\\ЯндексДиск\\BOLOFFICIAL\\ЯГТУ\\Бакалавриат\\3 КУРС\\6 СЕМЕСТР\\ИНСТРУМЕНТАЛЬНЫЕ СРЕДСТВА ИНФОРМАЦИОННЫХ СИСТЕМ\\Курсовая работа\\кор+регр.csv";

        var csv = new CsvData(fileputh);


        csv.SetResultColumn("Y");

        var X = new List<double>();

        foreach (var el in csv.GetColumn("X").Value)
        {
            X.Add(double.Parse(el.Replace(".", ",")));
        }

        var Y = new List<double>();

        foreach (var el in csv.ResultColumn.Value)
        {
            Y.Add(double.Parse(el.Replace(".", ",")));
        }

        List<string> equations = new List<string>();
        List<double> correlationCoefficients = new List<double>();
        List<double> determinationCoefficients = new List<double>();
        List<double> meanErrors = new List<double>();
        List<double> fisherCrit = new List<double>();
        List<double> DCrit = new List<double>();

        Console.WriteLine("линейной регрессии");
        {
            var linear = new LinearRegression(X, Y);
            Console.WriteLine("Уравнение регрессии: " + linear.Equation);
            equations.Add(linear.Equation);
            Console.WriteLine("Коэффициент корреляции: " + linear.R);
            correlationCoefficients.Add(linear.R);
            Console.WriteLine("Коэффициент детерминации: " + linear.R2);
            determinationCoefficients.Add(linear.R2);
            Console.WriteLine("Средняя ошибка аппроксимации: " + linear.AvrA);
            meanErrors.Add(linear.AvrA);
            Console.WriteLine("F-критерии Фишера: " + linear.Ffact);
            fisherCrit.Add(linear.Ffact);
            Console.WriteLine("Критерии Дарбина-Уотсона: " + linear.Dfact);
            DCrit.Add(linear.Dfact);
        }
        Console.WriteLine("\nстепенной регрессии");
        {
            var power = new PowerRegression(X, Y);
            Console.WriteLine("Уравнение регрессии: " + power.Equation);
            equations.Add(power.Equation);
            Console.WriteLine("Коэффициент корреляции: " + power.R);
            correlationCoefficients.Add(power.R);
            Console.WriteLine("Коэффициент детерминации: " + power.R2);
            determinationCoefficients.Add(power.R2);
            Console.WriteLine("Средняя ошибка аппроксимации: " + power.AvrA);
            meanErrors.Add(power.AvrA);
            Console.WriteLine("F-критерии Фишера: " + power.Ffact);
            fisherCrit.Add(power.Ffact);
            Console.WriteLine("Критерии Дарбина-Уотсона: " + power.Dfact);
            DCrit.Add(power.Dfact);
        }
        Console.WriteLine("\nквадратичной регрессии");
        {
            var quadratic = new QuadraticRegression(X, Y);
            Console.WriteLine("Уравнение регрессии: " + quadratic.Equation);
            equations.Add(quadratic.Equation);
            Console.WriteLine("Коэффициент корреляции: " + quadratic.R);
            correlationCoefficients.Add(quadratic.R);
            Console.WriteLine("Коэффициент детерминации: " + quadratic.R2);
            determinationCoefficients.Add(quadratic.R2);
            Console.WriteLine("Средняя ошибка аппроксимации: " + quadratic.AvrA);
            meanErrors.Add(quadratic.AvrA);
            Console.WriteLine("F-критерии Фишера: " + quadratic.Ffact);
            fisherCrit.Add(quadratic.Ffact);
            Console.WriteLine("Критерии Дарбина-Уотсона: " + quadratic.Dfact);
            DCrit.Add(quadratic.Dfact);
        }
        Console.WriteLine("\nлогарифмической регрессии");
        {
            var logarithmic = new LogarithmicRegression(X, Y);
            Console.WriteLine("Уравнение регрессии: " + logarithmic.Equation);
            equations.Add(logarithmic.Equation);
            Console.WriteLine("Коэффициент корреляции: " + logarithmic.R);
            correlationCoefficients.Add(logarithmic.R);
            Console.WriteLine("Коэффициент детерминации: " + logarithmic.R2);
            determinationCoefficients.Add(logarithmic.R2);
            Console.WriteLine("Средняя ошибка аппроксимации: " + logarithmic.AvrA);
            meanErrors.Add(logarithmic.AvrA);
            Console.WriteLine("F-критерии Фишера: " + logarithmic.Ffact);
            fisherCrit.Add(logarithmic.Ffact);
            Console.WriteLine("Критерии Дарбина-Уотсона: " + logarithmic.Dfact);
            DCrit.Add(logarithmic.Dfact);
        }
        Console.WriteLine("\nгиперболической регрессии");
        {
            var Hyperbolic = new HyperbolicRegression(X, Y);
            Console.WriteLine("Уравнение регрессии: " + Hyperbolic.Equation);
            equations.Add(Hyperbolic.Equation);
            Console.WriteLine("Коэффициент корреляции: " + Hyperbolic.R);
            correlationCoefficients.Add(Hyperbolic.R);
            Console.WriteLine("Коэффициент детерминации: " + Hyperbolic.R2);
            determinationCoefficients.Add(Hyperbolic.R2);
            Console.WriteLine("Средняя ошибка аппроксимации: " + Hyperbolic.AvrA);
            meanErrors.Add(Hyperbolic.AvrA);
            Console.WriteLine("F-критерии Фишера: " + Hyperbolic.Ffact);
            fisherCrit.Add(Hyperbolic.Ffact);
            Console.WriteLine("Критерии Дарбина-Уотсона: " + Hyperbolic.Dfact);
            DCrit.Add(Hyperbolic.Dfact);
        }

        Console.WriteLine("\n"+ BestRegression(equations, correlationCoefficients, determinationCoefficients, meanErrors, fisherCrit));
    }

    public static string BestRegression(List<string> equations, List<double> correlationCoefficients, List<double> determinationCoefficients, List<double> meanErrors, List<double> fisherCrit)
    {
        int bestIndex = 0;
        double bestFisherCrit = 0;
        for (int i = 0; i < equations.Count; i++)
        {
            if (correlationCoefficients[i] > 0.9 && determinationCoefficients[i] > 0.8 && meanErrors[i] < 10)
            {
                if (fisherCrit[i] > bestFisherCrit)
                {
                    bestIndex = i;
                    bestFisherCrit = fisherCrit[i];
                }
            }
        }
        return equations[bestIndex];
    }
}
