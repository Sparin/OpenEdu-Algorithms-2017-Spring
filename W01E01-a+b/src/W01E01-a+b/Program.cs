using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace W01E01_a_b
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            string[] stdin = File.ReadAllLines("input.txt");
            Console.WriteLine(stdin[0].Split(' ').Sum(x => Convert.ToInt64(x)));

            sw.Dispose();
        }
    }
}
