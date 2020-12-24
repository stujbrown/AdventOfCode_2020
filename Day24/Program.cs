using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day24
{
    class Program
    {
        struct HexCoordinate
        {
            public int X;
            public int Y;
            public int Z { get => -X - Y; }
        };

        readonly static HexCoordinate[] AdjacentOffsets = new HexCoordinate[6] { new HexCoordinate() { X = 1, Y = -1 }, new HexCoordinate() { X = 1, Y = 0 },  new HexCoordinate() { X = 0, Y = 1 },
            new HexCoordinate() { X = -1, Y = 1 }, new HexCoordinate() { X = -1, Y = 0 }, new HexCoordinate() { X = 0, Y = -1 } };

        static HexCoordinate[] GetNeighbours(Dictionary<HexCoordinate, bool> hexes, HexCoordinate location)
        {
            var neighbours = new HexCoordinate[6];
            for (int i = 0; i < 6; ++i)
                neighbours[i] = new HexCoordinate { X = location.X + AdjacentOffsets[i].X, Y = location.Y + AdjacentOffsets[i].Y };
            return neighbours;
        }

        static void Main(string[] args)
        {
            var allInstructions = File.ReadAllLines("input.txt").Select(instructionLine => Regex.Matches(instructionLine, @"e|se|sw|w|nw|ne"));
            var hexes = new Dictionary<HexCoordinate, bool>();
            foreach (var instructionSet in allInstructions)
            {
                var location = new HexCoordinate();
                foreach (var instruction in instructionSet)
                {
                    switch (instruction.ToString())
                    {
                        case "e": location.X += AdjacentOffsets[0].X; location.Y += AdjacentOffsets[0].Y; break;
                        case "se": location.X += AdjacentOffsets[1].X; location.Y += AdjacentOffsets[1].Y; break;
                        case "sw": location.X += AdjacentOffsets[2].X; location.Y += AdjacentOffsets[2].Y; break;
                        case "w": location.X += AdjacentOffsets[3].X; location.Y += AdjacentOffsets[3].Y; break;
                        case "nw": location.X += AdjacentOffsets[4].X; location.Y += AdjacentOffsets[4].Y; break;
                        case "ne": location.X += AdjacentOffsets[5].X; location.Y += AdjacentOffsets[5].Y; break;
                    }
                }

                bool isBlack = false;
                hexes.TryGetValue(location, out isBlack);
                hexes[location] = !isBlack;
            }

            Console.WriteLine("Num tiles set to black at start: {0}", hexes.Values.ToList().Where(value => value == true).Count());

            var coordinatesToCheck = new HashSet<HexCoordinate>();
            for (int turnIndex = 0; turnIndex < 100; ++turnIndex)
            {
                var newHexes = new Dictionary<HexCoordinate, bool>(hexes);
                foreach (var hex in hexes)
                {
                    foreach (var coordinateToCheck in GetNeighbours(hexes, hex.Key).Append(hex.Key))
                    {
                        int blackNeighbours = 0;
                        foreach (var neighbourCoordinate in GetNeighbours(hexes, coordinateToCheck))
                        {
                            if (hexes.GetValueOrDefault(neighbourCoordinate, false))
                                blackNeighbours++;
                        }

                        if (hexes.GetValueOrDefault(coordinateToCheck, false))
                        {
                            if (blackNeighbours == 0 || blackNeighbours > 2)
                                newHexes[coordinateToCheck] = false;
                        }
                        else if (blackNeighbours == 2)
                            newHexes[coordinateToCheck] = true;
                    }
                }
                hexes = newHexes;
            }

            Console.WriteLine("Num tiles set to black after updates: {0}", hexes.Values.ToList().Where(value => value == true).Count());
        }
    }
}
