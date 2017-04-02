using System;
using System.Collections.Generic;
using System.IO;

namespace W05E02_QueueWithPriorities
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            string[] stdin = File.ReadAllLines("input.txt");

            QueueWithPriorities queue = new QueueWithPriorities();
            for (int i = 1; i < stdin.Length; i++)
                switch (stdin[i][0])
                {
                    case 'A':
                        queue.Insert(i,int.Parse(stdin[i].Split(' ')[1]));
                        break;
                    case 'X':
                        queue.Extract();
                        break;
                    case 'D':
                        queue.Decrease(int.Parse(stdin[i].Split(' ')[1]), int.Parse(stdin[i].Split(' ')[2]));
                        break;
                }

            sw.Dispose();
        }
    }

    class Element
    {
        public int CurrentIndex { get; set; }
        public long Value { get; set; }
    }

    class QueueWithPriorities
    {
        public Dictionary<int, Element> References = new Dictionary<int, Element>();
        public Element[] array = new Element[6000000];
        public int HeapSize { get; private set; }
        public Element Top { get { return array[0]; } }

        public void Extract()
        {
            if (HeapSize == 0)
                Console.WriteLine("*");
            else
            {
                Console.WriteLine(Top.Value);
                HeapSize--;
                Swap(0, HeapSize);
                Heapify(0);
            }
        }

        public void Decrease(int lineIndex, int value)
        {
            int index = References[lineIndex].CurrentIndex;
            array[index].Value = value;
            while (index > 0 && array[Parent(index)].Value > array[index].Value)
            {
                Swap(index, Parent(index));
                index = Parent(index);
            }
        }

        public void Insert(int lineIndex, int value)
        {
            array[HeapSize] = new Element { Value = int.MaxValue, CurrentIndex = HeapSize };
            References.Add(lineIndex, array[HeapSize]);
            HeapSize++;
            Decrease(lineIndex, value);
        }

        private int Parent(int index)
        {
            return (index + 1) / 2 - 1;
        }

        private void Heapify(int index)
        {
            int rightChildIndex = (index + 1) * 2;
            int leftChildIndex = rightChildIndex - 1;
            int lowestIndex = int.MinValue;

            if (leftChildIndex < HeapSize && array[leftChildIndex].Value < array[index].Value)
                lowestIndex = leftChildIndex;
            else
                lowestIndex = index;
            if (rightChildIndex < HeapSize && array[rightChildIndex].Value < array[lowestIndex].Value)
                lowestIndex = rightChildIndex;

            if (lowestIndex != index)
            {
                Swap(lowestIndex, index);
                Heapify(lowestIndex);
            }
        }

        private void Swap(int a, int b)
        {
            array[a].CurrentIndex = b;
            array[b].CurrentIndex = a;
            Element temp = array[a];
            array[a] = array[b];
            array[b] = temp;
        }
    }
}