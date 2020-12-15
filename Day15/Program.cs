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
            var spoken = new List<Int64>();
            spoken.AddRange(seedNumbers);

            Int64 last = seedNumbers.Last();
            for (int count = spoken.Count; count < numTurns; ++count)
            {
                Int64 num = 0;
                int idx = spoken.LastIndexOf(last, spoken.Count - 2);
                if (idx != -1)
                {
                    num = spoken.Count - 1 - idx;
                }

                last = num;
                spoken.Add(last);
            }

            Console.WriteLine("Last spoken in {0} turn game: {1}", numTurns, last);
        }

        static void Main(string[] args)
        {
            Int64[] numbers = Array.ConvertAll(File.ReadAllLines("input.txt")[0].Split(","), item => Int64.Parse(item)).ToArray();
            PlayGame(numbers, 2020);
           // PlayGame(numbers, 30000000);
        }
    }
}
