using System;
using System.IO;
using System.Linq;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            Int64[] numbers = Array.ConvertAll(File.ReadAllLines("input.txt"), item => Int64.Parse(item));

            int preambleSize = 25;

            for (int currentIndex = preambleSize; currentIndex < numbers.Length; ++currentIndex)
            {
                var preamble = new ArraySegment<Int64>(numbers, currentIndex - preambleSize, preambleSize);
                var joinedResults = preamble.SelectMany(lhs => preamble.Where(x => x != lhs), (lhs, rhs) => lhs + rhs);

                // Part 1
                if (joinedResults.Contains(numbers[currentIndex]) == false)
                {
                    Console.WriteLine("Invalid number: {0}", numbers[currentIndex]);
                }
            }
        }
    }
}
