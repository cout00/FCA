using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Import
{
    public interface IImport
    {
        List<string> GetAttributes();
        List<string> GetObjects();
        List<List<byte>> GetContext();
    }
}
