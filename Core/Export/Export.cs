using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Export
{
    public class Export :IExport
    {
        private readonly List<List<string>> _exportData;
        List<string> _attributes=new List<string>();
        List<string> _objects=new List<string>();
        List<List<byte>> _context=new List<List<byte>>();

        public Export(List<List<string>> exportData)
        {
            _exportData = exportData;
            Parse();
        }

        public void Parse()
        {
            _attributes.AddRange(_exportData[0].GetRange(1,_exportData[0].Count-1));
            for (int i = 1; i < _exportData.Count; i++)
            {List<byte> row=new List<byte>();
                for (int j = 1; j < _exportData[i].Count; j++)
                {
                    byte value = 0;
                    if (byte.TryParse(_exportData[i][j], out value))
                    {                        
                        row.Add(value);
                    }
                    else
                        throw new AggregateException($"Parse error at {i} line pos {j} symbol <{_exportData[i][j]}> not byte");
                }
                _context.Add(row);
                _objects.Add(_exportData[i][0]);
            }
        }

        public List<string> GetAttributes()
        {
            return _attributes;
        }

        public List<List<byte>> GetContext()
        {
            return _context;
        }

        public List<string> GetObjects()
        {
            return _objects;
        }

        public byte[,] ToMatrix()
        {            
            byte[,] matrix=new byte[_context.Count,_context[0].Count];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = _context[i][j];
                }
            }
            return matrix;
        }
    }
}
