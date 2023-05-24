using Regress.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public static List<string> ErrorX;
        public static List<string> ErrorY;
        public ProgramData(string _fileputh) 
        {
            fileputh = _fileputh;
            csv = null;
            resultcolumn = "";
            parametercolumn = "";
            X = null;
            Y = null;
            ErrorX = null;
            ErrorY = null;
            regressionindex = -1;
        }
    }
}
