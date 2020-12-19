using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Day19
{
    class Program
    {
        static string ExpandRule(string[] rules, string rule, Dictionary<int, int> ruleDepth)
        {
            int index = int.Parse(rule.Split(':')[0]);
            if (ruleDepth.GetValueOrDefault(index) > 10)
                return ".*";

            ruleDepth[index] = ruleDepth.GetValueOrDefault(index) + 1;

            var current = new StringBuilder();
            foreach (var token in rule.Split(':')[1].Trim().Split(' '))
            {
                if (token.Equals("|"))
                    current.Append("|");
                else if (token.StartsWith('"'))
                    current.Append(token.Substring(1, token.Length - 2));
                else
                    current.Append("(" + ExpandRule(rules, rules[Int64.Parse(token)], ruleDepth) + ")");
            }

            ruleDepth[int.Parse(rule.Split(':')[0])]--;
            return current.ToString();
        }

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var rules = lines.ToList().TakeWhile(line => String.IsNullOrWhiteSpace(line) == false).ToList();
            rules.Sort((lhs, rhs) => int.Parse(lhs.Split(':')[0]) < int.Parse(rhs.Split(':')[0]) ? -1 : 1);
            var sparseRules = new List<string>();
            for (int i = 0; i < rules.Count; ++i)
            {
                while (sparseRules.Count < int.Parse(rules[i].Split(':')[0]))
                    sparseRules.Add(null);

                sparseRules.Add(rules[i]);
            }

            var messages = lines.ToList().SkipWhile(line => String.IsNullOrWhiteSpace(line) == false).Skip(1).ToList();

            var rule = ExpandRule(sparseRules.ToArray(), sparseRules[0], new Dictionary<int, int>());
            var matches = messages.Where(message => Regex.Match(message, "^" + rule + "$").Success);
            Console.WriteLine("Part 1 num matches: {0}", matches.Count());

            sparseRules[8] = "8: 42 | 42 8";
            sparseRules[11] = "11: 42 31 | 42 11 31";

            rule = ExpandRule(sparseRules.ToArray(), sparseRules[0], new Dictionary<int, int>());
            matches = messages.Where(message => Regex.Match(message, "^" + rule + "$").Success);
            Console.WriteLine("Part 2 num matches: {0}", matches.Count());
        }
    }
}
