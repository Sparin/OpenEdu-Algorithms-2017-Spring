using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace W04E04_QueueMin
{
    public class Program
    {
        static Stack<Pair<long, long>> s1 = new Stack<Pair<long, long>>();
        static Stack<Pair<long, long>> s2 = new Stack<Pair<long, long>>();

        public static void Main(string[] args)
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            string[] stdin = File.ReadAllLines("input.txt");
            for (int i = 1; i < stdin.Length; i++)
            {
                string[] temp = stdin[i].Split(' ');
                switch (temp[0])
                {
                    case "+":
                        long value = long.Parse(temp[1]);
                        Pair<long, long> item = new Pair<long, long>() { First = value, Second = value };
                        if (s1.Count != 0)
                            if (s1.Peek().Second < item.First)
                                item.Second = s1.Peek().Second;
                        s1.Push(item);
                        break;
                    case "-":
                        if (s2.Count == 0)
                            while (s1.Count != 0)
                            {
                                long element = s1.Pop().First;
                                long min = element;
                                if (s2.Count != 0)
                                    min = Math.Min(s2.Peek().Second, element);
                                s2.Push(new Pair<long, long> { First = element, Second = min });
                            }
                        s2.Pop();
                        break;
                    case "?":
                        long firstMinStack = long.MaxValue, secondMinStack = long.MaxValue;
                        if (s1.Count != 0)
                            firstMinStack = s1.Peek().Second;
                        if (s2.Count != 0)
                            secondMinStack = s2.Peek().Second;
                        Console.WriteLine(Math.Min(firstMinStack, secondMinStack));
                        break;
                }
            }

            sw.Dispose();
        }
    }

    public struct Pair<T, K> where T : IComparable<T> where K : IComparable<K>
    {
        public T First { get; set; }
        public K Second { get; set; }
    }

    public class Stack<T>
    {
        int head;
        public T[] array = new T[1000000];
        public int Count { get { return head; } }

        public void Push(T item)
        {
            array[head++] = item;
        }

        public T Pop()
        {
            return array[--head];
        }

        public T Peek()
        {
            return array[head - 1];
        }

    }
}
