using System;
using System.Collections.Generic;
using System.IO;

namespace InputParsing
{
    public static class PasswordAndPolicyList
    {
        public struct Policy
        {
            public int repeatMin;
            public int repeatMax;
            public char repeatCharacter;
        };

        public struct Entry
        {
            public Policy policy;
            public string password;
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

                    var policy = new Policy { repeatMin = min, repeatMax = max, repeatCharacter = character };
                    var entry = new Entry { policy = policy, password = password };

                    inputs.Add(entry);
                }
            }

            return inputs;
        }
    }
}
