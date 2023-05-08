using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
