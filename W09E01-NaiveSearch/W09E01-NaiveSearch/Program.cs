using System;
using System.Collections.Generic;
using System.IO;

namespace W09E01_NaiveSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            string[] stdin = File.ReadAllLines("input.txt");
            int[] result = NaiveSearch(stdin[1], stdin[0]);
            Console.WriteLine(result.Length);
            for (int i = 0; i < result.Length; i++)
                Console.Write(result[i]+1+" ");

            sw.Dispose();
        }

        static int[] NaiveSearch(string plainText, string subString)
        {
            List<int> result = new List<int>();

            for (int i = 0; i < plainText.Length - subString.Length+1; i++)
            {
                bool correct = true;
                for (int j = 0; j < subString.Length && correct; j++)
                    if (plainText[i + j] != subString[j])
                        correct = false;
                if (correct)
                    result.Add(i);
            }

            return result.ToArray();
        }
    }
}