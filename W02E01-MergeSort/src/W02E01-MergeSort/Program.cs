using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace W02E01_MergeSort
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
            MergeSort(ref inputArray,0,inputArray.Length-1);

            foreach (long item in inputArray)
                Console.Write("{0} ", item);
            
            sw.Dispose();
        }

        public static void MergeSort<T>(ref T[] array, long startIndex, long endIndex) where T : IComparable<T>
        {
            if (endIndex - startIndex == 0)
            {
                Console.WriteLine("{0} {1} {2} {3}", startIndex + 1, endIndex + 1, array[startIndex], array[endIndex]);
                return;
            }

            long middleIndex = (endIndex + startIndex) / 2;
            MergeSort(ref array, startIndex, middleIndex);
            MergeSort(ref array, middleIndex + 1, endIndex);
            Merge(ref array, startIndex, middleIndex, endIndex);
        }

        public static void Merge<T>(ref T[] array, long startIndex, long middleIndex, long endIndex) where T : IComparable<T>
        {
            T[] result = new T[endIndex - startIndex + 1];
            long li = 0, ri = 0;

            while (li < middleIndex - startIndex + 1 && ri < endIndex - middleIndex)
                if (array[startIndex + li].CompareTo(array[middleIndex + ri + 1])<=0)
                    result[ri + li] = array[startIndex + li++];
                else
                    result[ri + li] = array[middleIndex + ++ri];

            while (li < middleIndex - startIndex + 1)
                result[ri + li] = array[startIndex + li++];

            while (ri < endIndex - middleIndex)
                result[ri + li] = array[middleIndex + ++ri];

            for (int i = 0; i < result.Length; i++)
                array[startIndex + i] = result[i];

            Console.WriteLine("{0} {1} {2} {3}", startIndex + 1, endIndex + 1, array[startIndex], array[endIndex]);
        }

        #region Service example
        public static T[] MergeSort<T>(T[] array) where T : IComparable<T>
        {
            int length = array.Length;
            if (length == 1)
                return array;
            T[] l = array.Take(length / 2 - 1).ToArray();
            T[] r = array.Skip(length / 2 - 1).ToArray();
            l = MergeSort(l);
            r = MergeSort(r);
            return array;
        }

        public static T[] Merge<T>(T[] leftPart, T[] rightPart) where T : IComparable<T>
        {
            int i = 0, j = 0;
            T[] result = new T[leftPart.Length + rightPart.Length];
            while (i < leftPart.Length || j < rightPart.Length)
                if (j == rightPart.Length || (i < leftPart.Length && leftPart[i].CompareTo(rightPart[j]) <= 0))
                    result[i + j] = leftPart[i++];
                else
                    result[i + j] = rightPart[j++];
            return result;
        }
        #endregion
    }
}
