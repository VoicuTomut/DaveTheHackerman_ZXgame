using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public struct Fraction
{
    public Fraction(int n, int d)
    {
        N = n;
        D = d;
    }

    public int N { get; private set; }
    public int D { get; private set; }
}

