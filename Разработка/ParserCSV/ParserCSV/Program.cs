using ParserCSV;

internal class Program
{
    private static void Main(string[] args)
    {;
        var fileputh = "D:\\YandexDisk\\ЯндексДиск\\BOLOFFICIAL\\ЯГТУ\\Бакалавриат\\3 КУРС\\6 СЕМЕСТР\\ИНСТРУМЕНТАЛЬНЫЕ СРЕДСТВА ИНФОРМАЦИОННЫХ СИСТЕМ\\Курсовая работа\\archive\\google-stock-dataset-Monthly.csv";

        var csv = new CsvData(fileputh);
        csv.SetResultColumn("Bolofficial");

        Console.WriteLine(csv.ResultColumn.Name);
    }
}