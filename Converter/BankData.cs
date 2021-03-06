﻿using System;
using System.Collections.Generic;
using System.Globalization;

namespace Converter
{
    class BankData
    {
        static Logger logger;
        String[] lines;
        int numberOfSupoerPortRecords;
        FondCode fondCode;
        List<string> annuls;

        private bool isAnul(string anul)
        {
            return annuls.Find(x => x.CompareTo( anul) == 0) != null;
        }

        public BankData(String[] lines, ref FondCode fondCode, Logger l)
        {
            this.lines = lines;
            this.fondCode = fondCode;
            logger = l;
            annuls = new List<string>();
        }

        public int Process(ref string emailBody, ref bool debugLevel, ref bool success, string fileName, string folder)
        {
            numberOfSupoerPortRecords = 0;

            logger.Write("    Jyske Bank format");

            string counterPart = "JB";
            if (folder.ToLower().Contains("sydbank"))
            {
                counterPart = "SB";
            }

            if (lines.Length > 1)
            {
                int ub = 0;
                int kr = 0;
                int ks = 0;
                int ksAnuls = 0;

                for (int k = 1; k < lines.Length; k++)
                {
                    string[] fields = lines[k].Split(';');

                    if (fields[0].CompareTo("KS") == 0)
                    {
                        if (fields[5].CompareTo("95") == 0)
                        {
                            annuls.Add(fields[6]);
                        }
                    }
                }

                for (int k = 1; k < lines.Length; k++)
                {
                    string[] fields = lines[k].Split(';');
                    if (debugLevel)
                    {
                        logger.WriteFields(fields);
                    }

                    if (fields[0].CompareTo("UB") == 0)
                    {
                        ImpRecord impRecord = new ImpRecord(logger);

                        impRecord.setTransactionDate(fields[10]);  // // not fields[6] as it has to be the same as settlementdate
                        // use the dictionary to get the isin code
                        impRecord.setIdCode(fondCode.getIsin(fields[3]));
                        impRecord.setSettlementDate(fields[10]);
                        // impRecord.setTransactionNumber(fields[4]); use SuperPorts
                        // The file does not contain the original amount so we have to calculate it from the currency rate and the settled amount, in danish number format
                        CultureInfo cultureInfo = new CultureInfo("da-DK");
                        Decimal currencyRate = Convert.ToDecimal(fields[11], cultureInfo);
                        Decimal settlementAmount = Convert.ToDecimal(fields[14], cultureInfo);
                        impRecord.setPrice(Math.Round(settlementAmount / currencyRate * 100, 2).ToString());

                        impRecord.setCurrenciesRate(fields[11]);
                        // yeield tax should be decucted - hence negativ and the dash infront of the field
                        Decimal tax = Convert.ToDecimal(fields[15], cultureInfo);
                        impRecord.setYieldTax("-" + Math.Round(tax / currencyRate * 100, 2).ToString());
                        // take last 14 digits
                        impRecord.setAccountNumber(fields[2], false, 14);
                        // take last 14 digits

                        impRecord.setDepotNumber(fields[1], false, 14);
                        impRecord.setNota('N');
                        impRecord.blankAmount();
                        impRecord.blankKurtage();

                        Decimal cost = Convert.ToDecimal(fields[16], cultureInfo);
                        impRecord.setCost("-" + Math.Round(cost / currencyRate * 100, 2).ToString());

                        impRecord.setStatus('N');

                        impRecord.setCurrenciesCross(fields[8] + "/" + fields[9]);

                        if (fields[7] == "A")
                        {
                            ub++;
                            impRecord.setTransactionType("U");
                            numberOfSupoerPortRecords++;
                            impRecord.writeUdbytteAktier(fileName);
                        }
                        else if (fields[7] == "K")
                        {
                            kr++;
                            impRecord.setTransactionType("KR");
                            numberOfSupoerPortRecords++;
                            impRecord.writeRenteKuponer(fileName);
                        }
                    }

                    if (fields[0].CompareTo("KS") == 0)
                    {
                        ImpRecord impRecord = new ImpRecord(logger);

                        impRecord.setTransactionDate(fields[11]);

                        impRecord.setTransactionType(fields[8]);
                        impRecord.setSettlementDate(fields[12]);
                        // impRecord.setTransactionNumber(fields[4]); use SuperPorts
                        impRecord.setAmount(fields[14]);
                        impRecord.setExchangeRate(fields[15]);

                        CultureInfo cultureInfo = new CultureInfo("da-DK");
                        Decimal currencyRate = Convert.ToDecimal(fields[13], cultureInfo);
                        // Remove negative kurtage, as all kurtage is negative
                        Decimal kurtage = Convert.ToDecimal(fields[17].Replace('-', ' '), cultureInfo);
                        impRecord.setKurtage("-" + Math.Round(kurtage / currencyRate * 100, 2).ToString());
                        impRecord.setCurrenciesRate(fields[13]);
                        // take last 14 digits
                        impRecord.setAccountNumber(fields[2], false, 14);
                        // take last 14 digits
                        impRecord.setDepotNumber(fields[1], false, 14);
                        impRecord.setNota('N');
                        impRecord.setCounterPart(counterPart); // Jydske bank 

                        impRecord.setStatus('N');

                        impRecord.setCurrenciesCross(fields[9], fields[10]);

                        // use the dictionary to get the isin code
                        string idCode = fondCode.getIsin(fields[3]);
                        if (idCode.Equals(string.Empty))
                        {
                            success = false;
                            emailBody += "Try to get Isin code " + fields[3] + " but was not found in the fondcode file.\n";
                        }
                        else
                        {
                            impRecord.setIdCode(idCode);

                            // check for anuls
                            if (isAnul(fields[4]))
                            {
                                ksAnuls++;
                            }
                            else
                            {
                                ks++;
                                numberOfSupoerPortRecords++;
                                impRecord.writeKoebSalgAktier(fileName);
                            }
                        }
                    }
                }

                if (ub > 0) logger.Write("      Udbytte : " + ub);
                if (kr > 0) logger.Write("      Kupon : " + kr);
                if (ks > 0) logger.Write("      Køb Salg : " + ks);
                if (ksAnuls > 0) logger.Write("      Køb Salg, annuleret : " + ksAnuls);
            }

            return numberOfSupoerPortRecords;
        }
    }
}
