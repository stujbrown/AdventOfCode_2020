using System;
using System.Collections.Generic;
using System.IO;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> codes = new List<string>();
            using (TextReader reader = File.OpenText("input.txt"))
            {
                while (reader.Peek() >= 0)
                {
                    string line = reader.ReadLine();
                    codes.Add(line);
                }
            }

            int highestId = 0;
            SortedSet<int> sortedIds = new SortedSet<int>();
            foreach (string code in codes)
            {
                Func<string, int, int, char, char, int> BinaryChop = (string str, int lowerStart, int upperStart, char lowerChar, char upperChar) =>
                {
                    int lower = lowerStart;
                    int upper = upperStart;

                    for (int i = 0; i < str.Length; ++i)
                    {
                        int range = upper - lower;
                        if (str[i] == lowerChar)
                        {
                            upper -= range / 2;
                        }
                        else if (str[i] == upperChar)
                        {
                            lower += range / 2;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }

                    if (upper - lower != 1)
                    {
                        throw new Exception();
                    }

                    return lower;
                };

                string rows = code.Substring(0, 7);
                int row = BinaryChop(rows, 0, 128, 'F', 'B');
                string columns = code.Substring(7, 3);
                int column = BinaryChop(columns, 0, 8, 'L', 'R');

                int id = (row * 8) + column;
                Console.WriteLine("{0}: row {1} column {2} - ID {3}", code, row, column, id);

                sortedIds.Add(id);
                highestId = Math.Max(highestId, id);
            }

            Console.WriteLine("Highest ID: {0}", highestId);

            int lastID = -1;
            foreach (int id in sortedIds)
            {
                if (id - lastID == 2)
                {
                    Console.WriteLine("ID space at: {0}", lastID + 1);
                }
                lastID = id;
            }
        }
    }
}
