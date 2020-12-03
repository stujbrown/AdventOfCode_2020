using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day3
{
    class Program
    {
        public sealed class Map
        {
            CellType[,] CellGrid;
            public readonly int width;
            public readonly int height;

            public enum CellType
            {
                Free,
                Tree
            };

            public Map(int width, int height)
            {
                CellGrid = new CellType[width, height];
                for (int x = 0; x < CellGrid.GetLength(0); ++x)
                {
                    for (int y = 0; y < CellGrid.GetLength(1); ++y)
                    {
                        CellGrid[x, y] = CellType.Free;
                    }
                }

                this.width = width;
                this.height = height;
            }

            public void InitRow(int rowIndex, CellType[] values)
            {
                if (values.Length != CellGrid.GetLength(0))
                {
                    throw new Exception();
                }

                for (int x = 0; x < CellGrid.GetLength(0); ++x)
                {
                    CellGrid[x, rowIndex] = values[x];
                }
            }

            public CellType GetValueWrappedOnX(int x, int y)
            {
                int wrappedX = x % width;
                return CellGrid[wrappedX, y];
            }
        };

        public static Map ParseInputMap(string path)
        {
            int lastRowCount = -1;

            var cells = new List<List<Map.CellType>>();
            using (TextReader reader = File.OpenText(path))
            {
                while (reader.Peek() >= 0)
                {
                    var row = new List<Map.CellType>();
                    cells.Add(row);

                    int rowCount = 0;
                    string readLine = reader.ReadLine();
                    foreach (char c in readLine)
                    {
                        if (c == '.')
                        {
                            row.Add(Map.CellType.Free);
                            ++rowCount;
                        }
                        else if (c == '#')
                        {
                            row.Add(Map.CellType.Tree);
                            ++rowCount;
                        }
                    }

                    if (lastRowCount != -1 && rowCount != lastRowCount)
                    {
                        throw new Exception();
                    }

                    lastRowCount = rowCount;
                }
            }

            var map = new Map(lastRowCount, cells.Count());
            for (int rowIndex = 0; rowIndex < cells.Count(); ++rowIndex)
            {
                map.InitRow(rowIndex, cells[rowIndex].ToArray());
            }

            return map;
        }

        static void Main(string[] args)
        {
            Map map = ParseInputMap("input.txt");

            for (int y = 0; y < map.height; ++y)
            {
                for (int x = 0; x < map.width; ++x)
                {

                    var val = map.GetValueWrappedOnX(x, y);
                    if (val == Map.CellType.Free)
                    {
                        Console.Write(".");
                    }
                    else
                    {
                        Console.Write("#");
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("Result: ");

            long[] numTrees = new long[5];
            numTrees[0] = RunTest(map, 1, 1);
            numTrees[1] = RunTest(map, 3, 1);
            numTrees[2] = RunTest(map, 5, 1);
            numTrees[3] = RunTest(map, 7, 1);
            numTrees[4] = RunTest(map, 1, 2);

            long sum = numTrees[0] * numTrees[1] * numTrees[2] * numTrees[3] * numTrees[4];

            Console.WriteLine("Part 1");
            Console.WriteLine(numTrees[1]);
            Console.WriteLine("Part 2");
            Console.WriteLine("{0} * {1} * {2} * {3} * {4} = {5}", numTrees[0], numTrees[1], numTrees[2], numTrees[3], numTrees[4], sum);
        }

        static int RunTest(Map map, int moveX, int moveY)
        {
            int numTrees = 0;

            int posX = 0;
            int posY = 0;
            while (posY < map.height)
            {
                Map.CellType type = map.GetValueWrappedOnX(posX, posY);
                if (type == Map.CellType.Tree)
                {
                    ++numTrees;
                }

                posX += moveX;
                posY += moveY;
            }

            return numTrees;
        }
    }
}
