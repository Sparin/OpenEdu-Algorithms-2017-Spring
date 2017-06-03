using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace W07E03_Addition
{
    class Program
    {
        static void Main()
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            string[] stdin = File.ReadAllLines("input.txt");
            long N = long.Parse(stdin[0]);
            AvlTree<long>[] nodes = new AvlTree<long>[N];

            //Parsing
            for (int i = 1; i <= N; i++)
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

            AvlTree<long> root=null;
            if (nodes.Length != 0)
            {
                root = nodes[0];
                while (root.Parent != null)
                    root = root.Parent;
            }

            for (long i = N + 1; i < stdin.Length; i++)
                root = AvlTree<long>.Insert(root, new AvlTree<long> { Key = long.Parse(stdin[i]) });

            Console.WriteLine(stdin.Length - 1);
            AvlTree<long>.PrintTree(root);

            sw.Dispose();
        }
    }

    class AvlTree<T> where T : IComparable<T>
    {
        public T Key { get; set; }
        public AvlTree<T> Parent { get; set; }
        public AvlTree<T> Left { get; set; }
        public AvlTree<T> Right { get; set; }

        private long Depth { get; set; }

        /// <returns>Root of tree after insert</returns>
        public static AvlTree<T> Insert(AvlTree<T> root, AvlTree<T> node)
        {
            if (root == null)
                return node;
            AvlTree<T> current = root;
            while (true)
            {
                if (current.Key.CompareTo(node.Key) == 0)
                    throw new ArgumentException("Not unique key");
                if (current.Key.CompareTo(node.Key) < 0)
                {
                    if (current.Right != null)
                        current = current.Right;
                    else
                    {
                        current.Right = node;
                        node.Parent = current;
                        return Balance(node);
                    }
                }
                else
                {
                    if (current.Left != null)
                        current = current.Left;
                    else
                    {
                        current.Left = node;
                        node.Parent = current;
                        return Balance(node);
                    }
                }
            }
        }

        /// <returns>Root of tree after balanc</returns>
        public static AvlTree<T> Balance(AvlTree<T> leaf)
        {
            AvlTree<T> current = leaf;
            while (current != null)
            {
                long balance = GetBalance(current);
                if (balance > 1)
                {
                    if (GetBalance(current.Right) == -1)
                        current = BigLeftTurn(current);
                    else
                        current = SmallLeftTurn(current);
                }
                if (balance < -1)
                {
                    if (GetBalance(current.Left) == 1)
                        current = BigRightTurn(current);
                    else
                        current = SmallRightTurn(current);
                }
                if (current.Parent == null)
                    return current;
                else
                    current = current.Parent;
            }
            return current;
        }

        public static long Height(AvlTree<T> root)
        {
            if (root == null)
                return -1;
            Queue<AvlTree<T>> bfsQueue = new Queue<AvlTree<T>>();
            long height = 0;
            root.Depth = 0;
            bfsQueue.Enqueue(root);
            while (bfsQueue.Count != 0)
            {
                AvlTree<T> current = bfsQueue.Dequeue();
                if (current != root)
                    current.Depth = current.Parent.Depth + 1;
                if (current.Depth > height)
                    height = current.Depth;
                if (current.Right != null)
                    bfsQueue.Enqueue(current.Right);
                if (current.Left != null)
                    bfsQueue.Enqueue(current.Left);
            }
            return height;
        }

        public static void PrintTree(AvlTree<T> root)
        {
            if (root == null)
                return;
            Queue<AvlTree<T>> bfsQueue = new Queue<AvlTree<T>>();
            long counter = 1;
            bfsQueue.Enqueue(root);
            while (bfsQueue.Count != 0)
            {
                AvlTree<T> current = bfsQueue.Dequeue();
                Console.Write(current.Key);

                if (current.Left != null)
                {
                    bfsQueue.Enqueue(current.Left);
                    Console.Write(" " + ++counter);
                }
                else
                    Console.Write(" " + 0);

                if (current.Right != null)
                {
                    bfsQueue.Enqueue(current.Right);
                    Console.WriteLine(" " + ++counter);
                }
                else
                    Console.WriteLine(" " + 0);
            }
        }

        public static long GetBalance(AvlTree<T> tree)
        {
            if (tree == null)
                throw new ArgumentNullException("tree");

            return Height(tree.Right) - Height(tree.Left);
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

        /// <returns>Root of tree after turn</returns>
        public static AvlTree<T> SmallRightTurn(AvlTree<T> root)
        {
            AvlTree<T> child = root.Left;
            AvlTree<T> parent = root.Parent;
            AvlTree<T> x = root.Right;
            AvlTree<T> y = root.Left.Left;
            AvlTree<T> z = root.Left.Right;

            //Parents
            child.Parent = parent;
            root.Parent = child;
            if (x != null)
                x.Parent = root;
            if (y != null)
                y.Parent = child;
            if (z != null)
                z.Parent = root;

            //Childs
            root.Left = z;
            root.Right = x;
            child.Left = y;
            child.Right = root;
            if (parent != null)
                if (parent.Right == root)
                    parent.Right = child;
                else
                    parent.Left = child;

            return child;
        }

        /// <returns>Root of tree after turn</returns>
        public static AvlTree<T> BigRightTurn(AvlTree<T> root)
        {
            AvlTree<T> w = root.Right;
            AvlTree<T> parent = root.Parent;
            AvlTree<T> b = root.Left;
            AvlTree<T> c = root.Left.Right;
            AvlTree<T> z = b.Left;
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
                y.Parent = root;
            if (x != null)
                x.Parent = b;

            //Childs
            if (parent != null)
                if (parent.Right == root)
                    parent.Right = c;
                else
                    parent.Left = c;
            c.Left = b;
            c.Right = root;
            b.Left = z;
            b.Right = x;
            root.Left = y;
            root.Right = w;

            return c;
        }

        /// <returns>Root of tree after turn</returns>
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