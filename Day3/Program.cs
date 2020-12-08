using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");

            Console.WriteLine("Result: ");

            long[] numTrees = new long[5];
            numTrees[0] = RunTest(lines, 1, 1);
            numTrees[1] = RunTest(lines, 3, 1);
            numTrees[2] = RunTest(lines, 5, 1);
            numTrees[3] = RunTest(lines, 7, 1);
            numTrees[4] = RunTest(lines, 1, 2);

            long sum = numTrees[0] * numTrees[1] * numTrees[2] * numTrees[3] * numTrees[4];

            Console.WriteLine("Part 1");
            Console.WriteLine(numTrees[1]);
            Console.WriteLine("Part 2");
            Console.WriteLine("{0} * {1} * {2} * {3} * {4} = {5}", numTrees[0], numTrees[1], numTrees[2], numTrees[3], numTrees[4], sum);
        }

        static int RunTest(string[] map, int moveX, int moveY)
        {
            int width = map[0].Length;

            int numTrees = 0;

            int posX = 0;
            int posY = 0;
            while (posY < map.Length)
            {
                switch (map[posY][posX % width])
                {
                    case '#':
                        ++numTrees;
                        break;
                }

                posX += moveX;
                posY += moveY;
            }

            return numTrees;
        }
    }
}
