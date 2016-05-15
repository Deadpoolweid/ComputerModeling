using System.Collections.Generic;

namespace Task1
{
    internal class SeasonComponent
    {
        public SeasonComponent(IReadOnlyCollection<int> t, IReadOnlyList<double> y, int size)
        {
            var n = t.Count;
            ThreeMonthOverall = new double[n];
            for (var i = 1; i < n-1; i++)
            {
                ThreeMonthOverall[i] = y[i - 1] + y[i] + y[i + 1];
            }

            SlideAverage = new double[n];
            for (var i = 1; i < n-1; i++)
            {
                SlideAverage[i] = ThreeMonthOverall[i]/size;
            }

            SlideAverageCenter = new double[n];
            for (var i = 2; i < n-1; i++)
            {
                SlideAverageCenter[i] = (SlideAverage[i] + SlideAverage[i - 1])/2;
            }

            Value = new double[n];
            for (var i = 2; i < n-1; i++)
            {
                Value[i] = y[i] - SlideAverageCenter[i];
            }
        }

        public double[] Value { get; }

        public double[] ThreeMonthOverall { get; }

        public double[] SlideAverage { get; }

        public double[] SlideAverageCenter { get; }
    }
}