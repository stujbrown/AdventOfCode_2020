using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = File.ReadAllLines("input.txt");

            string mask = "0";
            Int64[] memory = new Int64[1024 * 1024];

            // Part 1
            foreach (var line in program)
            {
                string[] instructionAndValue = line.Split(" = ");
                if (instructionAndValue[0].Equals("mask"))
                {
                    mask = instructionAndValue[1];
                }
                else
                {
                    Int64 memOffset = Int64.Parse(Regex.Match(instructionAndValue[0], @"\[([0-9]+)\]").Groups[1].Value);
                    Int64 value = (Convert.ToInt64(mask.Replace("X", "1"), 2) & Int64.Parse(instructionAndValue[1])) | Convert.ToInt64(mask.Replace("X", "0"), 2);
                    memory[memOffset] = value;
                }
            }

            Int64 total = 0;
            memory.ToList().ForEach(x => total += x);
            Console.WriteLine("Total sum: {0}", total);

            // Part 2
            var part2Memory = new Dictionary<Int64, Int64>();
            foreach (var line in program)
            {
                string[] instructionAndValue = line.Split(" = ");
                if (instructionAndValue[0].Equals("mask"))
                {
                    mask = instructionAndValue[1];
                }
                else
                {
                    Int64 memOffset = Int64.Parse(Regex.Match(instructionAndValue[0], @"\[([0-9]+)\]").Groups[1].Value);
                    Int64 adjustedAddress = Convert.ToInt64(mask.Replace('X', '0'), 2) | memOffset;
                    char[] adjustedAddressStr = Convert.ToString(adjustedAddress, 2).PadLeft(mask.Length, '0').ToCharArray();

                    char[] mutableAddress = mask.ToList().Select((x, index) =>
                    {
                        return (x == 'X') ? adjustedAddressStr[index] = 'X' : adjustedAddressStr[index];
                    }).ToArray();

                    visitPermutations(mutableAddress, 0, address => part2Memory[Convert.ToInt64(address, 2)] = Int64.Parse(instructionAndValue[1]));
                }
            }

            total = 0;
            part2Memory.ToList().ForEach(x => total += x.Value);
            Console.WriteLine("Total sum part 2: {0}", total);
        }

        static void visitPermutations(char[] mask, int position, Action<string> action)
        {
            if (position == mask.Length)
            {
                action(new string(mask));
                return;
            }

            if (mask[position] == 'X')
            {
                mask[position] = '0';
                visitPermutations(mask, position + 1, action);
                mask[position] = '1';
                visitPermutations(mask, position + 1, action);
                mask[position] = 'X'; // repair mutated string
            }
            else
            {
                visitPermutations(mask, position + 1, action);
            }
        }
    }

}
