using System;
using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Physics
{
    class MainViewModel
    {
        public string Title { get; private set; }

        public IList<DataPoint> Points { get; private set; } 

        public PlotModel MyModel { get; private set; }

        public MainViewModel()
        {
            MyModel = new PlotModel
            {
                Title = "Electric field"
            };

            DataPoint[] points = new DataPoint[]
            {
                new DataPoint(0.2,0.2),
                new DataPoint(0.8,0.8),
                new DataPoint(0.2,0.8),
                new DataPoint(0.8,0.2),
                new DataPoint(0.2,0.5),
                new DataPoint(0.5,0.5),
                new DataPoint(0.8,0.5),     
            };

            var q = new double[]
            {
                -1,
                -1,
                -1,
                -1,
                -1,
                -1,
                -1
            };

            Electric e = new Electric(points,q);
            

            var series = new ContourSeries
            {
                ColumnCoordinates = ArrayBuilder.CreateVector(e.getPoints().Min2D(),e.getPoints().Max2D(),1d),
                RowCoordinates = ArrayBuilder.CreateVector(e.getPoints().Min2D(), e.getPoints().Max2D(), 1d),
                Data = e.getPoints(),
                //ContourLevels = e.G,
            };


            //data = ArrayBuilder.Evaluate(peaksAlt, x,y);


            //new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)")

            MyModel.Series.Add(series);


            //this.Title = "Example";


            //Points = new List<DataPoint>
            //{
            //    new DataPoint(0,4),
            //    new DataPoint(10,13),
            //    new DataPoint(20,15),
            //    new DataPoint(30,16),
            //    new DataPoint(40,12),
            //    new DataPoint(50,12)
            //};

        }

        private static Func<double, double, double> peaks = (x, y) =>
            3 * (1 - x) * (1 - x) * Math.Exp(-(x * x) - (y + 1) * (y + 1))
            - 10 * (x / 5 - x * x * x - y * y * y * y * y) * Math.Exp(-x * x - y * y)
            - 1.0 / 3 * Math.Exp(-(x + 1) * (x + 1) - y * y);

        private static Func<double, double, double> peaksAlt = (x, y) =>
            x+y;
    }
}
