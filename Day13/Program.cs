using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            Int64 earliestTime = int.Parse(lines[0]);
            Int64[] busNumbers = Array.ConvertAll(lines[1].Split(',').ToArray(), item => item.Equals("x") ? -1 : Int64.Parse(item));

            Func<Int64, Int64, Int64> calcWaitTime = (Int64 bus, Int64 startTime) => ((Int64)Math.Ceiling((double)startTime / bus) * bus) - startTime;
            var ordered = busNumbers.Where(item => item != -1).OrderBy(bus => calcWaitTime(bus, earliestTime)).ToArray();

            Console.WriteLine("Best bus: {0} after {1} minutes = {2}", ordered.First(), calcWaitTime(ordered.First(), earliestTime), ordered.First() * calcWaitTime(ordered.First(), earliestTime));

            var busNumbersAndIndices = busNumbers.Select((bus, i) => new KeyValuePair<int, Int64>(i, bus)).ToList();
            busNumbersAndIndices.RemoveAll(x => x.Value == -1);

            Int64 time = 0;
            Int64 inc = busNumbersAndIndices.First().Value;
            busNumbersAndIndices.Skip(1).ToList().ForEach(x =>
            {
                while (((time + x.Key) % x.Value == 0) == false)
                {
                    time += inc;
                }

                inc *= x.Value;
            });

            Console.WriteLine("Timestamp for part 2: {0}", time);
        }
    }
}
