using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace W06E02_Garland
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            //.NET Framework 
            //Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            //.NET Core
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            string[] stdin = File.ReadAllText("input.txt").Split(' ');

            int n = int.Parse(stdin[0]);
            double a = double.Parse(stdin[1]);
            double left = 0, right = a, last = -1;
            while((right-left) > 0.0000001 /(n-1))
            {
                double mid = (left + right) / 2;
                double prev = a;
                double cur = mid;
                bool aboveGround = cur > 0;
                for(int i = 3; i<=n; i++)
                {
                    double next = 2 * cur - prev + 2;
                    aboveGround &= next > 0;
                    prev = cur;
                    cur = next;
                }
                if (aboveGround)
                {
                    right = mid;
                    last = cur;
                }
                else
                    left = mid;
            }

            Console.WriteLine(last);
            sw.Dispose();
        }
    }
}