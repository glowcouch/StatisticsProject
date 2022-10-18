using System;
using CsvHelper;

namespace StatisticsProject
{
    class Graph
    {
        static void Main(string[] args)
        {
            //Get data from Data.csv
            StreamReader reader = new StreamReader("Data.csv");
            CsvReader csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            IEnumerable<Answer> answers = csv.GetRecords<Answer>();
        }
    }
}