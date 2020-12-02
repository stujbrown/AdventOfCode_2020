using System;
using System.Collections.Generic;
using System.Linq;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            List<InputParsing.PasswordAndPolicyList.Entry> inputs = InputParsing.PasswordAndPolicyList.ParseInputs("input.txt");
            Console.WriteLine("Inputs: ");

            inputs.ForEach(
                input =>
                {
                    Console.WriteLine("[{0}-{1} {2}: {3}]", input.Policy.FirstInt, input.Policy.SecondInt, input.Policy.Character, input.Password);
                });

            Console.WriteLine();
            Console.WriteLine("Results: ");

            // Part 1
            int validCount = 0;
            foreach (var entry in inputs)
            {
                int count = entry.Password.Count(c => c == entry.Policy.Character);
                if (count >= entry.Policy.FirstInt && count <= entry.Policy.SecondInt)
                {
                    ++validCount;
                }
            }

            Console.WriteLine("{0} valid entries", validCount);

            // Part 2
            int validCount2 = 0;
            foreach (var entry in inputs)
            {
                bool firstValid = false;
                bool secondValid = false;
                int Index = entry.Policy.FirstInt - 1;
                if (Index < entry.Password.Count())
                {
                    firstValid = entry.Password[Index] == entry.Policy.Character;
                }

                Index = entry.Policy.SecondInt - 1;
                if (Index < entry.Password.Count())
                {
                    secondValid = entry.Password[Index] == entry.Policy.Character;
                }

                bool bothValid = firstValid && secondValid;

                if (bothValid == false &&
                    (firstValid || secondValid))
                {
                    ++validCount2;
                }
            }



            Console.WriteLine("{0} valid entries for part 2", validCount2);
        }
    }
}
