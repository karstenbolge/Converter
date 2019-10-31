using System;
using System.Globalization;

// Beware danske bank is using decimal point, hence culrura varial en-US
namespace Converter
{
    public class DanskeBank
    {
        static Logger logger;
        String[] lines;
        int numberOfSupoerPortRecords;

        public DanskeBank(String [] lines, Logger l)
        {
            this.lines = lines;
            logger = l;
        }

        public String splitTransactioNumber(String t)
        {
            String lastPart = t.Split(' ')[t.Split(' ').Length - 1];
            String[] datePart = lastPart.Split('-');
            String dotPart = lastPart.Split('.')[lastPart.Split('.').Length - 1];

            if (datePart.Length < 2)
            {
                return t;
            }

            if (dotPart.Length < 1)
            {
                return t;
            }

            return datePart[0] + datePart[1] + datePart[2].Substring(0, 2) + dotPart;
        }

        public int Process(ref string emailBody, ref bool debugLevel, ref bool success, string fileName)
        {
            numberOfSupoerPortRecords = 0;

            logger.Write("    Danske Bank format");

            int hdlobl = 0;
            int udbakt = 0;
            int kntopd = 0;
            int hdlakt = 0;
            int rntobl = 0;

            if (lines.Length > 2)
            {
                for (int k = 1; k < lines.Length - 1; k++)
                {
                    string[] fields = lines[k].Split((char)31);
                    if (debugLevel)
                    {
                        logger.WriteFields(fields);
                    }

                    if (lines[k].IndexOf("HDLOBL") == 0)
                    {
                        hdlobl++;
                        ImpRecord impRecord = new ImpRecord(logger);

                        if (fields.Length < 70)
                        {
                            emailBody += Environment.NewLine + "Danske bank KS record " + hdlobl + " has too few fields";
                            logger.Write("      Handel med obligationer og pantebreve record too few fields");
                        }
                        else
                        {
                            impRecord.setTransactionDate(fields[11]); // not fields[3] as it has to be the same as settlementdate
                            impRecord.setIdCode(fields[4]);
                            impRecord.setSettlementDate(fields[11]);
                            // impRecord.setTransactionNumber(splitTransactioNumber(fields[16])); use SuperPorts
                            impRecord.setAmount(fields[17]);
                            impRecord.setExchangeRate(fields[18]);
                            impRecord.setKurtage("-" + fields[21]);
                            impRecord.setCurrenciesCross(fields[53], fields[55]);
                            impRecord.setCurrenciesRate(fields[23]);
                            // take last 14 digits
                            impRecord.setAccountNumber(fields[30], false, 14);
                            impRecord.setPrice(fields[31]);
                            // take last 14 digits
                            impRecord.setDepotNumber(fields[34], false, 14);

                            impRecord.setStatus('N');
                            
                            impRecord.writeKoebSalgObligationer(fileName);
                        }
                    }
                    else if (lines[k].IndexOf("UDBAKT") == 0)
                    {
                        udbakt++;
                        ImpRecord impRecord = new ImpRecord(logger);
                        
                        if (fields.Length < 58)
                        {
                            emailBody += Environment.NewLine + "Danske Bank UDBAKT record " + udbakt + " has too few fields";
                            logger.Write("      Udbytte aktier record too few fields");
                        }
                        else
                        {
                            impRecord.setTransactionDate(fields[11]); // not fields[3] as it has to be the same as settlementdate
                            impRecord.setIdCode(fields[4]);
                            impRecord.setSettlementDate(fields[11]);
                            // impRecord.setTransactionNumber(splitTransactioNumber(fields[16])); use SuperPorts
                            impRecord.setPrice(fields[17]);
                            impRecord.setCurrenciesRate(fields[23]);
                            // yeield tax should be decucted - hence negativ and the dash infront of the field
                            impRecord.setYieldTax("-" + fields[24]);
                            // take last 14 digits
                            impRecord.setAccountNumber(fields[30], false, 14);
                            // take last 14 digits
                            impRecord.setDepotNumber(fields[34], false, 14);
                            impRecord.setTransactionType("U");
                            impRecord.setNota('N');
                            impRecord.blankAmount();
                            impRecord.blankKurtage();

                            impRecord.setStatus('N');
                            
                            impRecord.setCurrenciesCross(fields[55], fields[53]);

                            numberOfSupoerPortRecords++;
                            impRecord.writeUdbytteAktier(fileName);
                        }
                    }
                    else if (lines[k].IndexOf("KNTOPD") == 0)
                    {
                        kntopd++;
                        ImpRecord impRecord = new ImpRecord(logger);

                        if (fields.Length < 58)
                        {
                            emailBody += Environment.NewLine + "Danske Bank KNTOPD record " + kntopd + " has too few fields";
                            logger.Write("      Konto opdatering record too few fields");
                        }
                        else
                        {
                            impRecord.setTransactionDate(fields[11]); // not fields[3] as it has to be the same as settlementdate
                            impRecord.setSettlementDate(fields[11]);
                            // impRecord.setTransactionNumber(splitTransactioNumber(fields[16])); use SuperPorts
                            impRecord.setPrice(fields[17]);
                            impRecord.setCurrenciesRate(fields[23]);
                            // take last 14 digits
                            impRecord.setAccountNumber(fields[30], false, 14);
                            // take last 14 digits
                            impRecord.setDepotNumber(fields[34], false, 14);
                            impRecord.setTransactionType("I");
                            impRecord.setNota('N');
                            impRecord.setAmount(fields[17]);
                            impRecord.blankKurtage();

                            impRecord.setStatus('N');

                            impRecord.setCurrenciesCross(fields[55], fields[53]);

                            numberOfSupoerPortRecords++;
                            impRecord.writeIndsaetHaev(fileName);
                        }
                    }
                    else if (lines[k].IndexOf("HDLAKT") == 0)
                    {
                        hdlakt++;
                        ImpRecord impRecord = new ImpRecord(logger);
                        
                        if (fields.Length < 72)
                        {
                            emailBody += Environment.NewLine + "Danske Bank HDLAKT record " + hdlakt + " has too few fields";
                            logger.Write("      Handel med aktier record too few fields");
                        }
                        else
                        {
                            impRecord.setTransactionDate(fields[3]);
                            impRecord.setIdCode(fields[4]);
                            if (fields[17].Length > 0 && fields[17][0] == '-')
                            {
                                impRecord.setTransactionType("S"); // Salg
                            }
                            else
                            {
                                impRecord.setTransactionType("K"); // Køb
                            }

                            impRecord.setSettlementDate(fields[11]);
                            // impRecord.setTransactionNumber(splitTransactioNumber(fields[16])); use SuperPorts
                            impRecord.setAmount(fields[17]);
                            impRecord.setExchangeRate(fields[18]);

                            CultureInfo cultureInfo = new CultureInfo("en-US");
                            Decimal currencyRate = Convert.ToDecimal(fields[23], cultureInfo);
                            Decimal kurtage = Convert.ToDecimal(fields[21], cultureInfo);
                            Decimal cost = Convert.ToDecimal(fields[22], cultureInfo);
                            impRecord.setKurtage("-" + Math.Round(cost + kurtage / currencyRate * 100, 2).ToString());
                             
                            //impRecord.setKurtage("-" + fields[21]);
                            impRecord.setCurrenciesRate(fields[23]);
                            impRecord.setYieldTax(fields[24]);
                            // take last 14 digits
                            impRecord.setAccountNumber(fields[30], false, 14);
                            // take last 14 digits
                            impRecord.setDepotNumber(fields[34], false, 14);
                            impRecord.setNota('N');
                            impRecord.setCounterPart("DB"); // Danske bank 

                            impRecord.setStatus('N');

                            impRecord.setCurrenciesCross(fields[55], fields[53]);

                            numberOfSupoerPortRecords++;
                            impRecord.writeKoebSalgAktier(fileName);
                        }
                    }
                    else if (lines[k].IndexOf("RNTOBL") == 0)
                    {
                        rntobl++;
                        ImpRecord impRecord = new ImpRecord(logger);
                        
                        if (fields.Length < 58)
                        {
                            emailBody += Environment.NewLine + "Danske Bank RNTOBL record " + rntobl + " has too few fields";
                            logger.Write("      Kupon rente for obligationer og pantebreve record too few fields");
                        }
                        else
                        {
                            impRecord.setTransactionDate(fields[11]); // not fields[3] as it has to be the same as settlementdate
                            impRecord.setIdCode(fields[4]);
                            impRecord.setSettlementDate(fields[11]);
                            // impRecord.setTransactionNumber(splitTransactioNumber(fields[16])); use SuperPorts
                            impRecord.setPrice(fields[17]);

                            impRecord.setExchangeRate(fields[17]);
                            impRecord.setKurtage("-" + fields[21]);
                            impRecord.setAmount(fields[32]);
                            impRecord.setCurrenciesRate(fields[23]);
                            impRecord.setYieldTax(fields[24]);
                            // take last 14 digits
                            impRecord.setAccountNumber(fields[30], false, 14);
                            // take last 14 digits
                            impRecord.setDepotNumber(fields[34], false, 14);
                            impRecord.setTransactionType("KR");
                            impRecord.setNota('N');

                            impRecord.setStatus('N');

                            impRecord.setCurrenciesCross(fields[55], fields[53]);

                            numberOfSupoerPortRecords++;
                            impRecord.writeRenteKuponer(fileName);
                        }
                    }
                    else
                    {
                        // success = false; It is ok with unknown danske bank formats
                        logger.Write("      Ukendt fil format, " + lines[k].Split((char)31)[0]);
                    }
                }
                if (hdlobl > 0) logger.Write("      Handel med obligationer og pantebreve : " + hdlobl);
                if (udbakt > 0) logger.Write("      Udbytte aktier : " + udbakt);
                if (hdlakt > 0) logger.Write("      Handel med aktier : " + hdlakt);
                if (rntobl > 0) logger.Write("      Kupon rente for obligationer og pantebreve : " + rntobl);
            }
            else
            {
                if (lines.Length == 2 && lines[1].IndexOf("TAIL") == 0)
                {
                    logger.Write("      Filen indeholder ingen rcords");
                }
                else
                {
                    success = false;
                    logger.Write("      Ukendt fil format");
                }
            }

            return numberOfSupoerPortRecords;
        }
    }
}
