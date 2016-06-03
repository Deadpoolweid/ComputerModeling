using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_3
{
    class Data
    {
        public static List<double> rand = new List<double>();

        public static List<double> randF = new List<double>();

        public static List<double> pi = new List<double>();

        public static void AddRand(double x)
        {
            rand.Add(x);
        }

        public static void AddRandF(double x)
        {
            randF.Add(x);
        }

        public static void AddPi(double x)
        {
            pi.Add(x);
        }

        public static void Clear(Lists l)
        {
            switch (l)
            {
                case Lists.rand:
                    rand.Clear();
                    break;
                case Lists.randF:
                    randF.Clear();
                    break;
                case Lists.pi:
                    pi.Clear();
                    break;
            }
        }
    }

    enum Lists
    {
        rand = 0,
        randF = 1,
        pi = 2
    }
}
