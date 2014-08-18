using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace MackPortfolio.Prototyping
{
    public static class CVSParser
    {

        public static List<Dictionary<string, string>> ReadFile(string filepath)
        {
            var list = new List<Dictionary<string, string>>();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(filepath))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    string[] columns = csvReader.ReadFields();

                    while (!csvReader.EndOfData)
                    {
                        var dict = new Dictionary<string, string>();
                        string[] fields = csvReader.ReadFields();
                        if (fields == null) { break; }
                        for (var i = 0; i < columns.Length; i++)
                        {
                            dict[columns[i]] = fields[i];
                        }
                        list.Add(dict);
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }


        public static List<Dictionary<string, object>> ReadFileAlt(string filepath)
        {
            var list = new List<Dictionary<string, object>>();
            try
            {
                char[] delimiters = new char[] { ',' };
                using (StreamReader csvReader = new StreamReader(filepath))
                {
                    string[] columns = (csvReader.ReadLine()).Split(delimiters);
                    while (true)
                    {
                        var dict = new Dictionary<string, object>();
                        string line = csvReader.ReadLine();
                        if (line == null) { break; }
                        string[] fields = line.Split(delimiters);
                        for (var i = 0; i < columns.Length; i++)
                        {
                            dict[columns[i]] = fields[i];
                        }
                        list.Add(dict);
                    }
                    return list;
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }

    }
}
