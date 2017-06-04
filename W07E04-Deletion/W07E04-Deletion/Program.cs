using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace W07E04_Deletion
{
    class Program
    {
        static void Main()
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            string[] stdin = File.ReadAllLines("input.txt");
            int N = int.Parse(stdin[0]);

            AvlTree<long> root = null;
            for (int i = 1; i <=N; i++)
                root = AvlTree<long>.Insert(root, new AvlTree<long> { Key = long.Parse(stdin[i].Split(' ')[0]) });

            for(int i = N+1;i<stdin.Length;i++)
            {
                AvlTree<long> node = AvlTree<long>.Search(root, long.Parse(stdin[i]));
                if (node != null)
                    root = AvlTree<long>.Remove(node);
            }

            Console.WriteLine(N - (stdin.Length - 1 - N));
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
        private long height = 0;
        public long Height { get { return height; } set { height = value; } }

        public static AvlTree<T> Next(AvlTree<T> node)
        {
            if (node.Right == null)
                return node;
            return Minimum(node.Right);
        }

        public static AvlTree<T> Previous(AvlTree<T> node)
        {
            if (node.Left == null)
                return node;
            return Maximum(node.Left);
        }

        public static AvlTree<T> Maximum(AvlTree<T> node)
        {
            while (node.Right != null)
                node = node.Right;
            return node;
        }

        public static AvlTree<T> Minimum(AvlTree<T> node)
        {
            while (node.Left != null)
                node = node.Left;
            return node;
        }

        /// <returns>Root of tree after remove</returns>
        public static AvlTree<T> Remove(AvlTree<T> item)
        {
            AvlTree<T> parent = item.Parent;

            //Leaf
            if (item.Left == null && item.Right == null)
            {
                if (parent == null)
                    return null;
                if (parent.Left == item)
                    parent.Left = null;
                else
                    parent.Right = null;

                UpdateHeight(parent);
                return Balance(parent);
            }

            //One child
            if ((item.Left == null) ^ (item.Right == null))
                if (item.Left != null)
                {
                    if (parent != null)
                    {
                        if (parent.Left == item)
                            parent.Left = item.Left;
                        else
                            parent.Right = item.Left;

                        UpdateHeight(parent);
                    }

                    item.Left.Parent = parent;
                    return Balance(item.Left);
                }
                else
                {
                    if (parent != null)
                    {
                        if (parent.Left == item)
                            parent.Left = item.Right;
                        else
                            parent.Right = item.Right;

                        UpdateHeight(parent);
                    }

                    item.Right.Parent = parent;
                    return Balance(item.Right);
                }


            //Two child
            if ((item.Left != null) && (item.Right != null))
            {
                AvlTree<T> prev = Previous(item);
                Remove(prev);
                item.Key = prev.Key;
            }

            return Balance(item);
        }

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
                        UpdateHeight(node);
                        return root;
                        //return Balance(node);
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
                        UpdateHeight(node);
                        return root;
                        //return Balance(node);
                    }
                }
            }
        }

        private static void UpdateHeight(AvlTree<T> node)
        {
            while (node != null)
            {
                long rH = node.Right != null ? node.Right.Height : -1;
                long lH = node.Left != null ? node.Left.Height : -1;
                
                if (rH > lH)
                    node.Height = rH + 1;
                else
                    node.Height = lH + 1;

                node = node.Parent;
            }
        }

        public static AvlTree<T> Search(AvlTree<T> root, T key)
        {
            while (root != null && root.Key.CompareTo(key) != 0)
                if (root.Key.CompareTo(key) > 0)
                    root = root.Left;
                else
                    root = root.Right;

            return root;
        }

        /// <returns>Root of tree after balance</returns>
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
                return 0;

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

            //Heights
            long xH = x != null ? x.Height : -1;
            long yH = y != null ? y.Height : -1;
            long zH = z != null ? z.Height : -1;

            if (xH > yH)
                root.Height = xH + 1;
            else
                root.Height = yH + 1;
            if (root.Height > zH)
                child.Height = root.Height + 1;
            else
                child.Height = zH + 1;

            UpdateHeight(child);
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

            //Heights
            long xH = x != null ? x.Height : -1;
            long yH = y != null ? y.Height : -1;
            long zH = z != null ? z.Height : -1;

            if (zH > xH)
                root.Height = zH + 1;
            else
                root.Height = xH + 1;

            if (y.Height > root.Height)
                child.Height = yH + 1;
            else
                child.Height = root.Height + 1;

            UpdateHeight(child);
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

            //Heights
            long xH = x != null ? x.Height : -1;
            long yH = y != null ? y.Height : -1;
            long zH = z != null ? z.Height : -1;
            long wH = w != null ? w.Height : -1;

            if (zH > xH)
                b.Height = zH + 1;
            else
                b.Height = xH + 1;

            if (yH > wH)
                root.Height = yH + 1;
            else
                root.Height = wH + 1;

            if (b.Height > root.Height)
                c.Height = b.Height + 1;
            else
                c.Height = root.Height + 1;

            UpdateHeight(c);
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

            //Heights
            long xH = x != null ? x.Height : -1;
            long yH = y != null ? y.Height : -1;
            long zH = z != null ? z.Height : -1;
            long wH = w != null ? w.Height : -1;

            if (wH > xH)
                root.Height = wH + 1;
            else
                root.Height = xH + 1;

            if (yH > zH)
                b.Height = yH + 1;
            else
                b.Height = zH + 1;

            if (b.Height > root.Height)
                c.Height = b.Height + 1;
            else
                c.Height = root.Height + 1;

            UpdateHeight(c);
            return c;
        }
    }
}