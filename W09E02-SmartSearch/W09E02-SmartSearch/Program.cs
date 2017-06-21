using System;
using System.Collections.Generic;
using System.IO;

namespace W09E02_SmartSearch
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            string[] stdin = File.ReadAllLines("input.txt");
            int[] result = SmartSearch(stdin[1], stdin[0]);
            Console.WriteLine(result.Length);
            for (int i = 0; i < result.Length; i++)
                Console.Write(result[i] + 1 + " ");

            sw.Dispose();
        }

        const int p = 7;

        static int GetHashCode(string x)
        {
            int result = 0;
            for (int i = 0; i < x.Length; i++)
            {
                result += (int)Math.Pow(p, x.Length - 1 - i) * (int)(x[i]);
            }
            return result;
        }

        static int[] SmartSearch(string plainText, string subString)
        {
            List<int> result = new List<int>();
            if (subString.Length > plainText.Length)
                return result.ToArray();
            int hashX = GetHashCode(plainText.Substring(0, subString.Length));
            int hashS = GetHashCode(subString);

            for (int i = 0; i < plainText.Length - subString.Length + 1; i++)
            {
                if (hashS == hashX)
                {
                    bool correct = true;
                    for (int j = 0; j < subString.Length && correct; j++)
                        if (plainText[i + j] != subString[j])
                            correct = false;
                    if (correct)
                        result.Add(i);
                }
                if (i + subString.Length != plainText.Length)
                    hashX = (hashX - (int)Math.Pow(p, subString.Length - 1) * (int)(plainText[i])) * p + (int)(plainText[i + subString.Length]);
            }

            return result.ToArray();
        }
    }
}