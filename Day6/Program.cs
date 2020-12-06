using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day6
{
    using CharacterCountDictionary = Dictionary<char, int>;

    class Program
    {
        struct Group
        {
            public CharacterCountDictionary Answers;
            public int NumPeople;
        };

        static void Main(string[] args)
        {
            var groups = new List<Group>();
            var currentAnswers = new CharacterCountDictionary();
            int numPeople = 0;

            using (TextReader reader = File.OpenText("input.txt"))
            {
                Action recordGroup = () =>
                {
                    if (numPeople > 0)
                    {
                        groups.Add(new Group() { Answers = currentAnswers, NumPeople = numPeople });
                        currentAnswers = new CharacterCountDictionary();
                        numPeople = 0;
                    }
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
                        ++numPeople;
                        foreach (char character in line)
                        {
                            int count;
                            if (currentAnswers.TryGetValue(character, out count))
                            {
                                ++count;
                            }
                            else
                            {
                                count = 1;
                            }
                            currentAnswers[character] = count;
                        }
                    }
                }

                recordGroup();

                int uniqueSum = 0;
                int unifiedSum = 0;
                foreach (var group in groups)
                {
                    uniqueSum += group.Answers.Count;
                    group.Answers.Values.ToList().ForEach(answerCount => unifiedSum += (group.NumPeople == answerCount) ? 1 : 0);
                }
                Console.WriteLine("Sum of unique answers per-group: {0}", uniqueSum);
                Console.WriteLine("Sum of unified answers per-group: {0}", unifiedSum);
            }
        }
    }
}
