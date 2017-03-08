using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace W01E04_Sortland
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            string[] stdin = File.ReadAllLines("input.txt");
            int i = 1;
            Person[] array = stdin[1].Split(' ').Select(x => new Person { Balance = double.Parse(x), Index = i++ }).ToArray();
            array = InsertionSort(array);

            Console.Write("{0} {1} {2}", array[0], array[array.Length / 2], array[array.Length - 1]);

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
            }
            return array;
        }
    }

    struct Person : IComparable<Person>
    {
        public double Balance { get; set; }
        public int Index { get; set; }

        public int CompareTo(Person person)
        {
            return Balance.CompareTo(person.Balance);
        }

        public override string ToString()
        {
            return Index.ToString();
        }
    }
}
