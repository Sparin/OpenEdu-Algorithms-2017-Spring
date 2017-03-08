using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace W02E03_Anti_QuickSort
{
    public class Program
    {
        static void Main(string[] args)
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            string[] stdin = File.ReadAllLines("input.txt");

            int n = int.Parse(stdin[0]);
            int[] array = new int[n];
            for (int i = 0; i < n; i++)
                array[i] = i + 1;

            for (int i = 2; i < n; i++)
            {
                int temp = array[i / 2];
                array[i / 2] = array[i];
                array[i] = temp;
            }

            for (int i = 0; i < n; i++)
                Console.Write("{0} ", array[i]);

            sw.Dispose();
        }
    }
}
