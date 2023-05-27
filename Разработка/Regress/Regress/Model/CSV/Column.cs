using System.Collections.Generic;

namespace Regress.CSV
{
    internal class Column
    {
        public string Name { get; private set; }
        public List<string> Value { get; }
        public string Type { get; private set; }

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

        public void DeleteRows(List<int> index)
        {
            List<string> newValue = new List<string>();

            for (int i = 0; i < Value.Count; i++)
            {
                if (!index.Contains(i))
                {
                    newValue.Add(Value[i]);
                }
            }

            // Замена исходных списков новыми списками без удаленных элементов
            Value.Clear();
            Value.AddRange(newValue);
        }
    }
}
