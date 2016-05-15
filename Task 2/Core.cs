using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    class Core
    {
        public int[] T { get; }

        public double[] Y { get; }

        public int Count => T.Length;

        public readonly int Size;

        public SeasonComponent SeasonComponent { get; }

        public SeasonContext SeasonContext { get; }

        public Trand Trand { get; }

        public Random Random { get; }

        public Core(int[] t, double[] y, int size)
        {
            Size = size;
            T = t;
            Y = y;

            SeasonComponent = new SeasonComponent(t, Y, Size);
            SeasonContext = new SeasonContext(SeasonComponent.Value, Size);
            Trand = new Trand(t, Y, SeasonContext.SeasonComponentCorrected);
            Random = new Random(t, Y, SeasonContext.SeasonComponentCorrected, Trand.A, Trand.B);
        }
    }

    public class SeasonComponent
    {
        public SeasonComponent(IReadOnlyCollection<int> t, IReadOnlyList<double> y, int size)
        {
            var n = t.Count;

            int _size = 0;
            int p;
            for (int i = 0; i < t.Count; i++)
            {
                p = i + 1;
                if (p + size - 1 <= n)
                {
                    _size++;
                }
            }
            ThreeMonthOverall = new double[n];
            for (var i = 0; i < n; i++)
            {
                if (i >= 1 && i <= _size)
                {
                    ThreeMonthOverall[i] = 0;
                    for (int j = 0; j < size; j++)
                    {
                        ThreeMonthOverall[i] += y[i - 1 + j];
                    }
                }
                else
                {
                    ThreeMonthOverall[i] = 0;
                }
            }

            SlideAverage = new double[n];
            for (var i = 1; i < n - 1; i++)
            {
                SlideAverage[i] = ThreeMonthOverall[i] / size;
            }

            SlideAverageCenter = new double[n];
            for (var i = 2; i <= _size; i++)
            {
                SlideAverageCenter[i] = (SlideAverage[i] + SlideAverage[i - 1]) / 2;
            }

            Value = new double[n];
            for (var i = 2; i <= _size; i++)
            {
                Value[i] = y[i] / SlideAverageCenter[i];
            }
        }

        public double[] Value { get; }

        public double[] ThreeMonthOverall { get; }

        public double[] SlideAverage { get; }

        public double[] SlideAverageCenter { get; }
    }

    public class SeasonComponentSingle
    {
        public double Value;

        public double ThreeMonthOverall;

        public double SlideAverage;

        public double SlideAverageCenter;

        public SeasonComponentSingle(double value, double tmOverall, double slAvg, double slAvgCntr)
        {
            Value = value;
            ThreeMonthOverall = tmOverall;
            SlideAverage = slAvg;
            SlideAverageCenter = slAvgCntr;
        }
    }

    public class SeasonContext
    {
        public SeasonContext(double[] seasonComponent, int size)
        {
            var k = seasonComponent.Length / size;
            var p = size;
            var h = 0;
            SeasonComponent = new List<double>[k];


            for (var i = 0; i < k; i++)
            {
                SeasonComponent[i] = new List<double>();
            }

            for (var i = 0; i < seasonComponent.Length; i++)
            {
                if (i == p)
                {
                    p = p + size;
                    h++;
                }
                SeasonComponent[h].Add(seasonComponent[i]);

            }

            Overall = new double[size];
            for (var i = 0; i < Overall.Length; i++)
            {
                for (var j = 0; j < k; j++)
                {
                    Overall[i] += SeasonComponent[j][i];
                }
            }

            SeasonComponentAverage = new double[size];
            for (var i = 0; i < SeasonComponentAverage.Length; i++)
            {
                // Количество ненулевых элементов
                var u = 0;
                for (var j = 0; j < k; j++)
                {
                    if (Math.Abs(SeasonComponent[j][i]) > double.Epsilon)
                    {
                        u++;
                    }
                }
                SeasonComponentAverage[i] = Overall[i] / u;
            }

            KCorrecting = size / SeasonComponentAverage.Sum();

            SeasonComponentCorrected = new double[size];
            for (var i = 0; i < SeasonComponentCorrected.Length; i++)
            {
                SeasonComponentCorrected[i] = SeasonComponentAverage[i] * KCorrecting;
            }
        }

        public List<double>[] SeasonComponent { get; }

        public double[] Overall { get; }

        public double[] SeasonComponentAverage { get; }

        public double KCorrecting { get; }

        public double[] SeasonComponentCorrected { get; }
    }

    public class Trand
    {
        public double[] Yi;

        public double[] tSquare;

        public double[] YSquare;

        public double[] tOnY;

        public double A;

        public double B;

        public Trand(int[] t, double[] y, double[] si)
        {
            int n = t.Length;

            Yi = new double[y.Length];
            var index = 0;
            for (var i = 0; i < Yi.Length; i++)
            {
                Yi[i] = y[i] / si[index];
                index++;
                if (index == si.Length)
                {
                    index = 0;
                }
            }

            tSquare = new double[t.Length];
            for (var i = 0; i < tSquare.Length; i++)
            {
                tSquare[i] = t[i]*t[i];
            }

            YSquare = new double[Yi.Length];
            for (var i = 0; i < YSquare.Length; i++)
            {
                YSquare[i] = Yi[i] * Yi[i];
            }

            tOnY = new double[t.Length];
            for (var i = 0; i < YSquare.Length; i++)
            {
                tOnY[i] = Yi[i] * t[i];
            }

            

            B = new double();
            B = (tOnY.Sum()*n - Yi.Sum()*t.Sum())/(n*tSquare.Sum() - t.Sum()*t.Sum());

            A = new double();
            A = (Yi.Sum() - B*t.Sum())/n;
        }
    }

    public class Random
    {
        public double[] T;

        public double[] TOnS;

        public double[] E;

        public double Quality;

        public Random(int[] t, double[] y, double[] s, double a, double b)
        {
            var h = 0;

            T = new double[t.Length];
            for (var i = 0; i < T.Length; i++)
            {
                T[i] = a + b * t[i];
            }

            h = 0;
            TOnS = new double[T.Length];
            for (var i = 0; i < TOnS.Length; i++)
            {
                TOnS[i] = T[i] * s[h];
                h++;
                if (h == s.Length)
                {
                    h = 0;
                }
            }

            E = new double[T.Length];
            for (var i = 0; i < E.Length; i++)
            {
                E[i] = y[i] / TOnS[i];
            }

            int n = t.Length;
            double[] temp = new double[n];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = y[i] - TOnS[i];
                temp[i] *= temp[i];
            }
            double _temp = temp.Sum();

            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = y[i] - y.Average();
                temp[i] *= temp[i];
            }

            Quality = 1 - (_temp/temp.Sum());
        }
    }
}
