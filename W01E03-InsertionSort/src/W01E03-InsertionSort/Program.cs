using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace W01E03_InsertionSort
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            string[] stdin = File.ReadAllLines("input.txt");
            long[] inputArray = stdin[1].Split(' ').Select(x => long.Parse(x)).ToArray();
            inputArray = InsertionSort(inputArray);
            Console.WriteLine();
            foreach (long item in inputArray)
                Console.Write("{0} ", item);

            sw.Dispose();
        }

        public static T[] InsertionSort<T>(T[] array) where T : IComparable<T>
        {
            for (int j = 0; j < array.Length; j++)
            {
                int i = j - 1;                
                while (i >= 0 && array[i].CompareTo(array[i + 1]) > 0)
                {
                    T temp = array[i];
                    array[i] = array[i + 1];
                    array[i + 1] = temp;
                    i--;
                }
                Console.Write("{0} ", i+2);
            }
            return array;
        }
    }
}
