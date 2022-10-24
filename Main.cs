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

        static int graphPadding = 100;
      
        static Answer[] answers;

        static void Main(string[] args)
        {
            //Get data from Data.csv
            StreamReader reader = new StreamReader("Data.csv");
            CsvReader csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            answers = csv.GetRecords<Answer>().ToList<Answer>().ToArray<Answer>();

            //Draw all the charts
            WPMBooksWeekScreenTime(20);    
        }

        static void WPMBooksWeekScreenTime(int dotDiameter)
        {
            Image image = new Image<Rgba32>(graphWidth, graphHeight);

            image.Mutate(x=> x.Fill(Color.White, new Rectangle(new Point(0,0), new Size(graphWidth, graphHeight))));

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
                image.Mutate(x=> x.Fill(Color.Black, new PathBuilder().AddArc(
                    new PointF(((float)answers[i].WPM/(float)maxWPM)*(graphWidth-graphPadding*2)-graphPadding, 
                    graphHeight-(((float)answers[i].BooksWeek/(float)maxBooksWeek)*(graphHeight-graphPadding*2))-graphPadding), 
                    (answers[i].ScreenTime*5)+5, (answers[i].ScreenTime*5)+5, 0, 0, 360)
                    .Build()));
            }

            //Find the averages
            float meanWPM = 0;
            float meanScreenTime = 0;
            float meanBooksWeek = 0;

            foreach (Answer answer in answers)
            {
                meanWPM += answer.WPM;
                meanScreenTime += answer.ScreenTime;
                meanBooksWeek += answer.BooksWeek;
            }

            meanWPM = meanWPM/answers.Length;
            meanScreenTime = meanScreenTime/answers.Length;
            meanBooksWeek = meanBooksWeek/answers.Length;

            //Draw the averages
            image.Mutate(x=> x.Fill(Color.Red, new PathBuilder().AddArc(
                    new PointF(((float)meanWPM/(float)maxWPM)*(graphWidth-graphPadding*2)-graphPadding, 
                    graphHeight-(((float)meanBooksWeek/(float)maxBooksWeek)*(graphHeight-graphPadding*2))-graphPadding), 
                    (meanScreenTime*5)+5, (meanScreenTime*5)+5, 0, 0, 360)
                    .Build()));

            image.Save("WPMBooksWeek.png");
        }
    }
}