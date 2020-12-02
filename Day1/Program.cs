using System;
using System.Collections.Generic;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> inputs = InputParsing.IntList.ParseInputs("input.txt");
            Console.WriteLine("Inputs: ");
            inputs.ForEach(Console.WriteLine);

            Console.WriteLine();
            Console.WriteLine("Results: ");
            for (int i = 0; i < inputs.Count; ++i)
            {
                for (int j = 0; j < inputs.Count; ++j)
                {
                    if (i != j)
                    {
                        int lhs = inputs[i];
                        int rhs = inputs[j];

                        if (lhs + rhs == 2020)
                        {
                            Console.WriteLine("{0} * {1} = {2}", lhs, rhs, lhs * rhs);
                        }
                    }
                }
            }
        }
    }
}
