using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Import;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Tests
    {

        [Test]
        public void ParserTests()
        {
            CSVImport csvImport=new CSVImport(@"E:\Магистратура\5\test.csv",";");
        }

    }
}
