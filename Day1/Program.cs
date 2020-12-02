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

            // 2 numbers
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

            // 3 numbers
            for (int i = 0; i < inputs.Count; ++i)
            {
                for (int j = 0; j < inputs.Count; ++j)
                {
                    for (int k = 0; k < inputs.Count; ++k)
                    {
                        if (i != j && j != k)
                        {
                            int var1 = inputs[i];
                            int var2 = inputs[j];
                            int var3 = inputs[k];

                            if (var1 + var2 + var3 == 2020)
                            {
                                Console.WriteLine("{0} * {1} * {2} = {3}", var1, var2, var3, var1 * var2 * var3);
                            }
                        }
                    }
                }
            }
        }
    }
}
