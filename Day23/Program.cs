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
        static LinkedListNode<long> NextCup(LinkedListNode<long> current, LinkedList<long> list) => current.Next == null ? list.First : current.Next;

        static void DoCrabShuffle(LinkedList<long> cups, int numMoves)
        {
            long lowest = cups.Min();
            long highest = cups.Max();

            var nodeDictionary = new Dictionary<long, LinkedListNode<long>>(); // removed nodes are re-added to the list so this is stable

            var nextAdd = cups.First;
            while (nextAdd != null)
            {
                nodeDictionary.Add(nextAdd.Value, nextAdd);
                nextAdd = nextAdd.Next;
            }

            Func<long, long> subtractCupValue = (long startCupValue) => ((startCupValue - 1) < lowest) ? startCupValue = highest : (startCupValue - 1);

            var currentCupNode = cups.First;
            for (long move = 0; move < numMoves; ++move)
            {
                var currentCup = currentCupNode.Value;

                var taken = new LinkedListNode<long>[3];
                var nextTake = NextCup(currentCupNode, cups);
                foreach (var takeCount in Enumerable.Range(0, 3))
                {
                    taken[takeCount] = nextTake;
                    nextTake = NextCup(nextTake, cups);
                }

                Array.ForEach(taken, takenNode => cups.Remove(takenNode));

                var destinationCup = subtractCupValue(currentCup);
                while (taken.Select(cup => cup.Value).Contains(destinationCup))
                    destinationCup = subtractCupValue(destinationCup);

                var destinationNode = nodeDictionary[destinationCup];
                Array.ForEach(taken, takenNode => cups.AddAfter(destinationNode, destinationNode = takenNode));
                currentCupNode = NextCup(currentCupNode, cups);
            }
        }

        static void Main(string[] args)
        {
            var originalCups = Array.ConvertAll(File.ReadAllLines("input.txt")[0].ToCharArray(), c => long.Parse(c.ToString())).ToList();
            var cups = new LinkedList<long>(originalCups);

            DoCrabShuffle(cups, 100);

            var str = new StringBuilder();
            var next = NextCup(cups.Find(1), cups);
            while (next.Value != 1)
            {
                str.Append(next.Value);
                next = NextCup(next, cups);
            }
            Console.WriteLine("Cup labels = {0}", str.ToString());

            var lotsOfCups = new LinkedList<long>(originalCups);
            long nextCup = lotsOfCups.Max() + 1;
            for (int totalCups = lotsOfCups.Count; totalCups < 1000000; ++totalCups)
                lotsOfCups.AddLast(nextCup++);

            DoCrabShuffle(lotsOfCups, 10000000);

            var label1 = NextCup(lotsOfCups.Find(1), lotsOfCups);
            Console.WriteLine("Cup labels for long game = {0}", label1.Value * NextCup(label1, lotsOfCups).Value);
        }
    }
}
