using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;

namespace Physics
{
    class Electric
    {
        /// <summary>
        /// Количество зарядов
        /// </summary>
        public int K;

        /// <summary>
        /// Координаты местонахождения заряда
        /// </summary>
        public OxyPlot.DataPoint[] point;

        /// <summary>
        /// Величины зарядов
        /// </summary>
        public double[] q;

        public Dictionary<DataPoint, double> charge;

        /// <summary>
        /// Количество изолиний
        /// </summary>
        public int L;

        /// <summary>
        /// Значения потенциала для изолиний
        /// </summary>
        public double[] G;

        /// <summary>
        /// Массив с новыми точками
        /// </summary>
        public DataPoint[] newPoints;

        public double[,] getPoints()
        {
            double[,] points;

            int o = list.Sum(l => l.Count());

            points = new double[o,2];

            o = 0;

            foreach (var l in list)
            {
                foreach (var point in l)
                {
                    points[o, 0] = point.X;
                    points[o, 1] = point.Y;
                    o++;
                }
            }


            return points;
        }

        public List<DataPoint[]> list = new List<DataPoint[]>(); 

        public Electric(DataPoint[] points, double[] q)
        {
            this.point = points;
            this.q = q;

            K = q.Length;

            calcF();
            calcG();
            calcNewPoints();
        }

        /// <summary>
        /// Потенциал для узлов
        /// </summary>
        private double[,] F = new double[N,N];

        private const int N = 100;

        /// <summary>
        /// Расчёт потенциала в сетке
        /// </summary>
        private void calcF()
        {
            for (int j = 0; j < N; j++)
            {
                for (int i = 0; i < N; i++)
                {
                    for (int k = 0; k < K; k++)
                    {
                        double R = Math.Sqrt(Math.Pow((double)i / N - point[k].X, 2) + Math.Pow((double)j / N - point[k].Y, 2));

                        if (R >= 1e-6)
                        {
                            F[i, j] += (q[k]/R);
                        }
                        else
                        {
                            F[i, j] += q[k];
                        }
                        const double kE = 1/(4*Math.PI * 8.85418781762e-12);


                    }
                }
            }
        }

        /// <summary>
        /// Вычисляет диапозон возможных потенциалов для интерполяции
        /// </summary>
        private void calcG()
        {
            double max, min;
            max = F.Max2D();
            min = F.Min2D();

            List<double> charges = new List<double>();

            for (int i = (int)Math.Floor(min); i <= Math.Ceiling(max); i++)
            {
                charges.Add(i);
            }

            while (charges.Count<5)
            {
                charges.Add(charges.Max()+1);
            }

            G = charges.ToArray();
        }

        /// <summary>
        /// Расчитывает списки с новыми точками
        /// </summary>
        private void calcNewPoints()
        {
            List<DataPoint> points = new List<DataPoint>();

            foreach (var g in G)
            {
                points.Clear();
                for (int j = 0; j < N-1; j++)
                {
                    for (int i = 0; i < N-1; i++)
                    {
                        if ((F[i, j] - g)*(F[i+1, j] - g) < 0)
                        {
                            points.Add(new DataPoint(j+(g-F[i,j])/(F[i,j+1]-g),i));
                        }
                    }
                }
                for (int j = 0; j < N - 1; j++)
                {
                    for (int i = 0; i < N - 1; i++)
                    {
                        if ((F[i,j] - g) * (F[i,j+1] - g) < 0)
                        {
                            points.Add(new DataPoint(j, i + (g-F[i,j])/(F[i+1,j]-g)));
                        }
                    }
                }
                list.Add(points.ToArray());
            }
            newPoints = points.ToArray();
        }

        public double calcF(DataPoint knot)
        {
            double F = 0;
            for (int k = 0; k < K; k++)
            {
                double R = Math.Sqrt(Math.Pow(point[k].X - (double)knot.X, 2) + Math.Pow(point[k].Y - (double)knot.Y, 2));

                const double kE = 1 / (4 * Math.PI * 8.85418781762e-12);

                F += -kE * (q[k] / R);
            }
            return F;
        }

        //public Func<double, double, double> f = (x, y) =>
        //{
        //    double F = 0;
        //    for (int k = 0; k < K; k++)
        //    {
        //        double R = Math.Sqrt(Math.Pow(point[k].X - (double)x, 2) + Math.Pow(point[k].Y - (double)y, 2));

        //        const double kE = 1 / (4 * Math.PI * 8.85418781762e-12);

        //        F += -kE * (q[k] / R);
        //    }
        //    return F;
        //};

    }
}
