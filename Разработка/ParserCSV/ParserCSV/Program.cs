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
        var fileputh = "D:\\YandexDisk\\ЯндексДиск\\BOLOFFICIAL\\ЯГТУ\\Бакалавриат\\3 КУРС\\6 СЕМЕСТР\\ИНСТРУМЕНТАЛЬНЫЕ СРЕДСТВА ИНФОРМАЦИОННЫХ СИСТЕМ\\Курсовая работа\\test5.csv";

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

        var linear = new HyperbolicRegression(X,Y);
        
    }
}
