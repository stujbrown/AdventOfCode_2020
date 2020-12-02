using System;
using System.Collections.Generic;
using System.IO;

namespace InputParsing
{
    public static class PasswordAndPolicyList
    {
        public struct Policy
        {
            public int FirstInt;
            public int SecondInt;
            public char Character;
        };

        public struct Entry
        {
            public Policy Policy;
            public string Password;
        }


        public static List<Entry> ParseInputs(string path)
        {
            var inputs = new List<Entry>();

            using (TextReader reader = File.OpenText(path))
            {
                while (reader.Peek() >= 0)
                {
                    string readLine = reader.ReadLine();
                    string[] policyAndPassword = readLine.Split(new char[] { ':' });
                    string[] policyMinMaxAndChar = policyAndPassword[0].Split(new char[] { '-', ' ' });

                    int min = int.Parse(policyMinMaxAndChar[0]);
                    int max = int.Parse(policyMinMaxAndChar[1]);
                    char character = char.Parse(policyMinMaxAndChar[2]);

                    string password = policyAndPassword[1].Trim();

                    var policy = new Policy { FirstInt = min, SecondInt = max, Character = character };
                    var entry = new Entry { Policy = policy, Password = password };

                    inputs.Add(entry);
                }
            }

            return inputs;
        }
    }
}
