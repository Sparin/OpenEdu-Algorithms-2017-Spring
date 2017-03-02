using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W01E05_Swap;

namespace W01E05_Swap
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            string[] stdin = File.ReadAllLines("input.txt")[1].Split(' ');
            Element[] array = new Element[stdin.Length];
            for (int i = 0; i < stdin.Length; i++)
                array[i] = new Element
                {
                    StartIndex = i,
                    CurrentIndex = i,
                    Value = long.Parse(stdin[i])
                };
            array = InsertionSort(array);

            LogSort(array);
            Console.WriteLine("No more swaps needed.");
            foreach (Element item in array)
                Console.Write("{0} ", item.Value);

            sw.Dispose();
        }

        public static Element[] InsertionSort(Element[] array)
        {
            for (int j = 0; j < array.Length; j++)
            {
                int i = j - 1;
                while (i >= 0 && array[i].CompareTo(array[i + 1]) > 0)
                    Swap(ref array, i--, i + 1);
            }
            return array;
        }

        public static void LogSort(Element[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Element s = array.First(x => x.StartIndex == array[i].CurrentIndex);
                if (s.StartIndex != array[i].StartIndex)
                {                    
                    if (s.StartIndex < array[i].StartIndex)
                        Console.WriteLine("Swap elements at indices {0} and {1}.", s.StartIndex + 1, array[i].StartIndex + 1);
                    else
                        Console.WriteLine("Swap elements at indices {0} and {1}.", array[i].StartIndex + 1, s.StartIndex + 1);
                    
                    int temp = s.StartIndex;         
                    s.StartIndex = array[i].StartIndex;
                    array[i].StartIndex = temp;
                }
            }
        }

        public static void Swap(ref Element[] array, int firstIndex, int secondIndex)
        {
            array[secondIndex].CurrentIndex = firstIndex;
            array[firstIndex].CurrentIndex = secondIndex;

            Element temp = array[firstIndex];
            array[firstIndex] = array[secondIndex];
            array[secondIndex] = temp;
        }

    }
    public class Element
    {
        public int CurrentIndex { get; set; }
        public int StartIndex { get; set; }
        public long Value { get; set; }

        public int CompareTo(Element item)
        {
            return Value.CompareTo(item.Value);
        }
    }
}
