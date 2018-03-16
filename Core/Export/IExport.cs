using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Import;

namespace Core.Export
{
    public interface IExport:IImport
    {
        byte[,] ToMatrix();
    }
}
