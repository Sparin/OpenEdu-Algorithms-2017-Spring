using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace W06E01_BinarySearch
{
    class Program
    {
        static Dictionary<int, string> results = new Dictionary<int, string>();

        static void Main(string[] args)
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            string[] stdin = File.ReadAllLines("input.txt");

            int[] array = stdin[1].Split(' ').Select(x => int.Parse(x)).ToArray();
            int[] requests = stdin[3].Split(' ').Select(x => int.Parse(x)).ToArray();

            for (int i = 0; i < requests.Length; i++)
                if (results.ContainsKey(requests[i]))
                    Console.WriteLine(results[requests[i]]);
                else
                    BinarySearch(ref array, requests[i]);

            sw.Dispose();
        }

        static void BinarySearch(ref int[] array, int key)
        {
            int l = -1, r = array.Length;
            while (r > l + 1)
            {
                if (r < array.Length && l >= 0 && array[l] == key && array[r] == key)
                    break;
                int m = (l + r) / 2;
                if (array[m] < key)
                    l = m;
                else
                    r = m;
            }

            if (r < array.Length && array[r] == key)
            {
                while (r < array.Length && array[r] == key)
                    r++;
                while (l >= 0 && array[l] == key)
                    l--;
                l += 2;
                Console.WriteLine("{0} {1}", l, r);
                results.Add(key, string.Format("{0} {1}", l, r));
            }
            else
            {
                Console.WriteLine("-1 -1");
                results.Add(key, string.Format("-1 -1"));
            }
        }
    }
}