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
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");

            //   ((((1 + 2) * 3) + 4) * 5) + 6

            foreach (var line in lines)
            {
                string expandedLine = line;
                line.Select((c, index) => Tuple.Create(c, index)).Where(pair => pair.Item1 == '(' || pair.Item1 == ')').Reverse().ToList().ForEach(bracket => expandedLine = expandedLine.Insert(bracket.Item2 + 1, " ").Insert(bracket.Item2, " "));
                var tokens = expandedLine.Split(' ').Where(token => String.IsNullOrWhiteSpace(token) == false).ToList();

                var values = new Stack<Int64>();
                var operations = new Stack<char>();
                for (int i = 0; i < tokens.Count; ++i)
                {
                    if (tokens[i] == "(") ;
                    else if (tokens[i] == ")") ;
                    else if (tokens[i] == "+" || tokens[i] == "-" || tokens[i] == "/" || tokens[i] == "*")
                    {
                        operations.Push(tokens[i][0]);
                    }
                    else
                    {
                        values.Push(Int64.Parse(tokens[i]));
                    }
                }

                Int64 acc = 0;

                //Int64 acc = 0;
                //var newString = new StringBuilder();
                //foreach (var parenthesis in stack)
                //{
                //    var substr = line[i]
                //    var table = new DataTable();
                //    table.Compute("", "");
                //}
            }

        }
    }
}