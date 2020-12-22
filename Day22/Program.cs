using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day22
{
    class Program
    {
        static void Main(string[] args)
        {
            List<long>[] playerDecks = File.ReadAllText("input.txt").Split(Environment.NewLine + Environment.NewLine).Select(playerBlock => playerBlock.Split(Environment.NewLine).Skip(1).Select(intStr => long.Parse(intStr)).ToList()).ToArray();

            while (playerDecks.Where(playerDeck => playerDeck.Count > 0).Count() != 1)
            {
                var playedCards = playerDecks.Select((playerDeck, playerIndex) => KeyValuePair.Create(playerIndex, playerDeck.First())).ToList(); // select top cards


                playedCards.ForEach(playedCard => playerDecks[playedCard.Key].Remove(playedCard.Value));


                playedCards.Sort((lhsPair, rhsPair) => lhsPair.Value < rhsPair.Value ? 1 : -1); // find winner

                Debug.Assert(playedCards[0].Value != playedCards[1].Value, "Cards don't seem to be unique");

                playerDecks[playedCards.First().Key].AddRange(playedCards.Select(card => card.Value)); // put on bottom in order (already sorted)
            }

            var winningDeck = playerDecks.Where(playerDeck => playerDeck.Count > 0).First();
            Console.WriteLine("Winning score: {0}", winningDeck.Select((card, cardIndex) => card * (winningDeck.Count - cardIndex)).Aggregate((lhs, rhs) => lhs + rhs));
        }
    }
}
