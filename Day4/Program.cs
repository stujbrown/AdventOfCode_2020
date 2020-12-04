using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Day4
{
    class Program
    {
        static string[] Fields = new string[]
        {
            "byr",
            "iyr",
            "eyr",
            "hgt",
            "hcl",
            "ecl",
            "pid",
        };

        static string[] FieldPatterns = new string[]
        {
             @"(byr:(19[2-8][0-9]|199[0-9]|200[0-2]))\b",
             @"(iyr:(201[0-9]|2020))\b",
             @"(eyr:(202[0-9]|2030))\b",
             @"(hgt:(1[5-8][0-9]|19[0-3])cm|hgt:(59|6[0-9]|7[0-6])in)\b",
             @"(hcl:#[0-9a-f]{6})\b",
             @"(ecl:amb|blu|brn|gry|grn|hzl|oth)\b",
             @"(pid:[0-9]{9})\b",
        };

        static void Main(string[] args)
        {
            List<string> passportStrings = new List<string>();
            {
                StringBuilder builder = new StringBuilder();

                string str;
                using (TextReader reader = File.OpenText("input.txt"))
                {
                    while (reader.Peek() >= 0)
                    {
                        string readLine = reader.ReadLine();
                        if (String.IsNullOrWhiteSpace(readLine))
                        {
                            str = builder.ToString();
                            if (str.Length > 0)
                            {
                                passportStrings.Add(str);
                            }
                            builder.Clear();
                        }
                        else
                        {
                            builder.Append(readLine);

                            // Need space to seperate where newline chars were
                            builder.Append(" ");
                        }

                    }
                }

                str = builder.ToString();
                if (str.Length > 0)
                {
                    passportStrings.Add(str);
                }
            }

            int numValid = 0;

            // Parse entries
            foreach (string passport in passportStrings)
            {
                Console.WriteLine(passport);
                Console.WriteLine();

                bool isValid = true;
                foreach (string field in Fields)
                {
                    Match match = Regex.Match(passport, "(" + field + @":[^\s]+)");
                    if (!match.Success)
                    {
                        isValid = false;
                    }
                }

                if (isValid)
                {
                    ++numValid;
                }
            }

            Console.WriteLine("Num Valid: " + numValid);

            int numValidWithFullValidation = 0;

            foreach (string passport in passportStrings)
            {
                bool isValid = true;
                foreach (string fieldPattern in FieldPatterns)
                {
                    Match match = Regex.Match(passport, fieldPattern);
                    if (!match.Success)
                    {
                        isValid = false;
                    }
                }

                if (isValid)
                {
                    ++numValidWithFullValidation;
                }
            }

            Console.WriteLine("Num Valid with full validation: " + numValidWithFullValidation);
        }
    }
}
