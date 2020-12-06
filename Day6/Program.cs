using System;
using System.Collections.Generic;
using System.IO;

namespace Day6
{
    using CharacterSet = HashSet<char>;

    class Program
    {
        static void Main(string[] args)
        {
            List<CharacterSet> answerGroups = new List<CharacterSet>();
            CharacterSet currentGroup = new CharacterSet();
            using (TextReader reader = File.OpenText("input.txt"))
            {
                Action recordGroup = () =>
                {
                    answerGroups.Add(currentGroup);
                    currentGroup = new CharacterSet();
                };

                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    if (String.IsNullOrWhiteSpace(line))
                    {
                        recordGroup();
                    }
                    else
                    {
                        foreach (char character in line)
                        {
                            currentGroup.Add(character);
                        }
                    }
                }

                recordGroup();

                int sum = 0;
                foreach (CharacterSet group in answerGroups)
                {
                    sum += group.Count;
                }
                Console.WriteLine("Sum: {0}", sum );
            }
        }
    }
}
