using System;
using System.IO;
using System.Linq;

namespace W06E04_SubTreeRemover
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
            Node<long>[] nodes = new Node<long>[N];

            //Parsing
            for (int i = 1; i <= N; i++)
            {
                long[] args = stdin[i].Split(' ').Select(x => long.Parse(x)).ToArray();
                if (nodes[i - 1] == null)
                    nodes[i - 1] = new Node<long>();
                nodes[i - 1].Key = args[0];
                //Left child
                if (args[1] != 0)
                {
                    if (nodes[args[1] - 1] == null)
                        nodes[args[1] - 1] = new Node<long>() { Parent = nodes[i - 1] };
                    nodes[i - 1].Left = nodes[args[1] - 1];
                }
                //Right child
                if (args[2] != 0)
                {
                    if (args[2] != 0 && nodes[args[2] - 1] == null)
                        nodes[args[2] - 1] = new Node<long>() { Parent = nodes[i - 1] };
                    nodes[i - 1].Right = nodes[args[2] - 1];
                }
            }

            //Remove
            long[] requests = stdin[N + 2].Split(' ').Select(x => long.Parse(x)).ToArray();
            Node<long> root = nodes[0];
            while (root.Parent != null)
                root = root.Parent;
            for (int i = 0; i < requests.Length; i++)
            {
                if (root != null)
                {
                    Node<long> node = Node<long>.Search(root, requests[i]);
                    if (node != null)
                    {
                        if (node == root)
                        {
                            root = null;
                            Console.WriteLine(0);
                        }
                        else
                        {
                            if (node.Parent.Right == node)
                                node.Parent.Right = null;
                            else
                                node.Parent.Left = null;
                        }
                        N -= Node<long>.Count(node);
                    }                   
                    Console.WriteLine(N);
                }
                else
                    Console.WriteLine(0);
            }

            sw.Dispose();
        }
    }

    //TODO: Refactoring needed
    class Node<T> where T : IComparable<T>
    {
        public T Key { get; set; }
        public Node<T> Parent { get; set; }
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }

        public static long Count(Node<T> node)
        {
            long result = 1;
            if (node.Left != null)
                result += Count(node.Left);
            if (node.Right != null)
                result += Count(node.Right);
            return result;
        }

        public static Node<T> Minimum(Node<T> node)
        {
            while (node.Left != null)
                node = node.Left;
            return node;
        }

        public static Node<T> Maximum(Node<T> node)
        {
            while (node.Right != null)
                node = node.Right;
            return node;
        }

        public static Node<T> Next(Node<T> node)
        {
            if (node.Right != null)
                return Minimum(node.Right);
            Node<T> y = node.Parent;
            while (y != null && node == y.Right)
            {
                node = y;
                y = y.Parent;
            }
            return y;
        }

        public static Node<T> Insert(Node<T> node, Node<T> leaf)
        {
            if (node == null)
                return leaf;
            if (leaf.Key.CompareTo(node.Key) < 0)
            {
                node.Left = Insert(node.Left, leaf);
                node.Left.Parent = node;
            }
            else
            {
                node.Right = Insert(node.Right, leaf);
                node.Right.Parent = node;
            }
            return node;
        }

        public static Node<T> Search(Node<T> node, T value)
        {
            while (value.CompareTo(node.Key) != 0 &&
                ((value.CompareTo(node.Key) < 0 && node.Left != null) ||
                (value.CompareTo(node.Key) > 0 && node.Right != null)))
            {
                if (value.CompareTo(node.Key) < 0 && node.Left != null)
                    node = node.Left;
                if (value.CompareTo(node.Key) > 0 && node.Right != null)
                    node = node.Right;
            }

            if (value.CompareTo(node.Key) == 0)
                return node;
            else
                return null;
        }

        public static bool Exists(Node<T> node, T value)
        {
            if (Search(node, value) != null)
                return true;
            else
                return false;
        }

        public static void Remove(Node<T> item)
        {
            Node<T> parent = item.Parent;

            //Leaf
            if (item.Left == null && item.Right == null)
                if (parent.Left == item)
                    parent.Left = null;
                else
                    parent.Right = null;

            //One child
            if ((item.Left == null) ^ (item.Right == null))
            {
                if (item.Left != null)
                {
                    if (parent.Left == item)
                    {
                        parent.Left = item.Left;
                    }
                    else
                    {
                        parent.Right = item.Left;
                    }
                    item.Left.Parent = parent;
                }
                else
                {
                    if (parent.Left == item)
                    {
                        parent.Left = item.Right;

                    }
                    else
                    {
                        parent.Right = item.Right;

                    }
                    item.Right.Parent = parent;
                }
            }

            //Two child
            if ((item.Left != null) && (item.Right != null))
            {
                Node<T> next = Next(item);
                Remove(next);
                item.Key = next.Key;
            }


        }
    }
}