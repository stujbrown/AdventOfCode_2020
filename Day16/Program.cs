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


            Int64 errorRate = 0;
            
            var validFieldsForEachTicketIndex = new List<int>[myTicket.Length].Select(x => new List<int>(Enumerable.Range(0, fields.Keys.Count))).ToList();
            foreach (Int64[] ticket in nearbyTickets)
            {
                for (int ticketFieldIndex = 0; ticketFieldIndex < ticket.Length; ++ticketFieldIndex)
                {
                    var validRanges = fields.Values.Select((ranges, index) => Tuple.Create(ranges, index)).Where(ranges => ranges.Item1.Where(range => ticket[ticketFieldIndex] >= range.Item1 && ticket[ticketFieldIndex] <= range.Item2).Count() != 0);
                    errorRate += (validRanges.Count() == 0) ? ticket[ticketFieldIndex] : 0;

                    if (validRanges.Count() != 0) // ticket valid
                    {
                        foreach (var value in Enumerable.Range(0, fields.Keys.Count).Except(validRanges.Select(ranges => ranges.Item2)))
                        {
                            validFieldsForEachTicketIndex[ticketFieldIndex].Remove(value);
                        }
                    }
                }
            }

            var fieldIndices = new int[fields.Keys.Count];
            for (int indicesFilled = 0; indicesFilled < fieldIndices.Length; ++indicesFilled)
            {
                var validIndicesForField = validFieldsForEachTicketIndex.Select((indices, index) => Tuple.Create(new List<int>(indices), index)).Where(fieldIndexList => fieldIndexList.Item1.Count == 1).ToArray();
                fieldIndices[validIndicesForField[0].Item2] = validIndicesForField[0].Item1[0];

                for (int x = 0; x < validFieldsForEachTicketIndex.Count; ++x)
                {
                    validFieldsForEachTicketIndex[x].Remove(validIndicesForField[0].Item1[0]);
                }
            }

            var departureValues = new List<Int64>();
            for (int fieldIndex = 0; fieldIndex < fields.Keys.Count; ++fieldIndex)
            {
                if (fields.Keys.ToArray()[fieldIndex].StartsWith("departure"))
                {
                     departureValues.Add(myTicket[fieldIndices.ToList().IndexOf(fieldIndex)]);
                }
            }

            Console.WriteLine("Error rate: {0}", errorRate);
            Console.WriteLine("Ticket product: {0}", departureValues.Aggregate((lhs, rhs) => lhs * rhs));
        }
    }
}
