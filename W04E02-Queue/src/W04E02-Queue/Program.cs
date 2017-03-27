using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace W04E02_Queue
{
    public class Program
    {
        public static void Main(string[] args)
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);

            string[] stdin = File.ReadAllLines("input.txt");
            //Cheating
            //Also you can check special stack without checks for this task from this site
            //https://github.com/Sparin/OpenEdu-Algorithms/blob/W04E02-Queue/Queue/Queue/Program.cs
            //It's works too
            Queue<long> queue = new Queue<long>();
            for (int i = 1; i < stdin.Length; i++)
            {
                string[] temp = stdin[i].Split(' ');
                if (temp[0] == "-")
                    Console.WriteLine(queue.Dequeue());
                else
                    queue.Enqueue(long.Parse(temp[1]));
            }

            sw.Dispose();
        }
    }
}
