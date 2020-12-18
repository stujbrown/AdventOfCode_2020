using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Day18
{
    class Program
    {
        static Int64 DoMaths(string[] lines)
        {
            Int64 sum = 0;
            foreach (var line in lines)
            {
                string expandedLine = line;
                line.Select((c, index) => Tuple.Create(c, index)).Where(pair => pair.Item1 == '(' || pair.Item1 == ')').Reverse().ToList().ForEach(bracket => expandedLine = expandedLine.Insert(bracket.Item2 + 1, " ").Insert(bracket.Item2, " "));
                var tokens = expandedLine.Split(' ').Where(token => String.IsNullOrWhiteSpace(token) == false).ToList();

                var values = new Stack<Int64>();
                var operations = new Stack<char>();
                for (int i = 0; i < tokens.Count; ++i)
                {
                    if (tokens[i] == "(") operations.Push('(');
                    else if (tokens[i] == ")")
                    {
                        operations.Pop();
                        if (operations.Count > 0 && operations.Peek() != '(')
                        {
                            var top = values.Pop();
                            if (operations.Peek() == '+') values.Push(values.Pop() + top);
                            else if (operations.Peek() == '-') values.Push(values.Pop() - top);
                            else if (operations.Peek() == '/') values.Push(values.Pop() / top);
                            else if (operations.Peek() == '*') values.Push(values.Pop() * top);
                            operations.Pop();
                        }
                    }

                    else if (tokens[i] == "+" || tokens[i] == "-" || tokens[i] == "/" || tokens[i] == "*")
                    {
                        operations.Push(tokens[i][0]);
                    }
                    else
                    {
                        if (operations.Count > 0 && operations.Peek() != '(')
                        {
                            if (operations.Peek() == '+') values.Push(values.Pop() + Int64.Parse(tokens[i]));
                            else if (operations.Peek() == '-') values.Push(values.Pop() - Int64.Parse(tokens[i]));
                            else if (operations.Peek() == '/') values.Push(values.Pop() / Int64.Parse(tokens[i]));
                            else if (operations.Peek() == '*') values.Push(values.Pop() * Int64.Parse(tokens[i]));
                            operations.Pop();
                        }
                        else
                            values.Push(Int64.Parse(tokens[i]));
                    }
                }

                sum += values.Pop();
            }

            return sum;
        }
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            Console.WriteLine("PArt 1: {0}", DoMaths(lines));
        }

    }

}