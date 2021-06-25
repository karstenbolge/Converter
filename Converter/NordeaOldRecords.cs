using System;
using System.Collections.Generic;
using System.IO;

namespace Converter
{
    class NordeaOldRecords
    {
        static Logger logger;
        String filePath;
        List<string> records;

        public NordeaOldRecords(string folder, Logger l)
        {
            filePath = folder + "\\NordeOldRecords.txt";
            logger = l;

            records = new List<string>();
        }

        public bool readFile()
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(filePath);
                for (int i = 0; i < lines.Length; i++)
                {
                    records.Add(lines[i]);
                }
            }
            catch (Exception e)
            {
                logger.Write(e.Message);
                return false;
            }

            return true;
        }

        public bool writeFile()
        {
            try
            {
                using (StreamWriter w = File.CreateText(filePath))
                {
                    for (int i = 0; i < records.Count; i++)
                    {
                        bool skip = false;
                        string[] fields = records[i].Split((char)31);

                        DateOnly transDate = new DateOnly(logger);
                        if (fields[0] == "UDBAKT" || fields[0] == "KNTOPD")
                        {
                            transDate.setDate(fields[11]);

                            if (DateTime.Today.Month > transDate.getMonth() + 2) skip = true;
                            if (DateTime.Today.Year > 2000 + transDate.getYear() && DateTime.Today.Month > 1) skip = true;
                        }
                        else
                        {
                            skip = true;
                        }

                        if (!skip)
                        {
                            w.WriteLine(records[i]);
                        }
                        else
                        {
                            logger.Write("Skipping old record " + records[i]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Write(e.Message);
                return false;
            }

            return true;
        }

        public void addRecord(string record)
        {
            records.Add(record);
        }

        public bool previousRecord(string record)
        {
            for (int i = 0; i < records.Count; i++)
            {
                if (records[i].CompareTo(record) == 0) return true;
            }

            return false;
        }
    }
}
