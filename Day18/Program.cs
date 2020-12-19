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
        static Int64 DoMaths(string[] lines, bool additionTakesPriority)
        {
            Int64 sum = 0;
            foreach (var line in lines)
            {
                string expandedLine = line;
                line.Select((c, index) => Tuple.Create(c, index)).Where(pair => pair.Item1 == '(' || pair.Item1 == ')').Reverse().ToList().ForEach(bracket => expandedLine = expandedLine.Insert(bracket.Item2 + 1, " ").Insert(bracket.Item2, " "));
                var tokens = expandedLine.Split(' ').Where(token => String.IsNullOrWhiteSpace(token) == false).ToList();

                var valueStack = new Stack<Int64>();
                var operationStack = new Stack<char>();

                Action solveUntilBracketOrEmpty = () =>
                    {
                        var operations = new List<char>();
                        var values = new List<Int64>();
                        int numOperations = 0;
                        while (operationStack.Count > 0 && operationStack.Peek() != '(')
                        {
                            ++numOperations;
                            operations.Insert(0, operationStack.Pop());
                        }
                        for (int i = 0; i < numOperations + 1; ++i)
                            values.Insert(0, valueStack.Pop());

                        for (int i = 0; i < operations.Count && additionTakesPriority; ++i)
                        {
                            if (operations[i] == '+')
                            {
                                values[i] = values[i] + values[i + 1];
                                values.RemoveAt(i + 1);
                                operations.RemoveAt(i--);
                            }
                        }

                        while (operations.Count > 0)
                        {
                            if (operations[0] == '+') values[0] = values[0] + values[1];
                            else if (operations[0] == '-') values[0] = values[0] - values[1];
                            else if (operations[0] == '/') values[0] = values[0] / values[1];
                            else if (operations[0] == '*') values[0] = values[0] * values[1];
                            operations.RemoveAt(0);
                            values.RemoveAt(1);
                        }
                        valueStack.Push(values[0]);
                    };

                for (int i = 0; i < tokens.Count; ++i)
                {
                    if (tokens[i] == "(") operationStack.Push('(');
                    else if (tokens[i] == ")")
                    {
                        solveUntilBracketOrEmpty();
                        operationStack.Pop();
                    }

                    else if (tokens[i] == "+" || tokens[i] == "-" || tokens[i] == "/" || tokens[i] == "*") operationStack.Push(tokens[i][0]);
                    else valueStack.Push(Int64.Parse(tokens[i]));
                }
                solveUntilBracketOrEmpty();
                sum += valueStack.Pop();
            }

            return sum;
        }

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            Console.WriteLine("Part 1: {0}", DoMaths(lines, false));
            Console.WriteLine("PArt 2: {0}", DoMaths(lines, true));
        }

    }

}