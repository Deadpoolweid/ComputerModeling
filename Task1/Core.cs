﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Task1
{
    public class Core
    {
        public int[] T { get; }

        public double[] Y { get; }

        public int Count => T.Length;

        public readonly int Size;

        public SeasonComponent SeasonComponent { get; }

        public SeasonContext SeasonContext { get; }

        public Trand Trand { get;  }

        public Random Random { get; }

        public Core(int[] t, double[] y, int size)
        {
            Size = size;
            T = t;
            Y = y;

            SeasonComponent = new SeasonComponent(t,Y,Size);
            SeasonContext = new SeasonContext(SeasonComponent.Value,Size);
            Trand = new Trand(t,Y,SeasonContext.SeasonComponentCorrected);
            Random = new Random(t,Y,SeasonContext.SeasonComponentCorrected,Trand.A,Trand.B,Trand.DeltaYSquare.Sum());
        }
    }

    public class SeasonComponent
    {
        public SeasonComponent(IReadOnlyCollection<int> t, IReadOnlyList<double> y, int size)
        {
            var n = t.Count;
            ThreeMonthOverall = new double[n];
            for (var i = 1; i < n - 1; i++)
            {
                ThreeMonthOverall[i] = y[i - 1] + y[i] + y[i + 1];
            }

            SlideAverage = new double[n];
            for (var i = 1; i < n - 1; i++)
            {
                SlideAverage[i] = ThreeMonthOverall[i] / size;
            }

            SlideAverageCenter = new double[n];
            for (var i = 2; i < n - 1; i++)
            {
                SlideAverageCenter[i] = (SlideAverage[i] + SlideAverage[i - 1]) / 2;
            }

            Value = new double[n];
            for (var i = 2; i < n - 1; i++)
            {
                Value[i] = y[i] - SlideAverageCenter[i];
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
                SeasonComponentAverage[i] = Overall[i]/u;
            }

            KCorrecting = SeasonComponentAverage.Sum()/size;

            SeasonComponentCorrected = new double[size];
            for (var i = 0; i < SeasonComponentCorrected.Length; i++)
            {
                SeasonComponentCorrected[i] = SeasonComponentAverage[i] - k;
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

        public double[] Deltat;

        public double[] DeltaY;

        public double[] DeltatSquare;

        public double[] DeltatOndeltaY;

        public double[] DeltaYSquare;

        public double A;

        public double B;

        public Trand(int[] t, double[] y, double[] si)
        {
            Yi = new double[y.Length];
            var index = 0;
            for (var i = 0; i < Yi.Length; i++)
            {
                Yi[i] = y[i] - si[index];
                index++;
                if (index == si.Length)
                {
                    index = 0;
                }
            }

            Deltat = new double[t.Length];
            for (var i = 0; i < Deltat.Length; i++)
            {
                Deltat[i] = t[i] - t.AsQueryable().Average();
            }

            DeltaY = new double[y.Length];
            for (var i = 0; i < DeltaY.Length; i++)
            {
                DeltaY[i] = Yi[i] - Yi.AsQueryable().Average();
            }

            DeltatSquare = new double[Deltat.Length];
            for (var i = 0; i < Deltat.Length; i++)
            {
                DeltatSquare[i] = Deltat[i]*Deltat[i];
            }

            DeltatOndeltaY = new double[Deltat.Length];
            for (var i = 0; i < DeltatOndeltaY.Length; i++)
            {
                DeltatOndeltaY[i] = Deltat[i]*DeltaY[i];
            }

            DeltaYSquare = new double[DeltaY.Length];
            for (var i = 0; i < DeltaYSquare.Length; i++)
            {
                DeltaYSquare[i] = DeltaY[i]*DeltaY[i];
            }

            B = new double();
            B = DeltatOndeltaY.Average()/DeltatSquare.Sum();

            A = new double();
            A = Yi.AsQueryable().Average() -  B*t.AsQueryable().Average();
        }
    }

    public class Random
    {
        public double[] TplusE;

        public double[] T;

        public double[] TplusS;

        public double[] E;

        public double[] ESquare;

        public double Quality;

        public Random(int[] t, double[] y, double[] s, double a, double b, double deltaYSquareSum)
        {
            var h = 0;

            TplusE = new double[t.Length];
            for (var i = 0; i < TplusE.Length; i++)
            {
                TplusE[i] = y[i] - s[h];
                h++;
                if (h == s.Length)
                {
                    h = 0;
                }
            }

            T = new double[t.Length];
            for (var i = 0; i < T.Length; i++)
            {
                T[i] = a + b*t[i];
            }

            h = 0;
            TplusS = new double[T.Length];
            for (var i = 0; i < TplusS.Length; i++)
            {
                TplusS[i] = T[i] + s[h];
                h++;
                if (h == s.Length)
                {
                    h = 0;
                }
            }

            E = new double[T.Length];
            for (var i = 0; i < E.Length; i++)
            {
                E[i] = y[i] - TplusS[i];
            }

            ESquare = new double[E.Length];
            for (var i = 0; i < ESquare.Length; i++)
            {
                ESquare[i] = E[i]*E[i];
            }

            Quality = ESquare.Sum()/deltaYSquareSum;
        }
    }
}
