using MathNet.Numerics.LinearRegression;
using ParserCSV;
using System;
using MathNet.Numerics;
using System.Linq;
using MathNet.Numerics.Statistics;
using System.Formats.Asn1;
using CsvHelper;
using Accord.Statistics.Models.Regression.Linear;

internal class Program
{
    private static void Main(string[] args)
    {;
        var fileputh = "D:\\YandexDisk\\ЯндексДиск\\BOLOFFICIAL\\ЯГТУ\\Бакалавриат\\3 КУРС\\6 СЕМЕСТР\\ИНСТРУМЕНТАЛЬНЫЕ СРЕДСТВА ИНФОРМАЦИОННЫХ СИСТЕМ\\Курсовая работа\\archive\\google-stock-dataset-Monthly.csv";

        var csv = new CsvData(fileputh);

        //var tmp = csv.GetTable();

        csv.SetResultColumn("Low");

        var tmp = csv.GetColumn("Low").Value;

        foreach (var item in tmp) 
        {
            Console.WriteLine(item);
        }
    }
}