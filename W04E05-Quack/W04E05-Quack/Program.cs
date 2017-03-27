using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace W04E05_Quack
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            Console.OutputEncoding = Encoding.ASCII;
            Console.SetOut(sw);

            string[] stdin = File.ReadAllLines("input.txt");
            new QuackMachine(stdin).Run();

            sw.Dispose();
        }
    }

    public class QuackMachine
    {
        private ushort[] registers = new ushort[26];
        private int currentInstructionIndex = 0;
        private Dictionary<string, int> labels = new Dictionary<string, int>();
        private string[] instructions;
        private Queue<ushort> queue = new Queue<ushort>();

        public QuackMachine(string[] instructions)
        {
            this.instructions = instructions;
            RegisterLabels();
        }

        public void Run()
        {
            for (currentInstructionIndex = 0; currentInstructionIndex < instructions.Length; currentInstructionIndex++)
            {
                switch (instructions[currentInstructionIndex][0])
                {
                    case '+':
                        Sum();
                        break;
                    case '-':
                        Substract();
                        break;
                    case '*':
                        Multiply();
                        break;
                    case '/':
                        Divide();
                        break;
                    case '%':
                        Mod();
                        break;
                    case '>':
                        GetRegister(instructions[currentInstructionIndex][1]);
                        break;
                    case '<':
                        SetRegister(instructions[currentInstructionIndex][1]);
                        break;
                    case 'P':
                        if (instructions[currentInstructionIndex].Length == 1)
                            PrintValue();
                        else
                            PrintValue(instructions[currentInstructionIndex][1]);
                        break;
                    case 'C':
                        if (instructions[currentInstructionIndex].Length == 1)
                            PrintChar();
                        else
                            PrintChar(instructions[currentInstructionIndex][1]);
                        break;
                    case ':':
                        break;
                    case 'J':
                        GoTo(currentInstructionIndex);
                        break;
                    case 'Z':
                        GoToIfZeroEqual(currentInstructionIndex);
                        break;
                    case 'E':
                        GoToIfEquals(currentInstructionIndex);
                        break;
                    case 'G':
                        GoToIfMoreThan(currentInstructionIndex);
                        break;
                    case 'Q':
                        Exit();
                        break;
                    default:
                        queue.Enqueue(ushort.Parse(instructions[currentInstructionIndex]));
                        break;
                }
            }
        }

        private void RegisterLabels()
        {
            for (int i = 0; i < instructions.Length; i++)
                if (instructions[i][0] == ':')
                    labels.Add(instructions[i].Remove(0, 1), i);
        }

        private void Sum()
        {
            ushort alpha = queue.Dequeue();
            ushort beta = queue.Dequeue();
            queue.Enqueue((ushort)((alpha + beta) % 65536));
        }

        private void Substract()
        {
            ushort alpha = queue.Dequeue();
            ushort beta = queue.Dequeue();
            queue.Enqueue((ushort)((alpha - beta) % 65536));
        }

        private void Multiply()
        {
            ushort alpha = queue.Dequeue();
            ushort beta = queue.Dequeue();
            queue.Enqueue((ushort)((alpha * beta) % 65536));
        }

        private void Divide()
        {
            ushort alpha = queue.Dequeue();
            ushort beta = queue.Dequeue();
            if (beta != 0)
                queue.Enqueue((ushort)(alpha / beta));
            else
                queue.Enqueue(0);
        }

        private void Mod()
        {
            ushort alpha = queue.Dequeue();
            ushort beta = queue.Dequeue();
            if (beta != 0)
                queue.Enqueue((ushort)(alpha % beta));
            else
                queue.Enqueue(0);
        }

        private void GetRegister(char register)
        {
            ushort alpha = queue.Dequeue();
            registers[register - 'a'] = alpha;
        }

        private void SetRegister(char register)
        {
            queue.Enqueue(registers[register - 'a']);
        }

        private void PrintValue()
        {
            ushort alpha = queue.Dequeue();
            Console.WriteLine(alpha);
        }

        private void PrintValue(char register)
        {
            Console.WriteLine(registers[register - 'a']);
        }

        private void PrintChar()
        {
            ushort alpha = queue.Dequeue();
            Console.Write((char)(alpha % 256));
        }

        private void PrintChar(char register)
        {
            Console.Write((char)(registers[register - 'a'] % 256));
        }

        private void GoTo(int index)
        {
            currentInstructionIndex = labels[instructions[index].Remove(0, 1)];
        }

        private void GoToIfZeroEqual(int index)
        {
            if (registers[instructions[index][1] - 'a'] == 0)
                currentInstructionIndex = labels[instructions[index].Remove(0, 2)];
        }

        private void GoToIfEquals(int index)
        {
            if (registers[instructions[index][1] - 'a'] == registers[instructions[index][2] - 'a'])
                currentInstructionIndex = labels[instructions[index].Remove(0, 3)];
        }

        private void GoToIfMoreThan(int index)
        {
            if (registers[instructions[index][1] - 'a'] > registers[instructions[index][2] - 'a'])
                currentInstructionIndex = labels[instructions[index].Remove(0, 3)];
        }

        private void Exit()
        {
            currentInstructionIndex = int.MaxValue;
        }
    }
}