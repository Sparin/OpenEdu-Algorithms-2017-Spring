using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace W06E03_HeightOfTree
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            string[] stdin = File.ReadAllLines("input.txt");

            List<int> lists = new List<int>();
            Node[] array = new Node[stdin.Length];
            if (int.Parse(stdin[0]) == 0)
                Console.Write(0);
            else
            {
                for (int i = 1; i < stdin.Length; i++)
                {
                    int[] temp = stdin[i].Split(' ').Select(x => int.Parse(x)).ToArray();
                    if (array[i] == null)
                        array[i] = new Node();
                    array[i].Key = temp[0];
                    array[i].Left = temp[1];
                    array[i].Right = temp[2];
                    if (temp[1] != 0)
                    {
                        array[temp[1]] = new Node();
                        array[temp[1]].Parent = i;
                    }
                    if (temp[2] != 0)
                    {
                        array[temp[2]] = new Node();
                        array[temp[2]].Parent = i;
                    }
                    if (temp[1] == 0 && temp[2] == 0)
                        lists.Add(i);
                }

                int heigth = 0;
                for (int i = 0; i < lists.Count; i++)
                {
                    int tempHeigth = 1;
                    int currentNode = lists[i];
                    while (array[currentNode].Parent != 0)
                    {
                        tempHeigth++;
                        currentNode = array[currentNode].Parent;
                    }
                    if (heigth < tempHeigth)
                        heigth = tempHeigth;
                }
                Console.WriteLine(heigth);
            }
            sw.Dispose();
        }
    }

    class Node
    {
        public int Key { get; set; }
        public int Parent { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
    }

    //class Node<T>
    //{
    //    public T Key { get; set; }
    //    public Node<T> Parent { get; set; }
    //    public Node<T> Left { get; set; }
    //    public Node<T> Right { get; set; }

    //    public static Node<T> Minimum(Node<T> node)
    //    {
    //        while (node.Left != null)
    //            node = node.Left;
    //        return node;
    //    }

    //    public static Node<T> Next(Node<T> node)
    //    {
    //        if (node.Right != null)
    //            return Minimum(node.Right);
    //        Node<T> y = node.Parent;
    //        while (y != null && node == y.Right)
    //        {
    //            node = y;
    //            y = y.Parent;
    //        }
    //        return y;
    //    }

    //    public static int Height(Node<T> node)
    //    {
    //        int right = 0, left = 0;
    //        if (node.Right != null)
    //            right = Height(node.Right);
    //        if (node.Left != null)
    //            left = Height(node.Left);
    //        if (right > left)
    //            return right + 1;
    //        else
    //            return left + 1;
    //    }
    //}
}