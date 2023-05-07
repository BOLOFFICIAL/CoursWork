using Accord.Statistics.Distributions.Univariate;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;
using ParserCSV;
using System;
using static System.Formats.Asn1.AsnWriter;

internal class Program
{
    public static double kx = 0;
    public static double bx = 0;

    private static void Main(string[] args)
    {
        var fileputh = "D:\\YandexDisk\\ЯндексДиск\\BOLOFFICIAL\\ЯГТУ\\Бакалавриат\\3 КУРС\\6 СЕМЕСТР\\ИНСТРУМЕНТАЛЬНЫЕ СРЕДСТВА ИНФОРМАЦИОННЫХ СИСТЕМ\\Курсовая работа\\test.csv";

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

        LinearRegression linear = new LinearRegression(X,Y);
        Console.WriteLine($"Уравнение: Y = {linear.a.ToString("#.##")}x+{linear.b.ToString("#.##")}");
        Console.WriteLine($"Коэффициент корреляции: {linear.R.ToString("#.##")}");
        Console.WriteLine($"Коэффициент детерминации: {linear.R2.ToString("#.##")}");
        Console.WriteLine($"Средняя ошибка аппроксимации: {linear.AvrA.ToString("#.##")}");
        Console.WriteLine($"F-критерии Фишера: {linear.Ffact.ToString("#.##")}");
        Console.WriteLine($"Случайная ошибка параметра a: {linear.ma.ToString("#.##")}");
        Console.WriteLine($"Случайная ошибка параметра b: {linear.mb.ToString("#.##")}");
        Console.WriteLine($"Случайная ошибка параметра R: {linear.mR.ToString("#.##")}");
        Console.WriteLine($"t-статистики Стьюдента параметра a: {linear.ta.ToString("#.##")}");
        Console.WriteLine($"t-статистики Стьюдента параметра b: {linear.tb.ToString("#.##")}");
        Console.WriteLine($"t-статистики Стьюдента параметра R: {linear.tR.ToString("#.##")}");
        Console.WriteLine($"Критерии Дарбина-Уотсона: {linear.Dfact.ToString("#.##")}");
    }
}
