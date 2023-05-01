namespace ParserCSV
{
    internal class CsvData
    {
        public Column ResultColumn { get; private set; }
        // Определяем приватное поле для хранения пути к файлу
        private string _filePath;

        // Определяем свойство для хранения списка столбцов
        public List<Column> Columns { get; private set; }

        // Определяем приватное поле для хранения списка всех данных из файла
        private List<string> _data;

        // Определяем свойство для хранения количества столбцов в файле
        public int ColumnCount { get; private set; }

        // Определяем свойство для хранения количества строк в файле
        public int RowCount { get; private set; }

        // Определяем конструктор, который принимает путь к файлу в качестве аргумента
        public CsvData(string filePath)
        {
            // Инициализируем приватное поле пути к файлу
            _filePath = filePath;

            // Вызываем метод ParseData() для заполнения списка данных и списка столбцов
            ParseData();
        }

        private bool ParseData()
        {
            // Читаем все строки из файла в список _data
            _data = File.ReadAllLines(_filePath).ToList();
            if (_data != null)
            {
                // Создаем список столбцов Columns и заполняем его объектами Column на основе первой строки файла
                Columns = _data[0].Split(',').Select(columnName => new Column(columnName)).ToList();

                if (CheckDuplicateColumnNames() == false)
                {
                    return false;
                }

                // Проходим по всем строкам файла, начиная со второй строки
                foreach (var dataRow in _data.Skip(1))
                {
                    // Разбиваем строку на массив значений, используя разделитель ","
                    var splitDataRow = dataRow.Split(',');

                    // Проходим по всем столбцам и добавляем значение текущей строки в соответствующий столбец
                    for (int i = 0; i < splitDataRow.Length; i++)
                    {
                        Columns[i].Add(splitDataRow[i]);
                    }
                }

                // Получаем количество столбцов и строк
                ColumnCount = Columns.Count;
                RowCount = _data.Count - 1; // Уменьшаем количество строк на 1, так как первая строка содержит заголовок
            }

            return true;
        }

        private bool CheckDuplicateColumnNames()
        {
            // Создаем HashSet для хранения уникальных имен столбцов
            HashSet<string> uniqueNames = new HashSet<string>();
            // Проходим по массиву объектов класса Column и добавляем их имена в HashSet. 
            // Если имя столбца уже есть в HashSet, то метод Add() вернет false, иначе добавит его в HashSet и вернет true.
            // Таким образом, если метод Add() вернул false, то имя столбца уже было добавлено в HashSet, то есть есть дубликат
            foreach (Column column in Columns)
            {
                if (!uniqueNames.Add(column.Name))
                {
                    return false;
                }
            }

            // Если метод не вернул false, значит дубликатов нет
            return true;
        }

        public Column GetColumn(string namecolumn)
        {
            // Проходим по списку Columns
            foreach (Column column in Columns)
            {
                // Если имя текущего столбца соответствует заданному имени, то возвращаем этот столбец
                if (column.Name == namecolumn)
                {
                    return column;
                }
            }
            // Если столбец с заданным именем не найден, то возвращаем null
            return null;
        }

        public List<string> GetNames()
        {
            // Создаем новый список для хранения имен столбцов
            List<string> names = new List<string>();
            // Проходим по списку Columns
            foreach (Column column in Columns)
            {
                // Добавляем имя текущего столбца в список имен
                names.Add(column.Name);
            }
            // Возвращаем список имен столбцов
            return names;
        }

        public void SetResultColumn(string namecolumn)
        {
            // Проходим по массиву Columns
            foreach (Column column in Columns)
            {
                // Если имя столбца совпадает с искомым, устанавливаем значение поля ResultColumn
                if (column.Name == namecolumn)
                {
                    ResultColumn = column;
                }
            }
        }

        public string[,] GetTable(params string[] names)
        {
            var columnnames = names.ToList();
            var columns = new List<Column>();

            foreach (Column column in Columns)
            {
                if (columnnames.Contains(column.Name))
                {
                    columns.Add(column);
                }
            }
            var csvtable = new string[RowCount, columns.Count];
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < columns.Count; j++)
                {
                    csvtable[i, j] = columns[j].Value[i];
                }
            }
            return csvtable;
        }
    }
}
