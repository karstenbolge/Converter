using System;
using System.IO;

namespace Converter
{
    public class ImpRecord
    {
        const string FILE_NAME = "PfTrans.Imp";
        Logger logger;

        String transactionNumber;//Transaktions nummer
        DateOnly transactionDate;//Handel Dato
        Char status;//Status
        String customerNumber;//Kunde nummer
        String transactionType;//Transaktionstype
        DateOnly settlementDate;//Valør Dato
        String accountNumber;//Konto nummer
        String idCode;//Idkode
        String depotNumber;//Depot nummer
        DecimalNumber amount;//Stk
        DecimalNumber exchangeRate; //kurs / beløb
        String currenciesCross; //Valutakryds
        DecimalNumber currenciesRate;//Valutakurs
        DecimalNumber yeildTax;//Udbytte skat 
        DecimalNumber interest;//Vedh. Renter 
        DecimalNumber kurtage;//Kurtage
        DecimalNumber cost;//Diverse omkostninger
        DecimalNumber cuponTax;//Kuponskat
        String counterPart;//Modpart
        Char nota;//Notaudskrift
        String name;//Navn
        String time;//Tid
        String projectNumber;//Project nummer
        String unknown;//672-695
        String tekst;//tekst
        DecimalNumber art;//Art
        DecimalNumber saldiValuer;//Saldo pr.Valørdato

        const String spaces = "                                            ";

        public ImpRecord(Logger l)
        {
            logger = l;

            transactionNumber = string.Empty;
            transactionDate = new DateOnly(logger);//Handel Dato
            status = ' ';
            customerNumber = string.Empty;
            transactionType = string.Empty;
            settlementDate = new DateOnly(logger);//Valør Dato
            accountNumber = string.Empty;
            idCode = string.Empty;
            depotNumber = string.Empty;
            amount = new DecimalNumber(12, 12, false, logger);//Stk
            exchangeRate = new DecimalNumber(9, 9, true, logger); //kurs / beløb
            currenciesCross = string.Empty;
            currenciesRate = new DecimalNumber(9, 9, false, logger);//Valutakurs
            yeildTax = new DecimalNumber(12, 7, true, logger);//Valutakurs
            interest = new DecimalNumber(12, 7, true, logger);//Vedh. Renter 
            kurtage = new DecimalNumber(9, 3, true, logger);//Kurtage
            cost = new DecimalNumber(9, 3, true, logger);//Diverse omkostninger
            cuponTax = new DecimalNumber(9, 3, true, logger);//Kuponskat
            counterPart = string.Empty;
            nota = ' ';
            name = string.Empty;
            time = string.Empty;
            projectNumber = string.Empty;
            unknown = string.Empty;
            tekst = string.Empty;
            art = new DecimalNumber(12, 11, false, logger);//art
            saldiValuer = new DecimalNumber(12, 6, true, logger); //saldiValuer
        }

        public void setTransactionNumber(String s, bool onlyDecimas = false)
        {
            if (s.Length > 14)
            {
                if (onlyDecimas)
                {
                    int i = 0;
                    while(i < s.Length && s[i] >= '0' && s[i] <= '9')
                    {
                        i++;
                    }

                    if (i < s.Length)
                    {
                        s = s.Substring(0, i);
                    }
                }
                if (s.Length > 14)
                {
                    logger.Write("TransactionNumber : " + s + " too long!");
                }
            }

            transactionNumber = s.Substring(0, s.Length > 14 ? 14 : s.Length);
        }

        public String getTransactionNumber()
        {
            return spaces.Substring(0, 14 - transactionNumber.Length) + transactionNumber;
        }

        public void setTransactionDate(String s)
        {
            transactionDate.setDate(s);            
        }

        public String getTransactionDate()
        {
            return transactionDate.getDate();
        }

        public void setStatus(Char s)
        {
            status = s;
        }

        public Char getStatus()
        {
            return status;
        }

        public void setCustomerNumber(String s)
        {
            if (s.Length > 10)
            {
                logger.Write("CustomerNumber : " + s + " too long!");
            }

            customerNumber = s.Substring(0, s.Length > 10 ? 10 : s.Length);
        }

        public String getCustomerNumber()
        {
            return spaces.Substring(0, 10 - customerNumber.Length) + customerNumber;
        }

        public void setTransactionType(String s)
        {
            if (s.Length > 4)
            {
                logger.Write("TransactionType : " + s + " too long!");
            }

            transactionType = s.Substring(0, s.Length > 4 ? 4 : s.Length);
        }

        public String getTransactionType()
        {
            return transactionType + spaces.Substring(0, 4 - transactionType.Length);
        }

        public void setSettlementDate(String s)
        {
            settlementDate.setDate(s);
        }

        public String getSettlementDate()
        {
            return settlementDate.getDate();
        }

