using System;
using System.Globalization;

// Beware danske bank is using decimal point, hence culrura varial en-US
namespace Converter
{
    class Nordea
    {
        static Logger logger;
        String[] lines;
        NordeaDepot nordeaDepot;
        int numberOfSupoerPortRecords;

        public Nordea(String [] lines, ref NordeaDepot nordeaDepot, Logger l)
        {
            this.lines = lines;
            this.nordeaDepot = nordeaDepot;
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

            logger.Write("    Nordea format");

            int hdlobl = 0;
            int udbakt = 0;
            int kntopd = 0;
            int hdlakt = 0;
            int rntobl = 0;
            int rntknt = 0;
            int gbrtrn = 0;
            int behold = 0;

            if (lines.Length > 0)
            {
                for (int k = 0; k < lines.Length; k++)
                {
                    string[] fields = lines[k].Split((char)31);
                    if (debugLevel)
                    {
                        logger.WriteFields(fields);
                    }

                    if (lines[k].IndexOf("UDBAKT") == 0)
                    {
                        udbakt++;
                        ImpRecord impRecord = new ImpRecord(logger);
                        
                        if (fields.Length < 58)
                        {
                            emailBody += Environment.NewLine + "Nordea UDBAKT record " + udbakt + " has too few fields";
                            logger.Write("      Udbytte aktier record too few fields");
                        }
                        else
                        {
                            impRecord.setIdCode(fields[4]);
                            impRecord.setSettlementDate(fields[11]);
                            impRecord.setTransactionDate(fields[11]); // not fields[3] as it has to be the same as settlementdate
                            impRecord.setPrice(fields[17]);
                            // yeield tax should be decucted - hence negativ and the dash infront of the field
                            impRecord.setYieldTax("-" + fields[24]);
                            // take last 14 digits
                            impRecord.setAccountNumber(fields[30], false, 10);

                            impRecord.setCurrenciesRate(fields[56]);

                            // take last 14 digits
                            // impRecord.setDepotNumber(fields[34], false, 14);
                            string depot = nordeaDepot.getDepot(impRecord.getAccountNumber());
                            if (depot.Equals(string.Empty))
                            {
                                success = false;
                                string acc = fields[30];
                                acc = acc.Trim();
                                acc = acc.TrimStart('0');
                                emailBody += "Try to get Nordea Depot code for account " + acc + " but was not found in the Nordea depot file.\n";
                            }
                            impRecord.setDepotNumber("0000000000" + depot, false, 10);

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
                            emailBody += Environment.NewLine + "Nordea KNTOPD record " + kntopd + " has too few fields";
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
                            impRecord.setAccountNumber(fields[30], false, 10);
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

                        if (fields.Length < 58)
                        {
                            emailBody += Environment.NewLine + "Nordea HDLAKT record " + hdlakt + " has too few fields";
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
                            Decimal currencyRate = Convert.ToDecimal(fields[56], cultureInfo);
                            Decimal kurtage = Convert.ToDecimal(fields[21], cultureInfo);
                            Decimal cost = Convert.ToDecimal(fields[20], cultureInfo);
                            impRecord.setKurtage("-" + Math.Round(cost + kurtage / currencyRate * 100, 2).ToString());

                            //impRecord.setKurtage("-" + fields[21]);
                            impRecord.setCurrenciesRate(fields[56]);
                            impRecord.setYieldTax("-" + fields[24]);
                            // take last 14 digits
                            impRecord.setAccountNumber(fields[30], false, 10);
                            // take last 14 digits

                            string depot = nordeaDepot.getDepot(impRecord.getAccountNumber());
                            if (depot.Equals(string.Empty))
                            {
                                success = false;
                                string acc = fields[30];
                                acc = acc.Trim();
                                acc = acc.TrimStart('0');
                                emailBody += "Try to get Nordea Depot code for account " + acc + " but was not found in the Nordea depot file.\n";
                            }
                            impRecord.setDepotNumber("0000000000" + depot, false, 10);

                            impRecord.setNota('N');
                            impRecord.setCounterPart("NO"); // Nordea


                            impRecord.setStatus('N');

                            impRecord.setCurrenciesCross(fields[55], fields[53]);

                            numberOfSupoerPortRecords++;
                            impRecord.writeKoebSalgAktier(fileName);
                        }
                    }
                    else if (lines[k].Length > 12 && lines[k][10] == 31 && lines[k][11] == 31)
                    {
                        behold++;
                        ImpRecord impRecord = new ImpRecord(logger);

                        if (fields.Length < 6)
                        {
                            emailBody += Environment.NewLine + "Nordea BEHOLD record " + behold + " has too few fields";
                            logger.Write("      Konto opdatering record too few fields (" + fields.Length + ")");
                        }
                        else
                        {
                            // TODO
                        }
                    }
                    else if (lines[k].Length > 12 && lines[k][10] == 31)
                    {
                        kntopd++;
                        ImpRecord impRecord = new ImpRecord(logger);

                        if (fields.Length < 9)
                        {
                            emailBody += Environment.NewLine + "Nordea KNTOPD record " + kntopd + " has too few fields";
                            logger.Write("      Konto opdatering record too few fields (" + fields.Length + ")");
                        }
                        else
                        {
                            // TODO
                        }
                    }
                    else
                    {
                        // success = false; It is ok with unknown danske bank formats
                        logger.Write("      Ukendt record format, " + lines[k].Split((char)31)[0]);
                    }
                }
                if (hdlobl > 0) logger.Write("      Handel med obligationer og pantebreve : " + hdlobl);
                if (udbakt > 0) logger.Write("      Udbytte aktier : " + udbakt);
                if (hdlakt > 0) logger.Write("      Handel med aktier : " + hdlakt);
                if (rntobl > 0) logger.Write("      Kupon rente for obligationer og pantebreve : " + rntobl);
                if (rntknt > 0) logger.Write("      Rente konto : " + rntknt);
                if (gbrtrn > 0) logger.Write("      Ren-gebyrte konto : " + gbrtrn);
                if (kntopd > 0) logger.Write("      Konto opdatering : " + kntopd);
                if (behold > 0) logger.Write("      Konto beholdning : " + behold);
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
