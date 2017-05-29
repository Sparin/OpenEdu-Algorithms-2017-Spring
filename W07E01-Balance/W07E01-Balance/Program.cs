using System;
using System.IO;
using System.Linq;

namespace W07E01_Balance
{
    class Program
    {
        static void Main()
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            string[] stdin = File.ReadAllLines("input.txt");
            bool isCorrect = true;
            long N = long.Parse(stdin[0]);
            AvlTree<long>[] nodes = new AvlTree<long>[N];

            //Parsing
            for (int i = 1; i <= N && isCorrect; i++)
            {
                long[] args = stdin[i].Split(' ').Select(x => long.Parse(x)).ToArray();


                if (nodes[i - 1] == null)
                    nodes[i - 1] = new AvlTree<long>();
                nodes[i - 1].Key = args[0];
                //Left child
                if (args[1] != 0)
                {
                    if (nodes[args[1] - 1] == null)
                        nodes[args[1] - 1] = new AvlTree<long>() { Parent = nodes[i - 1] };
                    nodes[i - 1].Left = nodes[args[1] - 1];
                }
                //Right child
                if (args[2] != 0)
                {
                    if (args[2] != 0 && nodes[args[2] - 1] == null)
                        nodes[args[2] - 1] = new AvlTree<long>() { Parent = nodes[i - 1] };
                    nodes[i - 1].Right = nodes[args[2] - 1];
                }
            }

            for (int i = 0; i < N; i++)
                Console.WriteLine(AvlTree<long>.GetBalance(nodes[i]));

            sw.Dispose();
        }
    }

    class AvlTree<T> where T : IComparable<T>
    {
        public T Key { get; set; }
        public AvlTree<T> Parent { get; set; }
        public AvlTree<T> Left { get; set; }
        public AvlTree<T> Right { get; set; }

        public static long Height(AvlTree<T> tree)
        {
            if (tree == null)
                return -1;
            long leftHeight = 0, rightHeight = 0;

            if (tree.Left != null)
                leftHeight = 1 + Height(tree.Left);
            if (tree.Right != null)
                rightHeight = 1 + Height(tree.Right);

            if (leftHeight > rightHeight)
                return leftHeight;
            else
                return rightHeight;
        }

        public static long GetBalance(AvlTree<T> tree)
        {
            if (tree == null)
                throw new ArgumentNullException("tree");
            return Height(tree.Right) - Height(tree.Left);
        }
    }
}