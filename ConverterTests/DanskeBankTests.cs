using Microsoft.VisualStudio.TestTools.UnitTesting;
using Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter.Tests
{
    [TestClass()]
    public class DanskeBankTests
    {
        static Logger logger = new Logger(DateTime.Now.ToShortDateString());

        DanskeBank danskeBank = new DanskeBank(new string[0], logger);

        [TestMethod()]
        public void splitTransactioNumberTest()
        {
            Assert.AreEqual("20190823468308", danskeBank.splitTransactioNumber("3010097525     US855244109420190807  2019-08-23-07.22.01.468308"));
        }
    }
}