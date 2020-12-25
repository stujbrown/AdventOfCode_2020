using System;
using System.IO;
using System.Linq;

namespace Day25
{
    class Program
    {
        static void Main(string[] args)
        {
            long[] publicKeys = Array.ConvertAll(File.ReadAllLines("input.txt"), line => long.Parse(line));

            int testSubject = 7;
            var keys = new long[2] { 1, 1 };
            var loopCounts = new int[2] { 0, 0 };
            for (int keyIndex = 0; keyIndex < 2; ++keyIndex)
            {
                while (keys[keyIndex] != publicKeys[keyIndex])
                {
                    keys[keyIndex] = (keys[keyIndex] * testSubject) % 20201227L;
                    ++loopCounts[keyIndex];
                }
            }

            long encryptionKey = 1;
            Array.ForEach(Enumerable.Range(0, loopCounts[0]).ToArray(), i => encryptionKey = (encryptionKey * keys[1]) % 20201227L);
            Console.WriteLine("Encryption key: {0}", encryptionKey);
        }
    }
}
