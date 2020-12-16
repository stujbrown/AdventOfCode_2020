using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day16
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var fieldLines = lines.ToList().TakeWhile(line => String.IsNullOrWhiteSpace(line) == false).ToList();

            var fields = new Dictionary<string, Tuple<Int64, Int64>[]>();
            fieldLines.ForEach(line =>
            {
                var ranges = Array.ConvertAll(line.Split(":")[1].Split("or"), range => Tuple.Create<Int64, Int64>(Int64.Parse(range.Split("-")[0]), Int64.Parse(range.Split("-")[1])));
                fields.Add(line.Split(":")[0], ranges);
            });

            Int64[][] nearbyTickets = Array.ConvertAll(lines.ToList().SkipWhile(line => line.Contains("nearby tickets:") == false).Skip(1).ToArray(), line => Array.ConvertAll(line.Split(","), val => Int64.Parse(val)));

            Int64 errorRate = 0;
            foreach (Int64[] ticket in nearbyTickets)
            {
                foreach (var ticketValue in ticket)
                {
                    bool fits = false;
                    fields.Values.ToList().ForEach(ranges => ranges.ToList().ForEach(range =>
                    {
                        fits |= (ticketValue >= range.Item1 && ticketValue <= range.Item2);
                    }));

                    if (fits == false)
                    {
                        errorRate += ticketValue;
                    }
                }

            }

            Console.WriteLine("Error rate: {0}", errorRate);
        }
    }
}
