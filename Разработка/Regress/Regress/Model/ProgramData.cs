using Regress.CSV;
using System.Collections.Generic;
using System.Data;

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
        public static DataTable datatable;
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