        public void setAccountNumber(String s, bool removePaddingZero = false, int lastXdigits = 0)
        {
            s = s.TrimEnd(' ').TrimStart(' ');
            if (lastXdigits > 0 && s.Length > lastXdigits)
            {
                s = s.Substring(s.Length - lastXdigits);
            }

            if (removePaddingZero)
            {
                while(s.Length > 0 && s[0] == '0')
                {
                    s = s.Substring(1);
                }
            }

            if (s.Length > 14)
            {
                while(s[0] == '0' && s.Length > 14)
                {
                    s = s.Substring(1, s.Length - 1);
                }
                if (s.Length > 14)
                {
                    logger.Write("AccountNumber : " + s + " too long!");
                }
            }

            accountNumber = s.Substring(0, s.Length > 14 ? 14 : s.Length);
        }

        public String getAccountNumber()
        {
            return spaces.Substring(0, 14 - accountNumber.Length) + accountNumber;
        }

        public void setIdCode(String s)
        {
            if (s.Length > 12)
            {
                logger.Write("IdCode : " + s + " too long!");
            }

            idCode  = s.Substring(0, s.Length > 12 ? 12 : s.Length);
        }

        public String getIdCode()
        {
            return spaces.Substring(0, 12 - idCode.Length) + idCode;
        }

        public void setDepotNumber(String s, bool removePaddingZero = false, int lastXdigits = 0)
        {   
            if (lastXdigits > 0 && s.Length > lastXdigits)
            {
                s = s.Substring(s.Length - lastXdigits);
            }

            if (removePaddingZero)
            {
                while (s.Length > 0 && s[0] == '0')
                {
                    s = s.Substring(1);
                }
            }

            if (s.Length > 16)
            {
                logger.Write("DepotNumber : " + s + " too long!");
            }

            depotNumber = s.Substring(0, s.Length > 16 ? 16 : s.Length);
        }

        public String getDepotNumber()
        {
            return spaces.Substring(0, 16 - depotNumber.Length) + depotNumber;
        }

        public void blankAmount()
        {
            amount.setBlank();
        }

        public void setAmount(String s)
        {
            amount.setDecimalNumber(s);
        }

        public String getAmount()
        {
            return amount.getDecimalNumber();
        }

        public void setExchangeRate(String s)
        {
            exchangeRate.setDecimalNumber(s);
        }

        public void setPrice(String s)
        {
            exchangeRate.setDecimalNumber(s);
        }

        public String getExchangeRate()
        {
            return exchangeRate.getDecimalNumber();
        }

        public void setCurrenciesCross(String s1, String s2)
        {
            if (s1.CompareTo(s2) != 0)
            {
                setCurrenciesCross(s1 + "/" + s2);
            }
            else
            {
                setCurrenciesCross(String.Empty);
            }
        }

        public void setCurrenciesCross(String s)
        {
            if (s.Length > 7)
            {
                logger.Write("CurrenciesCross : " + s + " too long!");
            }

            if (s.Length >= 7) { 
                string s1 = s.Substring(0, 3);
                string s2 = s.Substring(4, 3);

                if (s1.CompareTo(s2) != 0)
                {
                    currenciesCross = s.Substring(0, s.Length > 7 ? 7 : s.Length);
                }
                else
                {
                    setCurrenciesCross(String.Empty);
                }
                return;
            }

            currenciesCross = s.Substring(0, s.Length > 7 ? 7 : s.Length);
        }

        public String getCurrenciesCross()
        {
            return spaces.Substring(0, 7 - currenciesCross.Length) + currenciesCross;
        }

        public void setCurrenciesRate(String s)
        {
            /*try
            {
                int i = Int32.Parse(s);
                if (i == 100)
                {
                    currenciesRate.setDecimalNumber(string.Empty);
                    return;
                }
            }
            catch (FormatException)
            {
            }*/

            currenciesRate.setDecimalNumber(s);
        }

        public String getCurrenciesRate()
        {
            return currenciesRate.getDecimalNumber();
        }

        public void setYieldTax(String s)
        {
            yeildTax.setDecimalNumber(s);
        }

        public bool yieldTaxSet()
        {
            return yeildTax.notSet();
        }

        public String getYieldTax()
        {
            return yeildTax.getDecimalNumber();
        }

        public void setInterest(String s)
        {
            interest.setDecimalNumber(s);
        }

        public String getInterest()
        {
            return interest.getDecimalNumber();
        }

        public void blankKurtage()
        {
            kurtage.setBlank();
        }

        public void setKurtage(String s)
        {
            kurtage.setDecimalNumber(s);
        }

        public String getKurtage()
        {
            return kurtage.getDecimalNumber();
        }

        public void setCost(String s)
        {
            cost.setDecimalNumber(s);
        }

