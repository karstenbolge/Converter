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
    public class ImpRecordTests
    {
        static Logger logger = new Logger(DateTime.Now.ToShortDateString());

        ImpRecord impRecord = new ImpRecord(logger);
        [TestMethod()]
        public void setTransactionNumberTest()
        {
            impRecord.setTransactionNumber("123456789012345");
            Assert.AreEqual( "12345678901234", impRecord.getTransactionNumber());

            impRecord.setTransactionNumber("12345");
            Assert.AreEqual( "         12345", impRecord.getTransactionNumber());
        }

        [TestMethod()]
        public void setTransactionDateTest()
        {
            impRecord.setTransactionDate("120318");
            Assert.AreEqual("120318", impRecord.getTransactionDate());

            impRecord.setTransactionDate("121318");
            Assert.AreEqual( "121218", impRecord.getTransactionDate());

            impRecord.setTransactionDate("300218");
            Assert.AreEqual("290218", impRecord.getTransactionDate());

            impRecord.setTransactionDate("15-04-2019");
            Assert.AreEqual("150419", impRecord.getTransactionDate());

            impRecord.setTransactionDate("15-14-2019");
            Assert.AreEqual("151219", impRecord.getTransactionDate());

            impRecord.setTransactionDate("30-02-2019");
            Assert.AreEqual("290219", impRecord.getTransactionDate());

            impRecord.setTransactionDate("2019-09-10");
            Assert.AreEqual("100919", impRecord.getTransactionDate());

            impRecord.setTransactionDate("2019-09-03");
            Assert.AreEqual("030919", impRecord.getTransactionDate());
        }

        [TestMethod()]
        public void setStatusTest()
        {
            impRecord.setStatus('S');
            Assert.AreEqual('S', impRecord.getStatus());
        }

        [TestMethod()]
        public void setCustomerNumberTest()
        {
            impRecord.setCustomerNumber("120318");
            Assert.AreEqual("    120318", impRecord.getCustomerNumber());

            impRecord.setCustomerNumber("1234567812");
            Assert.AreEqual("1234567812", impRecord.getCustomerNumber());

            impRecord.setCustomerNumber("123456781203189");
            Assert.AreEqual("1234567812", impRecord.getCustomerNumber());
        }

        [TestMethod()]
        public void setTransactionTypeTest()
        {
            impRecord.setTransactionType("N");
            Assert.AreEqual("N   ", impRecord.getTransactionType());

            impRecord.setTransactionType("NADE");
            Assert.AreEqual("NADE", impRecord.getTransactionType());

            impRecord.setTransactionType("NADESTE");
            Assert.AreEqual("NADE", impRecord.getTransactionType());
        }

        [TestMethod()]
        public void setSettlementDateTest()
        {
            impRecord.setSettlementDate("120318");
            Assert.AreEqual("120318", impRecord.getSettlementDate());

            impRecord.setSettlementDate("121318");
            Assert.AreEqual("121218", impRecord.getSettlementDate());

            impRecord.setSettlementDate("300218");
            Assert.AreEqual("290218", impRecord.getSettlementDate());
        }


        [TestMethod()]
        public void setAccountNumberTest()
        {
            impRecord.setAccountNumber("120318");
            Assert.AreEqual("        120318", impRecord.getAccountNumber());

            impRecord.setAccountNumber("34567890120318");
            Assert.AreEqual("34567890120318", impRecord.getAccountNumber());

            impRecord.setAccountNumber("34567890120318123");
            Assert.AreEqual("34567890120318", impRecord.getAccountNumber());

            impRecord.setAccountNumber("000034567890120318123");
            Assert.AreEqual("34567890120318", impRecord.getAccountNumber());

            impRecord.setAccountNumber("000034567890", true);
            Assert.AreEqual("      34567890", impRecord.getAccountNumber());

            impRecord.setAccountNumber("000034567890", false, 10);
            Assert.AreEqual("    0034567890", impRecord.getAccountNumber());
        }

        [TestMethod()]
        public void setIdCodeTest()
        {
            impRecord.setIdCode("120318");
            Assert.AreEqual("      120318", impRecord.getIdCode());

            impRecord.setIdCode("345678901212");
            Assert.AreEqual("345678901212", impRecord.getIdCode());

            impRecord.setIdCode("34567890120318123");
            Assert.AreEqual("345678901203", impRecord.getIdCode());
        }

        [TestMethod()]
        public void setDepotNumberTest()
        {
            impRecord.setDepotNumber("120318");
            Assert.AreEqual("          120318", impRecord.getDepotNumber());

            impRecord.setDepotNumber("3456789012123232");
            Assert.AreEqual("3456789012123232", impRecord.getDepotNumber());

            impRecord.setDepotNumber("3456789012123232786");
            Assert.AreEqual("3456789012123232", impRecord.getDepotNumber());

            impRecord.setDepotNumber("00012123232786");
            Assert.AreEqual("  00012123232786", impRecord.getDepotNumber());

            impRecord.setDepotNumber("00012123232786", true);
            Assert.AreEqual("     12123232786", impRecord.getDepotNumber());

            impRecord.setDepotNumber("00112123232786", false, 10);
            Assert.AreEqual("      2123232786", impRecord.getDepotNumber());

            impRecord.setDepotNumber("00000000232786", true, 10);
            Assert.AreEqual("          232786", impRecord.getDepotNumber());
        }

        [TestMethod()]
        public void setAmountTest()
        {
            impRecord.setAmount("012345678901.012345000000");
            Assert.AreEqual("012345678901.012345000000", impRecord.getAmount());

            impRecord.setAmount("22,33");
            Assert.AreEqual("000000000022.330000000000", impRecord.getAmount());

            impRecord.setAmount("-19.33");
            Assert.AreEqual("000000000019.330000000000", impRecord.getAmount());

            impRecord.setAmount("A1212");
            Assert.AreEqual("000000001212.000000000000", impRecord.getAmount());

            impRecord.setAmount("00000000002742.000");
            Assert.AreEqual("000000002742.000000000000", impRecord.getAmount());
        }

        [TestMethod()]
        public void setExchangeRateTest()
        {
            impRecord.setExchangeRate("012345678901.012345000000");
            Assert.AreEqual("345678901.012345000", impRecord.getExchangeRate());

            impRecord.setExchangeRate("22,33");
            Assert.AreEqual("000000022.330000000", impRecord.getExchangeRate());

            impRecord.setExchangeRate("-19.33");
            Assert.AreEqual("000000019.33000000-", impRecord.getExchangeRate());

            impRecord.setExchangeRate("A1212");
            Assert.AreEqual("000001212.000000000", impRecord.getExchangeRate());

            impRecord.setPrice("412345678901.012345000000");
            Assert.AreEqual("345678901.012345000", impRecord.getExchangeRate());

            impRecord.setPrice("24,36");
            Assert.AreEqual("000000024.360000000", impRecord.getExchangeRate());

            impRecord.setPrice("-14.36");
            Assert.AreEqual("000000014.36000000-", impRecord.getExchangeRate());

            impRecord.setPrice("A12123");
            Assert.AreEqual("000012123.000000000", impRecord.getExchangeRate());
        }

        [TestMethod()]
        public void setCurrenciesCrossTest()
        {
            impRecord.setCurrenciesCross("DKK/UDS");
            Assert.AreEqual("DKK/UDS", impRecord.getCurrenciesCross());

            impRecord.setCurrenciesCross("DK/US");
            Assert.AreEqual("  DK/US", impRecord.getCurrenciesCross());

            impRecord.setCurrenciesCross("DDDK/UDSD");
            Assert.AreEqual("DDDK/UD", impRecord.getCurrenciesCross());

            impRecord.setCurrenciesCross("DKK", "USD");
            Assert.AreEqual("DKK/USD", impRecord.getCurrenciesCross());

            impRecord.setCurrenciesCross("DKK", "DKK");
            Assert.AreEqual("       ", impRecord.getCurrenciesCross());
        }

        [TestMethod()]
        public void setCurrenciesRateTest()
        {
            impRecord.setCurrenciesRate("0123435678901.012345000000");
            Assert.AreEqual("435678901.012345000", impRecord.getCurrenciesRate());

            impRecord.setCurrenciesRate("122,33");
            Assert.AreEqual("000000122.330000000", impRecord.getCurrenciesRate());

            impRecord.setCurrenciesRate("-219.33");
            Assert.AreEqual("000000219.330000000", impRecord.getCurrenciesRate());

            impRecord.setCurrenciesRate("A1212");
            Assert.AreEqual("000001212.000000000", impRecord.getCurrenciesRate());

            impRecord.setCurrenciesRate("100");
            Assert.AreEqual("000000100.000000000", impRecord.getCurrenciesRate());
        }

        [TestMethod()]
        public void setYeildTax()
        {
            impRecord.setYieldTax("345636789345.2346578");
            Assert.AreEqual("345636789345.2346578", impRecord.getYieldTax());

            impRecord.setYieldTax("345,68");
            Assert.AreEqual("000000000345.6800000", impRecord.getYieldTax());

            impRecord.setYieldTax("-536.38");
            Assert.AreEqual("000000000536.380000-", impRecord.getYieldTax());

            impRecord.setYieldTax("-   536.38");
            Assert.AreEqual("000000000536.380000-", impRecord.getYieldTax());

            impRecord.setYieldTax("A34");
            Assert.AreEqual("000000000034.0000000", impRecord.getYieldTax());
        }

        [TestMethod()]
        public void setInterestTest()
        {
            impRecord.setInterest("000123456789.0123456");
            Assert.AreEqual("000123456789.0123456", impRecord.getInterest());

            impRecord.setInterest("124,33");
            Assert.AreEqual("000000000124.3300000", impRecord.getInterest());

            impRecord.setInterest("-219.33");
            Assert.AreEqual("000000000219.330000-", impRecord.getInterest());

            impRecord.setInterest("A1212");
            Assert.AreEqual("000000001212.0000000", impRecord.getInterest());
        }

        [TestMethod()]
        public void setKurtageTest()
        {
            impRecord.setKurtage("000000456.123");
            Assert.AreEqual("000000456.123", impRecord.getKurtage());

            impRecord.setKurtage("8353,33");
            Assert.AreEqual("000008353.330", impRecord.getKurtage());

            impRecord.setKurtage("-220.43");
            Assert.AreEqual("000000220.43-", impRecord.getKurtage());

            impRecord.setKurtage("A1212");
            Assert.AreEqual("000001212.000", impRecord.getKurtage());
        }

        [TestMethod()]
        public void setCostTest()
        {
            impRecord.setCost("000000457.123");
            Assert.AreEqual("000000457.123", impRecord.getCost());

            impRecord.setCost("      457.123");
            Assert.AreEqual("000000457.123", impRecord.getCost());

            impRecord.setCost("8356,33");
            Assert.AreEqual("000008356.330", impRecord.getCost());

            impRecord.setCost("-320.43");
            Assert.AreEqual("000000320.43-", impRecord.getCost());

            impRecord.setCost("243D");
            Assert.AreEqual("000000243.000", impRecord.getCost());
        }

        [TestMethod()]
        public void setCuponTaxTest()
        {
            impRecord.setCuponTax("000000757.123");
            Assert.AreEqual("000000757.123", impRecord.getCuponTax());

            impRecord.setCuponTax("18356,33");
            Assert.AreEqual("000018356.330", impRecord.getCuponTax());

            impRecord.setCuponTax("-1320.43");
            Assert.AreEqual("000001320.43-", impRecord.getCuponTax());

            impRecord.setCuponTax("243D");
            Assert.AreEqual("000000243.000", impRecord.getCuponTax());
        }

        [TestMethod()]
        public void setCounterPartTest()
        {
            impRecord.setCounterPart("A");
            Assert.AreEqual("   A", impRecord.getCounterPart());

            impRecord.setCounterPart("ASDF");
            Assert.AreEqual("ASDF", impRecord.getCounterPart());

            impRecord.setCounterPart("ASDFGH");
            Assert.AreEqual("ASDF", impRecord.getCounterPart());
        }

        [TestMethod()]
        public void setNotaTest()
        {
            impRecord.setNota('N');
            Assert.AreEqual('N', impRecord.getNota());
        }

        [TestMethod()]
        public void setNameTest()
        {
            impRecord.setName("Allan");
            Assert.AreEqual("Allan                         ", impRecord.getName());

            impRecord.setName("Allan Egon Olsen er fodboldspi");
            Assert.AreEqual("Allan Egon Olsen er fodboldspi", impRecord.getName());

            impRecord.setName("Men denne streng er for lang og vil blive afkortet");
            Assert.AreEqual("Men denne streng er for lang o", impRecord.getName());
        }

        [TestMethod()]
        public void setTimeTest()
        {
            impRecord.setTime("1213");
            Assert.AreEqual("1213       ", impRecord.getTime());

            impRecord.setTime("12345678901");
            Assert.AreEqual("12345678901", impRecord.getTime());

            impRecord.setTime("212345678901");
            Assert.AreEqual("21234567890", impRecord.getTime());

            impRecord.setTime("12A45678901");
            Assert.AreEqual("12045678901", impRecord.getTime());
        }

        [TestMethod()]
        public void setProjectNumberTest()
        {
            impRecord.setProjectNumber("323");
            Assert.AreEqual("       323", impRecord.getProjectNumber());

            impRecord.setProjectNumber("1234567890");
            Assert.AreEqual("1234567890", impRecord.getProjectNumber());

            impRecord.setProjectNumber("2312345678901");
            Assert.AreEqual("2312345678", impRecord.getProjectNumber());
        }

        [TestMethod()]
        public void setUnknownTest()
        {
            impRecord.setUnknown("323");
            Assert.AreEqual("323                     ", impRecord.getUnknown());

            impRecord.setUnknown("123456789012345678901234");
            Assert.AreEqual("123456789012345678901234", impRecord.getUnknown());

            impRecord.setUnknown("1231234567890123456789012345");
            Assert.AreEqual("123123456789012345678901", impRecord.getUnknown());
        }

        [TestMethod()]
        public void setTekstTest()
        {
            impRecord.setTekst("min tekst");
            Assert.AreEqual("min tekst                     ", impRecord.getTekst());

            impRecord.setTekst("Palle Egon Olsen er fodboldspi");
            Assert.AreEqual("Palle Egon Olsen er fodboldspi", impRecord.getTekst());

            impRecord.setTekst("men denne streng er for lang og vil blive afkortet");
            Assert.AreEqual("men denne streng er for lang o", impRecord.getTekst());
        }

        [TestMethod()]
        public void setArtTest()
        {
            impRecord.setArt("345636789345.2346578");
            Assert.AreEqual("345636789345.23465780000", impRecord.getArt());

            impRecord.setArt("345,68");
            Assert.AreEqual("000000000345.68000000000", impRecord.getArt());

            impRecord.setArt("-536.38");
            Assert.AreEqual("000000000536.38000000000", impRecord.getArt());

            impRecord.setArt("A34");
            Assert.AreEqual("000000000034.00000000000", impRecord.getArt());
        }

        [TestMethod()]
        public void setSaldiValuerTest()
        {
            impRecord.setSaldiValuer("000000757.123");
            Assert.AreEqual("000000000757.123000", impRecord.getSaldiValuer());

            impRecord.setSaldiValuer("18356,33");
            Assert.AreEqual("000000018356.330000", impRecord.getSaldiValuer());

            impRecord.setSaldiValuer("-1320.43");
            Assert.AreEqual("000000001320.43000-", impRecord.getSaldiValuer());

            impRecord.setSaldiValuer("243D");
            Assert.AreEqual("000000000243.000000", impRecord.getSaldiValuer());
        }
    }
}