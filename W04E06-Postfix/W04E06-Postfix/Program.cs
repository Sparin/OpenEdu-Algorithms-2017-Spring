using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace W04E06_Postfix
{
    class Program
    {
        private static Stack<int> stack = new Stack<int>();

        static void Main(string[] args)
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            string[] stdin = File.ReadAllLines("input.txt")[1].Split(' ');

            int a, b;
            for (int i = 0; i < stdin.Length; i++)
            {
                switch (stdin[i])
                {
                    case "+":
                        a = stack.Pop();
                        b = stack.Pop();
                        stack.Push(a + b);
                        break;
                    case "-":
                        a = stack.Pop();
                        b = stack.Pop();
                        stack.Push(b - a);
                        break;
                    case "*":
                        a = stack.Pop();
                        b = stack.Pop();
                        stack.Push(b * a);
                        break;
                    default:
                        stack.Push(int.Parse(stdin[i]));
                        break;
                }
            }

            Console.WriteLine(stack.Pop());

            sw.Dispose();
        }
    }
}