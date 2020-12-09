using System;
using System.IO;
using System.Linq;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            const int preambleSize = 25;
            Int64[] numbers = Array.ConvertAll(File.ReadAllLines("input.txt"), item => Int64.Parse(item));

            Int64 badNumber = -1;
            for (int currentIndex = preambleSize; currentIndex < numbers.Length; ++currentIndex)
            {
                var preamble = new ArraySegment<Int64>(numbers, currentIndex - preambleSize, preambleSize);
                var joinedResults = preamble.SelectMany(lhs => preamble.Where(x => x != lhs), (lhs, rhs) => lhs + rhs);

                // Part 1
                if (joinedResults.Contains(numbers[currentIndex]) == false)
                {
                    badNumber = numbers[currentIndex];
                    Console.WriteLine("Invalid number: {0}", numbers[currentIndex]);
                    break;
                }
            }

            // Part 2
            for (int currentIndex = 0; currentIndex < numbers.Length; ++currentIndex)
            {
                Int64 total = numbers[currentIndex];
                for (int advanceIndex = currentIndex + 1; advanceIndex < numbers.Length; ++advanceIndex)
                {
                    total += numbers[advanceIndex];
                    if (total == badNumber)
                    {
                        var smallest = new ArraySegment<Int64>(numbers, currentIndex, advanceIndex - currentIndex).ToList().Min();
                        var largest = new ArraySegment<Int64>(numbers, currentIndex, advanceIndex - currentIndex).ToList().Max();
                        Console.WriteLine("Found range {0}+{1} = {2}", smallest, largest, smallest + largest);
                        break;
                    }
                }
            }
        }
    }
}
