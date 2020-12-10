using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day10
{
    class Program
    {
        static readonly int maxDifference = 3;

        static void Main(string[] args)
        {
            List<Int64> sortList = Array.ConvertAll(File.ReadAllLines("input.txt"), item => Int64.Parse(item)).ToList();
            sortList.Sort();
            Int64 deviceJoltage = sortList.Last() + 3;
            sortList.Add(deviceJoltage);
            Int64[] numbers = sortList.ToArray();

            var distribution = new int[maxDifference];

            // Part 1
            Int64 currentJoltage = 0;
            foreach (var number in numbers)
            {
                var diff = number - currentJoltage;
                if (diff > 0 && diff <= maxDifference)
                {
                    ++distribution[diff - 1];
                    currentJoltage = number;
                }
            }

            // Part 2
            var numPossibilities = new Int64[numbers.Length];
            numPossibilities[numbers.Length - 1] = 1;
            for (int i = numbers.Length - 2; i >= 0; --i)
            {
                int offset = 1;
                while ((i + offset) < numbers.Length &&
                   (numbers[i + offset] - numbers[i]) > 0 &&
                   (numbers[i + offset] - numbers[i]) <= maxDifference)
                {
                    numPossibilities[i] += numPossibilities[i + offset];
                    ++offset;
                }
            }

            Int64 validArrangements = 0;
            int countIndex = 0; numbers.TakeWhile(x => (x <= maxDifference)).ToList().ForEach(x => validArrangements += numPossibilities[countIndex++]);

            Console.WriteLine("Distribution of 1 [{0}] * distribution of 3 [{1}] = {2}", distribution[0], distribution[2], distribution[0] * distribution[2]);
            Console.WriteLine("Valid arrangements: {0}", validArrangements);
        }
    }
}