        public String getCost()
        {
            return cost.getDecimalNumber();
        }

        public void setCuponTax(String s)
        {
            cuponTax.setDecimalNumber(s);
        }

        public String getCuponTax()
        {
            return cuponTax.getDecimalNumber();
        }

        public void setCounterPart(String s)
        {
            if (s.Length > 4)
            {
                logger.Write("CounterPart : " + s + " too long!");
            }

            counterPart = s.Substring(0, s.Length > 4 ? 4 : s.Length);
        }

        public String getCounterPart()
        {
            return spaces.Substring(0, 4 - counterPart.Length) + counterPart;
        }

        public void setNota(Char s)
        {
            nota = s;
        }

        public Char getNota()
        {
            return nota;
        }

        public void setName(String s)
        {
            if (s.Length > 30)
            {
                logger.Write("Name : " + s + " too long!");
            }

            name = s.Substring(0, s.Length > 30 ? 30 : s.Length);
        }

        public String getName()
        {
            return name + spaces.Substring(0, 30 - name.Length);
        }

        public void setTime(String s)
        {
            if (s.Length > 11)
            {
                logger.Write("Time : " + s + " too long!");
            }

            for(int i = 0; i < s.Length; i++)
            {
                if (s[i] > '9' || s[i] < '0')
                {
                    logger.Write("Time : " + s + " should be number!");
                    s = s.Replace(s[i], '0');
                }
            }
            time = s.Substring(0, s.Length > 11 ? 11 : s.Length);
        }

        public String getTime()
        {
            return time + spaces.Substring(0, 11 - time.Length);
        }

        public void setProjectNumber(String s)
        {
            if (s.Length > 10)
            {
                logger.Write("CounterPart : " + s + " too long!");
            }

            projectNumber = s.Substring(0, s.Length > 10 ? 10 : s.Length);
        }

        public String getProjectNumber()
        {
            return spaces.Substring(0, 10 - projectNumber.Length) + projectNumber;
        }

        public void setUnknown(String s)
        {
            if (s.Length > 24)
            {
                logger.Write("Unknown : " + s + " too long!");
            }

            unknown = s.Substring(0, s.Length > 24 ? 24 : s.Length);
        }

        public String getUnknown()
        {
            return unknown + spaces.Substring(0, 24 - unknown.Length);
        }

        public void setTekst(String s)
        {
            if (s.Length > 30)
            {
                logger.Write("Tekst : " + s + " too long!");
            }

            tekst = s.Substring(0, s.Length > 30 ? 30 : s.Length);
        }

        public String getTekst()
        {
            return tekst + spaces.Substring(0, 30 - tekst.Length);
        }

        public void setArt(String s)
        {
            art.setDecimalNumber(s);
        }

        public String getArt()
        {
            return art.getDecimalNumber();
        }
        
        public void setSaldiValuer(String s)
        {
            saldiValuer.setDecimalNumber(s);
        }

        public String getSaldiValuer()
        {
            return saldiValuer.getDecimalNumber();
        }

        public void writeKoebSalgAktier(String path)
        {
            using (StreamWriter w = File.AppendText(path + "\\" + FILE_NAME))
            {
                w.WriteLine("{0} {1} {2} {3} {4}   {5} {6} {7} {8} {9} {10}                       {11} {12}                                                 {13} {14}               {15} {16}                                                                        {17} {18}                                                                                {19}                                                                                                                                                                                     {20}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        ", 
                    getTransactionNumber(), 
                    getTransactionDate(), 
                    getStatus(), 
                    getCustomerNumber(),
                    getTransactionType(),
                    getSettlementDate(),
                    getAccountNumber(),
                    getIdCode(),
                    getDepotNumber(),
                    getAmount(),
                    getExchangeRate(),
                    getCurrenciesCross(),
                    getCurrenciesRate(),
                    getKurtage(),
                    getCost(),
                    getCounterPart(),
                    getNota(),
                    getName(),
                    getTime(),
                    getProjectNumber(),
                    getUnknown());
            }
        }

        public void writeKoebSalgObligationer(String path)
        {
            using (StreamWriter w = File.AppendText(path + "\\" + FILE_NAME))
            {
                w.WriteLine("{0} {1} {2} {3} {4}   {5} {6} {7} {8} {9} {10}                       {11} {12}                            {20} {13} {14} {21} {15} {16}                                                                        {17} {18}                                                                                {19}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     ",
                    getTransactionNumber(),
                    getTransactionDate(),
                    getStatus(),
                    getCustomerNumber(),
                    getTransactionType(),
                    getSettlementDate(),
                    getAccountNumber(),
                    getIdCode(),
                    getDepotNumber(),
                    getAmount(),
                    getExchangeRate(),
                    getCurrenciesCross(),
                    getCurrenciesRate(),
                    getKurtage(),
                    getCost(),
                    getCounterPart(),
                    getNota(),
                    getName(),
                    getTime(),
                    getProjectNumber(),
                    getInterest(),
                    getCuponTax());
            }
        }

