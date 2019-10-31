using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Converter
{
    class Nykredit
    {
        static Logger logger;
        String[] lines;
        int numberOfSupoerPortRecords;

        public Nykredit(String[] lines, Logger l)
        {
            this.lines = lines;
            logger = l;
        }

        public int Process(ref string emailBody, ref bool debugLevel, ref bool success, string fileName)
        {
            numberOfSupoerPortRecords = 0;

            logger.Write("    Nykrdit format");

            // Removing qoutes
            if (lines[0].IndexOf("\"") == 0)
            {
                for (int k = 1; k < lines.Length; k++)
                {
                    lines[k] = lines[k].Substring(1, lines[k].Length - 2);
                }
            }

            int ks = 0;
            int ub = 0;
            int ud = 0;
            int an = 0;
            int db = 0;
            int kb = 0;
            int dk = 0;

            if (lines.Length > 2)
            {
                for (int k = 1; k < lines.Length - 1; k++)
                {
                    string[] fields = lines[k].Split(';');
                    if (debugLevel)
                    {
                        logger.WriteFields(fields);
                    }

                    if (lines[k].IndexOf("KS;") == 0)
                    {
                        ks++;
                        ImpRecord impRecord = new ImpRecord(logger);
                        
                        if (fields.Length < 35)
                        {
                            emailBody += Environment.NewLine + "Nykredit KS record " + ks + " has too few fields";
                            logger.Write("      KS record too few fields");
                        }
                        else
                        {
                            impRecord.setDepotNumber(fields[1]);
                            impRecord.setAccountNumber(fields[2]);
                            impRecord.setIdCode(fields[3]);
                            // impRecord.setTransactionNumber(fields[5]); use SuperPorts
                            impRecord.setTransactionDate(fields[13]);
                            impRecord.setSettlementDate(fields[14]);
                            if (fields[6][0] == 'P') // PENDING
                            {
                                impRecord.setStatus('O');
                            }
                            else if (fields[6][0] == 'S') // SETTLED
                            {
                                impRecord.setStatus('N');
                            }
                            else // CANCELLED
                            {
                                impRecord.setStatus('D');
                            }

                            if (fields[10] == "S")
                            {
                                impRecord.setTransactionType("S");
                            }
                            else if (fields[10] == "B")
                            {
                                impRecord.setTransactionType("K");
                            }
                            else
                            {
                                logger.Write("      KS unknown transaction type" + fields[10]);
                            }

                            impRecord.setCurrenciesCross(fields[11] + "/" + fields[12]);
                            impRecord.setCurrenciesRate(fields[15]);
                            impRecord.setAmount(fields[16]);
                            impRecord.setPrice(fields[27]);
                            impRecord.setCost(fields[22]);

                            impRecord.setUnknown("000000737632.00000000000");

                            numberOfSupoerPortRecords++;
                            impRecord.writeKoebSalgAktier(fileName);
                        }
                    }
                    else if (lines[k].IndexOf("UB;") == 0)
                    {
                        ub++;
                        ImpRecord impRecord = new ImpRecord(logger);
                        
                        if (fields.Length < 23)
                        {
                            emailBody += Environment.NewLine + "Nykredit UB record " + ub + " has too few fields";
                            logger.Write("      UB record too few fields");
                        }
                        else
                        {
                            impRecord.setDepotNumber(fields[1]);
                            impRecord.setAccountNumber(fields[2]);
                            impRecord.setIdCode(fields[3]);
                            // impRecord.setTransactionNumber(fields[4].Substring(0, fields[4].Length < 8 ? fields[4].Length : 8)); use SuperPorts
                            if (fields[5][0] == 'N' || fields[5][0] == 'A') // NEW // AMENDED
                            {
                                impRecord.setStatus('N');
                            }
                            else // CANCELLED
                            {
                                impRecord.setStatus('D');
                            }
                            if (fields[7] == "A")
                            {
                                impRecord.setTransactionType("U");
                            }
                            else if (fields[7] == "K")
                            {
                                impRecord.setTransactionType("KR");
                            }
                            else
                            {
                                logger.Write("      UB unknown transaction type" + fields[10]);
                            }
                            impRecord.setCurrenciesCross(fields[8] + "/" + fields[9]);
                            impRecord.setTransactionDate(fields[10]);
                            impRecord.setSettlementDate(fields[11]);
                            impRecord.setCurrenciesRate(fields[12]);
                            impRecord.setAmount(fields[15]);
                            impRecord.setPrice(fields[21]);
                            impRecord.setCost(fields[19]);

                            if (debugLevel)
                            {
                                logger.WriteFields(fields);
                            }

                            numberOfSupoerPortRecords++;
                            impRecord.writeUdbytteAktier(fileName);
                        }
                    }
                    else if (lines[1].IndexOf("UD;") == 0)
                    {
                        ud++;
                    }
                    else if (lines[1].IndexOf("AN;") == 0)
                    {
                        an++;
                    }
                    else if (lines[1].IndexOf("DB;") == 0)
                    {
                        db++;
                    }
                    else if (lines[1].IndexOf("KB;") == 0)
                    {
                        kb++;
                    }
                    else if (lines[1].IndexOf("DK;") == 0)
                    {
                        dk++;
                    }
                    else if (lines[k] == String.Empty)
                    {

                    }
                    else
                    {
                        success = false;
                        logger.Write("      Ukendt fil format, " + lines[k]);
                    }
                }
                if (ks > 0) logger.Write("      Depotbevægelser : " + ks);
                if (ub > 0) logger.Write("      Udbytte, Aktier og obligationer : " + ub);
                if (ud > 0) logger.Write("      Udtrukne obligationer : " + ud);
                if (an > 0) logger.Write("      Kontobevægelse : " + an);
                if (db > 0) logger.Write("      Depotbeholdninger : " + db);
                if (kb > 0) logger.Write("      Kontantbeholdninger : " + kb);
                if (dk > 0) logger.Write("      Relationer mellem depoter og konti : " + dk);
            }
            else
            {
                if (lines.Length == 2 && lines[1].IndexOf("TAIL;") == 0)
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
