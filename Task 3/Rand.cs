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

        public Rand()
        {
            R = System.DateTime.Now.Millisecond;
        }

        public double Next()
        {
            R = (R*a + c)%m;
            return (double)R/m;
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
}
