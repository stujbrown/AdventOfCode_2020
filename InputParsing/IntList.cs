using System;
using System.Collections.Generic;
using System.IO;

namespace InputParsing
{
    public static class IntList
    {
        public static List<int> ParseInputs(string path)
        {
            List<int> inputs = new List<int>();

            using (TextReader reader = File.OpenText(path))
            {
                while (reader.Peek() >= 0)
                {
                    string readLine = reader.ReadLine();
                    int entry = int.Parse(readLine);

                    inputs.Add(entry);
                }
            }

            return inputs;
        }
    }
}
