using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Day17
{
    class Program
    {
        struct IntVector { public Int64 X; public Int64 Y; public Int64 Z; public Int64 W; };

        static Dictionary<IntVector, bool> UpdateCells(Dictionary<IntVector, bool> cellStates, List<IntVector> offsets)
        {
            var updatedCellStates = new Dictionary<IntVector, bool>(cellStates);

            Func<IntVector, IntVector, IntVector> offsetPosition = (pos, posOffsets) => new IntVector() { X = pos.X + posOffsets.X, Y = pos.Y + posOffsets.Y, Z = pos.Z + posOffsets.Z, W = pos.W + posOffsets.W };

            var expandedCellCoordinates = new HashSet<IntVector>();
            foreach (var keyValPair in cellStates.AsEnumerable())
            {
                expandedCellCoordinates.Add(keyValPair.Key);
                if (keyValPair.Value == true)
                {
                    for (int offsetIndex = 0; offsetIndex < offsets.Count; ++offsetIndex)
                    {
                        expandedCellCoordinates.Add(offsetPosition(keyValPair.Key, offsets[offsetIndex]));
                        updatedCellStates[offsetPosition(keyValPair.Key, offsets[offsetIndex])] = false;
                    }
                }
            }

            foreach (var coordinates in expandedCellCoordinates)
            {
                int numNeighboursActive = 0;
                for (int offsetIndex = 0; offsetIndex < offsets.Count; ++offsetIndex)
                {
                    bool neighbourValue;
                    if (cellStates.TryGetValue(offsetPosition(coordinates, offsets[offsetIndex]), out neighbourValue))
                        numNeighboursActive += neighbourValue == true ? 1 : 0;
                }

                bool value;
                if (cellStates.TryGetValue(coordinates, out value) && value == true)
                    updatedCellStates[coordinates] = numNeighboursActive == 2 || numNeighboursActive == 3 ? true : false;
                else
                    updatedCellStates[coordinates] = numNeighboursActive == 3 ? true : false;
            }

            return updatedCellStates;
        }

        static void Main(string[] args)
        {
            var inputData = File.ReadAllLines("input.txt");

            var cellStates3D = new Dictionary<IntVector, bool>();

            Int64 x = 0, y = 0;
            inputData.ToList().ForEach(row =>
            {
                row.ToList().ForEach(cell => cellStates3D[new IntVector { X = x++, Y = y, Z = 0L, W = 0L }] = inputData[y][(int)x - 1] == '#');
                ++y;
                x = 0;
            });

            var cellStates4D = new Dictionary<IntVector, bool>(cellStates3D);

            var offsets3D = new List<IntVector>();
            foreach (var x2 in Enumerable.Range(-1, 3)) foreach (var y2 in Enumerable.Range(-1, 3)) foreach (var z in Enumerable.Range(-1, 3))
                        if ((x2 | y2 | z) != 0)
                            offsets3D.Add(new IntVector { X = x2, Y = y2, Z = z });


            var offsets4D = new List<IntVector>();
            foreach (var x2 in Enumerable.Range(-1, 3)) foreach (var y2 in Enumerable.Range(-1, 3)) foreach (var z in Enumerable.Range(-1, 3)) foreach (var w in Enumerable.Range(-1, 3))
                            if ((x2 | y2 | z | w) != 0)
                                offsets4D.Add(new IntVector { X = x2, Y = y2, Z = z, W = w });


            for (int cycleIndex = 0; cycleIndex < 6; ++cycleIndex)
            {
                cellStates3D = UpdateCells(cellStates3D, offsets3D);
                cellStates4D = UpdateCells(cellStates4D, offsets4D);
            }
            Console.WriteLine("Num active cells (3D): {0}", cellStates3D.Values.Where(value => value == true).Count());
            Console.WriteLine("Num active cells (4D): {0}", cellStates4D.Values.Where(value => value == true).Count());
        }
    }
}