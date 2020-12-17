using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Day17
{
    using CellDictionary = Dictionary<Tuple<Int64, Int64, Int64>, bool>;

    class Program
    {
        static CellDictionary UpdateCells(CellDictionary cellStates)
        {

            //int[,] offsets = new int[26, 3] {
            //        { -1, -1, -1 },
            //        { -1, -1, 0 },
            //        { -1, -1, 1 },
            //        { -1, 0, -1 },
            //        { -1, 0, 0 },
            //        { -1, 0, 1 },
            //        { -1, 1, -1 },
            //        { -1, 1, 0 },
            //        { -1, 1, 1 },
            //        { 0, -1, -1 },
            //        { 0, -1, 0 },
            //        { 0, -1, 1 },
            //        { 0, 0, -1 },
            //        { 0, 0, 1 },
            //        { 0, 1, -1 },
            //        { 0, 1, 0 },
            //        { 0, 1, 1 },
            //        { 1, -1, -1 },
            //        { 1, -1, 0 },
            //        { 1, -1, 1 },
            //        { 1, 0, -1 },
            //        { 1, 0, 0 },
            //        { 1, 0, 1 },
            //        { 1, 1, -1 },
            //        { 1, 1, 0 },
            //        { 1, 1, 1 },
            //    };

            var offsets = new List<Vector<Int64>>();
            foreach (var x in Enumerable.Range(-1, 2))
                foreach (var y in Enumerable.Range(-1, 2))
                    foreach (var z in Enumerable.Range(-1, 2))
                    {
                        offsets.Add(new Vector<Int64>(new Int64[3] { x, y, z}));
                    }



            var updatedCellStates = new CellDictionary(cellStates);

            var expandedCellCoordinates = new HashSet<Tuple<Int64, Int64, Int64>>();
            foreach (var keyValPair in cellStates.AsEnumerable())
            {
                expandedCellCoordinates.Add(keyValPair.Key);
                if (keyValPair.Value == true)
                {
                    for (int offsetIndex = 0; offsetIndex < offsets.GetLength(0); ++offsetIndex)
                    {
                        expandedCellCoordinates.Add(Tuple.Create(keyValPair.Key.Item1 + offsets[offsetIndex, 0], keyValPair.Key.Item2 + offsets[offsetIndex, 1], keyValPair.Key.Item3 + offsets[offsetIndex, 2]));
                        updatedCellStates[Tuple.Create(keyValPair.Key.Item1 + offsets[offsetIndex, 0], keyValPair.Key.Item2 + offsets[offsetIndex, 1], keyValPair.Key.Item3 + offsets[offsetIndex, 2])] = false;
                    }
                }
            }

            foreach (var coordinates in expandedCellCoordinates)
            {
                int numNeighboursActive = 0;


                var test = Tuple.Create(2L, 0L, 0L);
                if (Tuple.Create(coordinates.Item1, coordinates.Item2, coordinates.Item3).Equals(test))
                {
                    Console.Write("");
                }

                for (int offsetIndex = 0; offsetIndex < offsets.GetLength(0); ++offsetIndex)
                {
                    bool neighbourValue;
                    if (cellStates.TryGetValue(Tuple.Create(coordinates.Item1 + offsets[offsetIndex, 0], coordinates.Item2 + offsets[offsetIndex, 1], coordinates.Item3 + offsets[offsetIndex, 2]), out neighbourValue))
                    {
                        numNeighboursActive += neighbourValue == true ? 1 : 0;
                    }
                }



                bool value;
                if (cellStates.TryGetValue(Tuple.Create(coordinates.Item1, coordinates.Item2, coordinates.Item3), out value) && value == true)
                {
                    updatedCellStates[Tuple.Create(coordinates.Item1, coordinates.Item2, coordinates.Item3)] = numNeighboursActive == 2 || numNeighboursActive == 3 ? true : false;
                }
                else
                {
                    updatedCellStates[Tuple.Create(coordinates.Item1, coordinates.Item2, coordinates.Item3)] = numNeighboursActive == 3 ? true : false;
                }
            }

            return updatedCellStates;
        }

        static void Main(string[] args)
        {
            var inputData = File.ReadAllLines("input.txt");

            var cellStates = new Dictionary<Tuple<Int64, Int64, Int64>, bool>();

            Int64 x = 0, y = 0;
            inputData.ToList().ForEach(row =>
            {
                row.ToList().ForEach(cell => cellStates[Tuple.Create(x++, y, 0L)] = inputData[y][(int)x - 1] == '#');
                ++y;
                x = 0;
            });

            for (int cycleIndex = 0; cycleIndex < 6; ++cycleIndex)
            {
                var newCells = UpdateCells(cellStates);
                Console.WriteLine("Num active cells: {0}", newCells.Where(value => value.Value == true && value.Key.Item3 == 0).Count());
                cellStates = newCells;
            }

            Console.WriteLine("Num active cells: {0}", cellStates.Values.Where(value => value == true).Count());
        }
    }
}