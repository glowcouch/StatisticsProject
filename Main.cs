using System;
using CsvHelper;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace StatisticsProject
{
    class Graph
    {
        static int graphWidth = 1920;
        static int graphHeight = 1080;    
        static Answer[] answers;

        static void Main(string[] args)
        {
            //Get data from Data.csv
            StreamReader reader = new StreamReader("Data.csv");
            CsvReader csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            answers = csv.GetRecords<Answer>().ToList<Answer>().ToArray<Answer>();

            //Draw all the charts
            WPMBooksWeek(20);    
        }

        static void WPMBooksWeek(int dotDiameter)
        {
            Image image = new Image<Rgba32>(graphWidth, graphHeight);

            int maxWPM = 0;
            float maxBooksWeek = 0;

            //Find the highest WPM
            foreach(Answer answer in answers)
            {
                if (maxWPM < answer.WPM)
                {
                    maxWPM = answer.WPM;
                }
            }

            //Find the highest BooksWeek
            foreach(Answer answer in answers)
            {
                if (maxBooksWeek < answer.BooksWeek)
                {
                    maxBooksWeek = answer.BooksWeek;
                }
            }

            for (int i = 0; i < answers.Length; i++)
            {
                image.Mutate(x=> x.Fill(Color.Cyan, new PathBuilder().AddArc(
                    new PointF(((float)answers[i].WPM/(float)maxWPM)*graphWidth, 
                    graphHeight-(((float)answers[i].BooksWeek/(float)maxBooksWeek)*graphHeight)), 
                    dotDiameter/2, dotDiameter/2, 0, 0, 360)
                    .Build()));
            }

            image.Save("WPMBooksWeek.png");
        }
    }
}