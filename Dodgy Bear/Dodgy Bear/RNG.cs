using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DodgyBear
{
    static class RNG
    {
        static Random rand = new Random();
        static double drn;
        static int irn;

        static RNG()
        {
        }

        static public int getInt()
        {
            irn = rand.Next(5, 12);

            return irn;
        }

        static public int getInt(int x, int y)
        {
            irn = rand.Next(x, y);

            return irn;
        }


        static public double getDouble()
        {
            drn = rand.NextDouble();

            return drn;
        }
    }
}
