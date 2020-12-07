using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace Day7
{

    class Program
    {
        struct ContainedBagRule
        {
            public string ContainedBag;
            public int Count;
        };


        static bool FindInBagRecursive(Dictionary<string, List<ContainedBagRule>> dictionary, string startBag, string searchBag)
        {
            if (startBag.Equals(searchBag))
            {
                return true;
            }

            bool found = false;
            List<ContainedBagRule> rules;
            if (dictionary.TryGetValue(startBag, out rules))
            {
                rules.ForEach(rule => found |= FindInBagRecursive(dictionary, rule.ContainedBag, searchBag));
            }
            return found;
        }
        static int CountBagsRecursive(Dictionary<string, List<ContainedBagRule>> dictionary, string startBag, int numBags)
        {
            int count = numBags;
            List<ContainedBagRule> rules;
            if (dictionary.TryGetValue(startBag, out rules))
            {
                rules.ForEach(rule => count += CountBagsRecursive(dictionary, rule.ContainedBag, numBags * rule.Count));
            }

            return count;
        }

        static void Main(string[] args)
        {
            var ruleDictionary = new Dictionary<string, List<ContainedBagRule>>();

            using (TextReader reader = File.OpenText("input.txt"))
            {
                while (reader.Peek() >= 0)
                {
                    string[] segments = reader.ReadLine().Split(',');

                    // If not matched it's a dead-end colour
                    Match match = Regex.Match(segments[0], @"^(.+?)bags contain ([0-9]+) (.+?) bag");
                    if (match.Success)
                    {
                        var rules = new List<ContainedBagRule>();
                        rules.Add(new ContainedBagRule() { ContainedBag = match.Groups[3].Value.Trim(), Count = int.Parse(match.Groups[2].Value) });
                        for (int segmentIndex = 1; segmentIndex < segments.Length; ++segmentIndex)
                        {
                            Match subMatch = Regex.Match(segments[segmentIndex], @"([0-9]+) (.+?) bag");
                            if (subMatch.Success)
                            {
                                rules.Add(new ContainedBagRule() { ContainedBag = subMatch.Groups[2].Value.Trim(), Count = int.Parse(subMatch.Groups[1].Value) });
                            }
                        }

                        ruleDictionary.Add(match.Groups[1].Value.Trim(), rules);
                    }
                }
            }

            string queryBag = "shiny gold";

            // Part 1
            int numValidBags = 0;
            var keysToCheck = ruleDictionary.Keys.Where(x => x.Equals(queryBag) == false);
            keysToCheck.ToList().ForEach(x => numValidBags += FindInBagRecursive(ruleDictionary, x, queryBag) == true ? 1 : 0);

            // Part 2
            int numContained = CountBagsRecursive(ruleDictionary, queryBag, 1) - 1;

            Console.WriteLine("Num bags which could contain {0}: {1}", queryBag, numValidBags);
            Console.WriteLine("Total bags inside {0}: {1}", queryBag, numContained);
        }
    }
}
