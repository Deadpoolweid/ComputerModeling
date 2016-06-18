using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_4
{
    class dData
    {
        public  List<int> A = new List<int>();

        public  List<int> B = new List<int>();

        public  List<int> C = new List<int>();

        public  List<int> D = new List<int>();

        public  List<int> E = new List<int>();

        public  List<int> F = new List<int>();

        public  List<int> G = new List<int>();

        public  List<int> H = new List<int>();

        public dData(List<int> a, List<int> b, List<int> c, List<int> d, List<int> e, List<int> f, List<int> g, List<int> h)
        {
            A = a;
            B = b;
            C = c;
            D = d;
            E = e;
            F = f;
            G = g;
            H = h;
        }

        public  void Clear()
        {
            A = new List<int>();

            B = new List<int>();

            C = new List<int>();

            D = new List<int>();

            E = new List<int>();

            F = new List<int>();

            G = new List<int>();

            H = new List<int>();
        }
    }
}
