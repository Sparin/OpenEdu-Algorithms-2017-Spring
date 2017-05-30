using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace W07E02_Rotation
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
            List<AvlTree<long>> leafs = new List<AvlTree<long>>();

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

                //Calc Height
                if (args[1] == 0 & args[2] == 0)
                {
                    AvlTree<long> leaf = nodes[i - 1];
                    Stack<AvlTree<long>> temp = new Stack<AvlTree<long>>();
                    while (leaf != null)
                    {
                        temp.Push(leaf);
                        leaf = leaf.Parent;
                    }
                    while (temp.Count != 0)
                    {
                        leaf = temp.Pop();
                        if (leaf.Height < temp.Count)
                            leaf.Height = temp.Count;
                    }
                    leafs.Add(nodes[i - 1]);
                }
            }

            AvlTree<long> root = nodes[0];
            while (root.Parent != null)
                root = root.Parent;

            if (AvlTree<long>.GetBalance(root.Right) == -1)
                root = AvlTree<long>.BigLeftTurn(root);
            else
                root = AvlTree<long>.SmallLeftTurn(root);

            leafs.Clear();
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i].Height = 0;
                if (nodes[i].Right == null && nodes[i].Left == null)
                    leafs.Add(nodes[i]);
            }

            for (int i = 0; i < leafs.Count; i++)
            {
                AvlTree<long> leaf = leafs[i];
                Stack<AvlTree<long>> temp = new Stack<AvlTree<long>>();
                while (leaf != null)
                {
                    temp.Push(leaf);
                    leaf = leaf.Parent;
                }
                int depth = 0;
                while (temp.Count != 0)
                {
                    leaf = temp.Pop();
                    leaf.Depth = depth++;
                    if (leaf.Height < temp.Count)
                        leaf.Height = temp.Count;
                }
            }


            Console.WriteLine(N);
            AvlTree<long>.PrintTree(root, nodes, 1);

            sw.Dispose();
        }
    }

    class AvlTree<T> where T : IComparable<T>
    {
        public T Key { get; set; }
        public AvlTree<T> Parent { get; set; }
        public AvlTree<T> Left { get; set; }
        public AvlTree<T> Right { get; set; }

        public long Height { get; set; }
        public long Depth { get; set; }

        public static void PrintTree(AvlTree<T> tree, AvlTree<T>[] nodes, long index)
        {
            if (tree == null)
                return;
            int heads = 2;
            for (long i = tree.Height; i >= 0; i--)
            {
                AvlTree<T>[] array = nodes.Where(x => x.Depth == tree.Height - i).OrderBy(x => x.Key).Select(x => x).ToArray();
                for (int j = 0; j < array.Length; j++)
                {
                        long leftIndex = array[j].Left != null ? heads++ : 0;
                        long rightIndex = array[j].Right != null ? heads++ : 0;
                        Console.WriteLine(array[j].Key + " " + leftIndex + " " + rightIndex);
                }
            }
        }

        public static long GetBalance(AvlTree<T> tree)
        {
            if (tree == null)
                throw new ArgumentNullException("tree");

            if (tree.Left != null && tree.Right != null)
                return tree.Right.Height - tree.Left.Height;
            if (tree.Left == null && tree.Right != null)
                return tree.Right.Height + 1;
            if (tree.Left != null && tree.Right == null)
                return -1 - tree.Left.Height;
            else
                return 0;
        }

        /// <returns>Root of tree after turn</returns>
        public static AvlTree<T> SmallLeftTurn(AvlTree<T> root)
        {
            AvlTree<T> child = root.Right;
            AvlTree<T> parent = root.Parent;
            AvlTree<T> x = root.Left;
            AvlTree<T> y = root.Right.Left;
            AvlTree<T> z = root.Right.Right;

            //Parents
            child.Parent = parent;
            root.Parent = child;
            if (x != null)
                x.Parent = root;
            if (y != null)
                y.Parent = root;
            if (z != null)
                z.Parent = child;

            //Childs
            root.Left = x;
            root.Right = y;
            child.Left = root;
            child.Right = z;
            if (parent != null)
                if (parent.Right == root)
                    parent.Right = child;
                else
                    parent.Left = child;

            return child;
        }

        public static AvlTree<T> BigLeftTurn(AvlTree<T> root)
        {
            AvlTree<T> w = root.Left;
            AvlTree<T> parent = root.Parent;
            AvlTree<T> b = root.Right;
            AvlTree<T> c = root.Right.Left;
            AvlTree<T> z = b.Right;
            AvlTree<T> x = c.Left;
            AvlTree<T> y = c.Right;

            //Parents
            c.Parent = parent;
            b.Parent = c;
            root.Parent = c;
            if (w != null)
                w.Parent = root;
            if (z != null)
                z.Parent = b;
            if (y != null)
                y.Parent = b;
            if (x != null)
                x.Parent = root;

            //Childs
            if (parent != null)
                if (parent.Right == root)
                    parent.Right = c;
                else
                    parent.Left = c;
            c.Left = root;
            c.Right = b;
            b.Left = y;
            b.Right = z;
            root.Left = w;
            root.Right = x;

            return c;
        }
    }
}