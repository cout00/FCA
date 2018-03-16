using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Export;
using Core.Import;
using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet;

namespace Shell
{
    internal static class Extensions
    {
        public static void Load(this Worksheet self, IImport import)
        {
            self.Cells[0, 0].Value = string.Empty;
            var attributes = import.GetAttributes();
            for (int i = 0; i < attributes.Count; i++)
            {
                self.Cells[0, i + 1].Value = attributes[i];
            }
            var obj = import.GetObjects();
            for (int i = 0; i < obj.Count; i++)
            {
                self.Cells[i + 1, 0].Value = obj[i];
            }
            var context = import.GetContext();
            for (int i = 0; i < context.Count; i++)
            {
                for (int j = 0; j < context[i].Count; j++)
                {
                    self.Cells[i + 1, j + 1].Value = context[i][j];
                }
            }
        }

        public static IExport Export(this Worksheet self)
        {
            var posRow = 0;
            var posCol = 0;
            List<List<string>> data = new List<List<string>>();
            while ((posRow == 0 & posCol == 0) || (self.Cells[posRow, posCol].Value).TextValue != null|| (self.Cells[posRow, posCol].Value).IsNumeric)
            {
                List<string> rowData = new List<string>();
                while ((posRow == 0 & posCol ==0) || (self.Cells[posRow, posCol].Value).TextValue != null || (self.Cells[posRow, posCol].Value).IsNumeric)
                {
                    rowData.Add((self.Cells[posRow, posCol].Value).ToString());
                    posCol++;
                }
                data.Add(rowData);
                posRow++;
                posCol = 0;
            }
            return new Export(data);
        }


    }
}
