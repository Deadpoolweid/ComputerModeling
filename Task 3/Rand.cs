using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_3
{
    class Rand
    {

        private double a= 1664525;

        private double c = 1013904223;

        private double m = Math.Pow(2, 32);

        private double R;

        public double x;

        public double y;

        public Rand()
        {
            R = System.DateTime.Now.Millisecond;
        }

        public Rand(double a, double c, double m)
        {
            this.a = a;
            this.c = c;
            this.m = m;
            R = DateTime.Now.Millisecond;
        }

        public double Next()
        {
            R = (R*a + c)%m;
            var rm = (double) R/m;
            return x + (rm)*(y-x);
        }
    }

    class FRand
    {
        private int a= 17;
        private int b = 5;

        private int k;

        public FRand()
        {
            k = a + 1;
        }

        public FRand(int a, int b)
        {
            this.a = a;
            this.b = b;
            k = a + 1;
        }

        public double Next()
        {
            return f(k++);
        }

        double f(int k)
        {
            if (k < a)
            {
                double max = Math.Max(a, b);
                double[] arr = new double[(int)max];

                Rand r = new Rand();
                r.x = 0;
                r.y = 100;
                for (int i = 0; i < max; i++)
                {
                    arr[i] = r.Next();
                }

                return arr[k];
            }
            double res = f(k - a) - f(k - b);
            if (f(k - a) >= f(k - b))
            {
                return (res);
            }
            else
            {
                return (res + 1);
            }

        }
    }

    class PI
    {
        private double x, y;

        private bool Calculated = false;

        public double Value
        {
            get
            {
                if (Calculated)
                {
                    return value;
                }
                throw new Exception("Значение нужно посчитать.");
            }
        }

        private double value;

        /// <summary>
        /// Радиус окружности
        /// </summary>
        private int r;

        /// <summary>
        /// Количество точек попавших в окружность
        /// </summary>
        private double Nc;

        /// <summary>
        /// Оющее количество точек
        /// </summary>
        private double N;

        public PI(int a, double n)
        {
            this.r = a;
            this.N = n;
            Nc = 0;

            Calculate();
        }

        public void Calculate()
        {
            Nc = 0;
            Rand r = new Rand();
            r.x = 0;
            r.y = (double)this.r;
            for (int i = 0; i < N; i++)
            {
                x = r.Next();
                y = r.Next();
                if (y * y + x*x<= this.r*this.r)
                {
                    Nc++;
                }
            }

            value = ((double)Nc / (double)N) * 4;
            Calculated = true;
        }
    }
}
