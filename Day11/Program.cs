using System;
using System.IO;
using System.Linq;

namespace Day11
{
    class Program
    {
        static readonly int[,] adjacencyOffsets = new int[8, 2] { { -1, -1 }, { 0, -1 }, { -1, 0 }, { 1, 1 }, { 0, 1 }, { 1, 0 }, { -1, 1 }, { 1, -1 } };

        static string[] UpdateSeats(string[] rows, int occupiedLimit, bool directAdjacentOnly, bool freeSeatsStopPeopleWorryingAboutTheOccupiedSeatTheOtherSideOfTheFerry)
        {
            var newRows = new string[rows.Length];
            for (int y = 0; y < rows.Length; ++y)
            {
                var newRow = new char[rows[0].Length];
                for (int x = 0; x < rows[0].Length; ++x)
                {
                    int occupiedAdjacent = 0;
                    for (int offsetIndex = 0; offsetIndex < adjacencyOffsets.GetLength(0); ++offsetIndex)
                    {
                        int offsetX = adjacencyOffsets[offsetIndex, 0];
                        int offsetY = adjacencyOffsets[offsetIndex, 1];
                        while ((x + offsetX >= 0) && (x + offsetX < rows[0].Length) &&
                            (y + offsetY >= 0) && (y + offsetY < rows.Length) &&
                            (freeSeatsStopPeopleWorryingAboutTheOccupiedSeatTheOtherSideOfTheFerry == false || (rows[y + offsetY][x + offsetX] != 'L')))
                        {
                            if (rows[y + offsetY][x + offsetX] == '#')
                            {
                                occupiedAdjacent += 1;
                                break;
                            }
                            else if (directAdjacentOnly)
                                break;

                            offsetX += adjacencyOffsets[offsetIndex, 0];
                            offsetY += adjacencyOffsets[offsetIndex, 1];
                        }
                    }

                    newRow[x] = (rows[y][x] == 'L' && occupiedAdjacent == 0) ? '#' :
                        (rows[y][x] == '#' && occupiedAdjacent >= occupiedLimit) ? 'L' : rows[y][x];
                }
                newRows[y] = new string(newRow);
            }

            return newRows;
        }

        static int Solve(string[] rows, int occupiedLimit, bool directAdjacentOnly, bool freeSeatsStopPeopleWorryingAboutTheOccupiedSeatTheOtherSideOfTheFerry)
        {
            string[] sourceRows = rows;
            string[] updatedRows = sourceRows;
            do
            {
                sourceRows = updatedRows;
                updatedRows = UpdateSeats(sourceRows, occupiedLimit, directAdjacentOnly, freeSeatsStopPeopleWorryingAboutTheOccupiedSeatTheOtherSideOfTheFerry);
            } while (Enumerable.SequenceEqual(sourceRows, updatedRows) == false);

            int occupiedSeats = 0;
            updatedRows.ToList().ForEach(row => row.ToList().ForEach(seat => occupiedSeats += seat == '#' ? 1 : 0));
            return occupiedSeats;
        }

        static void Main(string[] args)
        {
            string[] rows = File.ReadAllLines("input.txt");

            Console.WriteLine("Occupied seats for part 1: {0}", Solve(rows, 4, true, false));
            Console.WriteLine("Occupied seats for part 2: {0}", Solve(rows, 5, false, true));
        }
    }
}
