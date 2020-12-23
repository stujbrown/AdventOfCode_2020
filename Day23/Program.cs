using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Day23
{
    class Program
    {
        static int IndexCup(int indexToWrap, int cupsInList)
        {
            return (indexToWrap > 0) ? (int)((long)indexToWrap % (long)cupsInList) : (int)(((long)cupsInList - Math.Abs((long)indexToWrap)) % (long)cupsInList);
        }

        static void DoCrabShuffle(List<long> cups, int numMoves)
        {
            long lowest = cups.Min();
            long highest = cups.Max();


            Func<long, long> subtractCupValue = (long startCupValue) => ((startCupValue - 1) < lowest) ? startCupValue = highest : (startCupValue - 1);

            var currentCup = cups[0];
            for (long move = 0; move < numMoves; ++move)
            {
                var currentIndex = cups.IndexOf(currentCup);

                var taken = new List<long>();
                foreach (var takeCount in Enumerable.Range(0, 3))
                    taken.Add(cups[IndexCup((currentIndex + 1) + takeCount, cups.Count)]);

                taken.ForEach(cup => cups.Remove(cup));

                // Debug.Assert(cups.Contains(cupValue) && cups[currentCupIndex] == cupValue); // ensure cup/index is preserved

                var destinationCup = subtractCupValue(currentCup);
                while (taken.Contains(destinationCup))
                    destinationCup = subtractCupValue(destinationCup);

                int destinationIndex = cups.IndexOf(destinationCup) + 1;
                cups.InsertRange(destinationIndex, taken);

                currentCup = cups[IndexCup(cups.IndexOf(currentCup) + 1, cups.Count)];

                //Array.ForEach(cups.ToArray(), cup => Console.Write(cup));
                //Console.WriteLine("");
            }
        }


        static void Main(string[] args)
        {
            var originalCups = Array.ConvertAll(File.ReadAllLines("input.txt")[0].ToCharArray(), c => long.Parse(c.ToString())).ToList();
            var cups = new List<long>(originalCups);

            DoCrabShuffle(cups, 100);

            var str = new StringBuilder();
            int iterationIndex = IndexCup(cups.IndexOf(1) + 1, cups.Count);
            while (cups[iterationIndex] != 1)
            {
                str.Append(cups[IndexCup(iterationIndex, cups.Count)]);
                iterationIndex = IndexCup(++iterationIndex, cups.Count);
            }
            Console.WriteLine("Cup labels = {0}", str.ToString());

            var lotsOfCups = new List<long>(originalCups);

            long nextCup = cups.Max() + 1;
            for (int totalCups = lotsOfCups.Count; totalCups < 1000000; ++totalCups)
            {
                lotsOfCups.Add(nextCup++);
            }

            DoCrabShuffle(lotsOfCups, 10000000);

            iterationIndex = lotsOfCups.IndexOf(1);
            var label1 = cups[IndexCup( iterationIndex + 1, cups.Count)];
            var label2 = cups[IndexCup(iterationIndex + 2, cups.Count)];

            Console.WriteLine("Cup labels for long game = {0}", label1 * label2);
        }
    }
}
