using Regress.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Regress.Model
{
    class ProgramData
    {
        public static string fileputh = "";
        public static CsvData csv;
        public static string resultcolumn = "";
        public static string parametercolumn = "";
        public static int regressionindex = -1;
        public static List<double> X;
        public static List<double> Y;
        public ProgramData(string _fileputh) 
        {
            fileputh = _fileputh;
            csv = null;
            resultcolumn = "";
            parametercolumn = "";
            X = null;
            Y = null;
            regressionindex = -1;
        }
    }
}
