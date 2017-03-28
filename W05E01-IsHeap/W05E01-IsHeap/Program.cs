using System;
using System.IO;
using System.Linq;

namespace W05E01_IsHeap
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            int[] stdin = File.ReadAllLines("input.txt")[1].Split(' ').Select(x => int.Parse(x)).ToArray();
            if (IsHeap(ref stdin))
                Console.WriteLine("YES");
            else
                Console.WriteLine("NO");

            sw.Dispose();
        }

        static bool IsHeap(ref int[] array)
        {
            bool isHeap = true;
            for (int i = array.Length / 2 - 1; i >= 0 && isHeap; i--)
                isHeap = MinHeapify(ref array, i + 1);
            return isHeap;
        }

        static bool MinHeapify(ref int[] array, int index)
        {
            int rigthChildIndex = index * 2;
            int leftChildIndex = index * 2 - 1;

            if (leftChildIndex < array.Length && array[leftChildIndex] < array[index - 1])
                return false;
            if (rigthChildIndex < array.Length && array[rigthChildIndex] < array[index - 1])
                return false;
            return true;
        }
    }
}