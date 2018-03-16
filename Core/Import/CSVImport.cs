using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace Core.Import
{
    public class CSVImport :IImport
    {
        private readonly string _path;
        private readonly string _delimeter;
        List<string> attributes = new List<string>();
        List<string> objects = new List<string>();
        private List<List<byte>> context;

        public CSVImport(string path, string delimeter = ",")
        {
            _path = path;
            _delimeter = delimeter;
            Parse();
        }

        void Parse()
        {
            using (TextFieldParser parser = new TextFieldParser(_path, Encoding.UTF8))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(_delimeter);
                var needHeader = true;
                context = new List<List<byte>>();
                while (!parser.EndOfData)
                {                    
                    var fields = parser.ReadFields();
                    if (fields.Length == 1)
                        throw new AggregateException("Parse error");
                    List<byte> row = new List<byte>();
                    for (int i = 1; i < fields.Length; i++)
                    {
                        if (needHeader)
                            attributes.Add(fields[i]);
                        else
                        {
                            byte result = 0;
                            if (byte.TryParse(fields[i], out result) && (result == 0 || result == 1))
                                row.Add(result);
                            else
                                throw new AggregateException($"Parse error at {parser.LineNumber} line pos {i} symbol <{fields[i]}> not byte");
                        }
                    }
                    if (!needHeader)
                    {
                        context.Add(row);
                        objects.Add(fields[0]);
                    }
                    needHeader = false;
                }
            }
        }


        public List<string> GetAttributes()
        {
            return attributes;
        }

        public List<List<byte>> GetContext()
        {
            return context;
        }

        public List<string> GetObjects()
        {
            return objects;
        }
    }
}
