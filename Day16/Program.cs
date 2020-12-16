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
            Int64[][] nearbyTickets = Array.ConvertAll(lines.ToList().SkipWhile(line => line.Contains("nearby tickets:") == false).Skip(1).ToArray(), line => Array.ConvertAll(line.Split(","), val => Int64.Parse(val)));
            Int64[] myTicket = Array.ConvertAll(lines.ToList().SkipWhile(line => line.Contains("your ticket:") == false).Skip(1).ToArray()[0].Split(","), val => Int64.Parse(val));

            var fields = new Dictionary<string, Tuple<Int64, Int64>[]>();
            fieldLines.ForEach(line =>
            {
                var ranges = Array.ConvertAll(line.Split(":")[1].Split("or"), range => Tuple.Create<Int64, Int64>(Int64.Parse(range.Split("-")[0]), Int64.Parse(range.Split("-")[1])));
                fields.Add(line.Split(":")[0], ranges);
            });


            var validNearbyTickets = new List<Int64[]>();
            Int64 errorRate = 0;

            var invalidFieldsPerIndex = new List<int>[fields.Keys.Count].Select(x => new List<int>()).ToArray();
            foreach (Int64[] ticket in nearbyTickets)
            {
                for (int valueIndex = 0; valueIndex < ticket.Length; ++valueIndex)
                {
                    var validRanges = fields.Values.Select((ranges, index) => Tuple.Create(ranges, index)).Where(ranges => ranges.Item1.Where(range => ticket[valueIndex] >= range.Item1 && ticket[valueIndex] <= range.Item2).Count() != 0);
                    errorRate += (validRanges.Count() == 0) ? ticket[valueIndex] : 0;

                    if (validRanges.Count() != 0) // ticket valid
                    {
                        for (int fieldIndex = 0; fieldIndex < fields.Values.Count(); ++fieldIndex)
                        {
                            if (validRanges.Where(rangeEntry => rangeEntry.Item2 == fieldIndex).Count() == 0)
                            {
                                invalidFieldsPerIndex[valueIndex].Add(fieldIndex);
                                break;
                            }
                        }
                    }
                }
            }

            var sortedInvalidFieldsPerIndex = invalidFieldsPerIndex.Select((list, index) => Tuple.Create(list, index)).ToList();
            sortedInvalidFieldsPerIndex.Sort((lhs, rhs) => lhs.Item1.Count < rhs.Item1.Count ? 1 : -1);

            var fieldIndices = new int[fields.Keys.Count];
            List<int> remainingIndices = Enumerable.Range(0, fields.Keys.Count).ToList();
            foreach (var invalidFields in sortedInvalidFieldsPerIndex)
            {
                fieldIndices[invalidFields.Item2] = remainingIndices.Except(invalidFields.Item1).ToArray()[0];
                remainingIndices.Remove(fieldIndices[invalidFields.Item2]);
            }


            var departureValues = new List<Int64>();
            for (int fieldIndex = 0; fieldIndex < fields.Keys.Count; ++fieldIndex)
            {
                if (fields.Keys.ToArray()[fieldIndex].StartsWith("departure"))
                    departureValues.Add(myTicket[fieldIndices[fieldIndex]]);
            }
            
            for (int fieldIndex = 0; fieldIndex < fields.Keys.Count; ++fieldIndex)
            {

                Console.WriteLine("{0} : {1}", fields.Keys.ToArray()[fieldIndex], myTicket[fieldIndices[fieldIndex]]);
            }



            Console.WriteLine("Error rate: {0}", errorRate);
            Console.WriteLine("Ticket product: {0}", departureValues.Aggregate((lhs, rhs) => lhs * rhs));
        }
    }
}