        public void writeUdbytteAktier(String path)
        {
            using (StreamWriter w = File.AppendText(path + "\\" + FILE_NAME))
            {
                w.WriteLine("{0} {1} {2} {3} {4}   {5} {6} {7} {8} {9} {10}                       {11} {12}       {19}                      {13} {14}                    {15}                                                                        {16} {17}                                                                                {18}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     ",
                    getTransactionNumber(),
                    getTransactionDate(),
                    getStatus(),
                    getCustomerNumber(),
                    getTransactionType(),
                    getSettlementDate(),
                    getAccountNumber(),
                    getIdCode(),
                    getDepotNumber(),
                    getAmount(),
                    getExchangeRate(),
                    getCurrenciesCross(),
                    getCurrenciesRate(),
                    getKurtage(),
                    getCost(),
                    getNota(),
                    getName(),
                    getTime(),
                    getProjectNumber(),
                    getYieldTax());
            }
        }

        public void writeRenteKuponer(String path)
        {
            using (StreamWriter w = File.AppendText(path + "\\" + FILE_NAME))
            {
                w.WriteLine("{0} {1} {2} {3} {4}   {5} {6} {7} {8}                           {9}                       {10} {11}                                                                             {17} {12} {13}                                                                        {14} {15}                                                                                {16}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     ",
                    getTransactionNumber(),
                    getTransactionDate(),
                    getStatus(),
                    getCustomerNumber(),
                    getTransactionType(),
                    getSettlementDate(),
                    getAccountNumber(),
                    getIdCode(),
                    getDepotNumber(),
                    getExchangeRate(),
                    getCurrenciesCross(),
                    getCurrenciesRate(),
                    getCounterPart(),
                    getNota(),
                    getName(),
                    getTime(),
                    getProjectNumber(),
                    getCuponTax());
            }
        }

        public void writeUdtraekObligationer(String path)
        {
            using (StreamWriter w = File.AppendText(path + "\\" + FILE_NAME))
            {
                w.WriteLine("{0} {1} {2} {3} {4}   {5} {6} {7} {8} {9} {10}                       {11} {12}                                                                                                {13}                                                                        {14} {15}                                                                                {16}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     ",
                    getTransactionNumber(),
                    getTransactionDate(),
                    getStatus(),
                    getCustomerNumber(),
                    getTransactionType(),
                    getSettlementDate(),
                    getAccountNumber(),
                    getIdCode(),
                    getDepotNumber(),
                    getAmount(),
                    getExchangeRate(),
                    getCurrenciesCross(),
                    getCurrenciesRate(),
                    getNota(),
                    getName(),
                    getTime(),
                    getProjectNumber(),
                    getUnknown());
            }
        }

        public void writeIndsaetHaev(String path)
        {
            using (StreamWriter w = File.AppendText(path + "\\" + FILE_NAME))
            {
                w.WriteLine("{0} {1} {2} {3} {4}   {5} {6}                                                         {7}                       {8} {9}                                                                                           {14} {10}                                  {15}        {11} {12}                                                                                {13}                                                                                                                                                                                                                                                                   {16}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          ",
                    getTransactionNumber(),
                    getTransactionDate(),
                    getStatus(),
                    getCustomerNumber(),
                    getTransactionType(),
                    getSettlementDate(),
                    getAccountNumber(),
                    getExchangeRate(),
                    getCurrenciesCross(),
                    getCurrenciesRate(),
                    getNota(),
                    getName(),
                    getTime(),
                    getProjectNumber(),
                    getCounterPart(),
                    getTekst(),
                    getArt());
            }
        }

        public void writeSaldi(String path)
        {
            using (StreamWriter w = File.AppendText(path + "\\" + FILE_NAME))
            {
                w.WriteLine("{0} {1} {2} {3} {4}   {5} {6}                                                         {7}                       {8} {9}       {15}                                                                      {10}                                  {13}        {11} {12}                                                                                                                                                                                                                                                                                                                                                             {14}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          ",
                    getTransactionNumber(),
                    getTransactionDate(),
                    getStatus(),
                    getCustomerNumber(),
                    getTransactionType(),
                    getSettlementDate(),
                    getAccountNumber(),
                    getExchangeRate(),
                    getCurrenciesCross(),
                    getCurrenciesRate(),
                    getNota(),
                    getName(),
                    getTime(),
                    getTekst(),
                    getArt(),
                    getSaldiValuer());
            }
        }

        public void createEmptyFile(String path)
        {
            File.Create(path + "\\" + FILE_NAME).Dispose();
        }
    }
}
