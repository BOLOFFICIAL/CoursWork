using MathNet.Numerics.LinearRegression;
using ParserCSV;

internal class Program
{
    public static double maxcorrelation = -1;
    public static string name = "";
    public static double kx = 0;
    public static double b = 0;

    private static void Main(string[] args)
    {


        //var fileputh = "C:\\Users\\Bolofficial\\Desktop\\Cancer_Data.csv";
        var fileputh = "C:\\Users\\Bolofficial\\Desktop\\1213.csv";

        var csv = new CsvData(fileputh);
        //csv.SetResultColumn("\"diagnosis\"");
        csv.SetResultColumn("Y");
        var names = new List<string>();
        foreach (var column in csv.Columns)
        {
            if (column.Name == csv.ResultColumn.Name || column.Name == "\"id\"") { continue; }
            CoRR(csv, column.Name);
        }

        Console.WriteLine(maxcorrelation + "\t" + name + "\t" +kx + "\t" +b);

        var bestcolomn = csv.GetColumn(name);
        for (int i = 0; i < bestcolomn.Value.Count; i++)
        {
            var Y = double.Parse(csv.ResultColumn.Value[i].Replace(".", ","));
            var Yp = (b * double.Parse(bestcolomn.Value[i].Replace(".", ",")) + kx);
            var err = ((Math.Abs(Y - Yp)) / Y);
            Console.WriteLine(Y + "\t" + Yp + "\t" + err);
        }
        //Коэффициент корреляции: 0,033387149827745176
        //Уравнение регрессии: y = 139,046x + 0,4100000000000004
        //Коэффициент корреляции: 0,6012663889650808
        //Уравнение регрессии: y = 122,79170287024216x + 0,19946086267809485
        //Коэффициент корреляции: -0,8492319893023778
        //Уравнение регрессии: y = 271,42773716755585x + -0,5461463357553197
        //Коэффициент корреляции: -0,023299412170348775
        //Уравнение регрессии: y = 140,76240525749353x + -0,013256795127201113
        //Коэффициент корреляции: 0,027521135185239604
        //Уравнение регрессии: y = 139,85436893203882x + 0,354368932038835
        //Коэффициент корреляции: 0,8950121561600756
        //Уравнение регрессии: y = 109,82090967849432x + 0,38766527083268487
        //Коэффициент корреляции: -0,8509431530425969
        //Уравнение регрессии: y = 273,7005018067544x + -0,5560823229818325
        //Коэффициент корреляции: 0,6428770491572463
        //Уравнение регрессии: y = -23,081249999999983x + 9,392708333333331


        //Коэффициент корреляции: 0,7935660171412694
        //Уравнение регрессии: y = -0,29696788422769504x + 5,8421902871415154
    }

    private static void CoRR(CsvData csv, string name)
    {
        var x = csv.GetColumn(name).Value;
        double[] xint = new double[x.Count];
        for (int i = 0; i < x.Count; i++)
        {
            xint[i] = double.Parse(x[i].Replace(".", ","));
        }
        var y = csv.ResultColumn.Value;
        double[] yint = new double[y.Count];
        for (int i = 0; i < y.Count; i++)
        {
            yint[i] = double.Parse(y[i].Replace(".", ","));
        }
        Correlation(xint, yint, name);
        Console.WriteLine();
    }

    private static void Correlation(double[] x, double[] y, string namecolumn)
    {
        try
        {
            double correlation = MathNet.Numerics.Statistics.Correlation.Pearson(x, y);
            var regression = SimpleRegression.Fit(x, y);
            if (correlation > maxcorrelation)
            {
                maxcorrelation = correlation;
                name = namecolumn;
                kx = regression.Item1;
                b = regression.Item2;
            }
            Console.WriteLine("Колонка: " + namecolumn);
            Console.WriteLine("Коэффициент корреляции: " + correlation);
            Console.WriteLine("Уравнение регрессии: y = " + regression.Item1 + "x + " + regression.Item2);
        }
        catch (Exception)
        {
            Console.WriteLine("\n\nОшибка в Колонке: " + namecolumn + "\n\n");
        }

    }
}
