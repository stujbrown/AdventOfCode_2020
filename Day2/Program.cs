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
                    Console.WriteLine("[{0}-{1} {2}: {3}]", input.policy.repeatMin, input.policy.repeatMax, input.policy.repeatCharacter, input.password);
                });

            Console.WriteLine();
            Console.WriteLine("Results: ");

            int validCount = 0;
            foreach (var entry in inputs)
            {
                int count = entry.password.Count(c => c == entry.policy.repeatCharacter);
                if (count >= entry.policy.repeatMin && count <= entry.policy.repeatMax)
                {
                    validCount += 1;
                }
            }

            Console.WriteLine("{0} valid entries", validCount);
        }
    }
}
