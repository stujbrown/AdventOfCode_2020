using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day15
{
    class Program
    {
        static void PlayGame(Int64[] seedNumbers, int numTurns)
        {
            var spoken = new Dictionary<Int64, Int64>();
            for (int i = 0; i < seedNumbers.Length; ++i)
                spoken.Add(seedNumbers[i], i);

            Int64 last = seedNumbers.Last();
            for (int count = seedNumbers.Length; count < numTurns; ++count)
            {
                Int64 num = 0;
                Int64 turnCount;
                if (spoken.TryGetValue(last, out turnCount))
                    num = count - 1 - turnCount;

                spoken[last] = count - 1;
                last = num;
            }

            Console.WriteLine("Last spoken in {0} turn game: {1}", numTurns, last);
        }

        static void Main(string[] args)
        {
            Int64[] numbers = Array.ConvertAll(File.ReadAllLines("input.txt")[0].Split(","), item => Int64.Parse(item)).ToArray();
            PlayGame(numbers, 2020);
            PlayGame(numbers, 30000000);
        }
    }
}
