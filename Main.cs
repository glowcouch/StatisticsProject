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

        static int graphPadding = 200;
      
        static Answer[] answers;

        static void Main(string[] args)
        {
            //Get data from Data.csv
            StreamReader reader = new StreamReader("Data.csv");
            CsvReader csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture);
            answers = csv.GetRecords<Answer>().ToList<Answer>().ToArray<Answer>();

            //Draw all the charts
            WPMBooksWeekScreenTime(20, 5, 20, 20, 30);    
        }

        static void WPMBooksWeekScreenTime(int dotDiameter, int axisesThickness, int ticksX, int ticksY, int tickLength)
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

            //Draw the axises

            Pen axisesPen = Pens.Solid(Color.Grey, axisesThickness);

            //X
            image.Mutate(x=> x.Draw(axisesPen, new PathBuilder().AddLine(new PointF(0, graphHeight-((graphPadding/3)*2)), new PointF(graphWidth, graphHeight-((graphPadding/3)*2))).Build()));

            for (int i = graphPadding; i < (graphWidth-(graphPadding*2)); i +=(graphWidth-(graphPadding*2))/ticksX)
            {
              image.Mutate(x=> x.Draw(axisesPen, new PathBuilder().AddLine(new PointF(i, graphHeight-((graphPadding/3)*2)), new PointF(i, graphHeight-((graphPadding/3)*2)+tickLength)).Build()));
            }

            //Y
            image.Mutate(x=> x.Draw(axisesPen, new PathBuilder().AddLine(new PointF((graphPadding/3)*2, 0), new PointF((graphPadding/3)*2, graphHeight)).Build()));

            for (int i = graphHeight-graphPadding; i > 0; i -=(graphHeight-(graphPadding*2))/ticksY)
            {
              image.Mutate(x=> x.Draw(axisesPen, new PathBuilder().AddLine(new PointF((graphPadding/3)*2, i), new PointF(((graphPadding/3)*2)-tickLength, i)).Build()));
            }

            image.Save("WPMBooksWeek.png");
        }
    }
}