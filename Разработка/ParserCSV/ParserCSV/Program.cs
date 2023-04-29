internal class Program
{
    private static void Main(string[] args)
    {
        Console.Write("Укажите путь до файла .csv: ");
        var fileputh = Console.ReadLine();
        fileputh = "D:\\YandexDisk\\ЯндексДиск\\BOLOFFICIAL\\ЯГТУ\\Бакалавриат\\3 КУРС\\6 СЕМЕСТР\\ИНСТРУМЕНТАЛЬНЫЕ СРЕДСТВА ИНФОРМАЦИОННЫХ СИСТЕМ\\Курсовая работа\\archive\\google-stock-dataset-Monthly.csv";
        Console.WriteLine($"Начинается процесс парсинга файла {Path.GetFileName(fileputh)}");
        var lines = File.ReadAllLines(fileputh).ToList();

        var columns = new List<Column>();

        var first = lines[0].Split(",");

        foreach (var word in first)
        {
            columns.Add(new Column(word));
        }

        var content = lines.Skip(1).ToList();

        foreach (var line in content)
        {
            var splitline = line.Split(',');
            for (int i = 0; i < splitline.Length; i++)
            {
                columns[i].Add(splitline[i]);
            }
        }

        NewMethod(columns, 0);
        NewMethod(columns, 1);
        NewMethod(columns, 2);
        NewMethod(columns, 3);
        NewMethod(columns, 4);
        NewMethod(columns, 5);

    }

    private static void NewMethod(List<Column> columns, int index)
    {
        Console.WriteLine(columns[index].Name + ":\n");

        foreach (var word in columns[index].Value)
        {
            Console.WriteLine(word);
        }
        Console.WriteLine();
    }
}

internal class Column
{
    public string Name { get; private set; }
    public List<string> Value { get;}

    public Column(string name)
    {
        Name = name;
        Value = new List<string>();
    }

    public void Add(string value)
    {
        Value.Add(value);
    }

    public void RenameColumn(string name)
    {
        Name = name;
    }
}