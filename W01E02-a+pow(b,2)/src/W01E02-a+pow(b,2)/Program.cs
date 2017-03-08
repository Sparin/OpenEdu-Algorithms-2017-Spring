using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace W01E02_a_pow_b_2_
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            string[] stdin = File.ReadAllLines("input.txt")[0].Split(' ');
            Console.WriteLine(long.Parse(stdin[0]) + long.Parse(stdin[1]) * long.Parse(stdin[1]));

            sw.Dispose();
        }
    }
}
