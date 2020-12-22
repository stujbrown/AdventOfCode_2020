using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Day22
{
    class Program
    {
        static string SerializeState(List<long>[] decks)
        {
            StringBuilder str = new StringBuilder();
            Array.ForEach(decks, deck => { deck.ForEach(card => { str.Append(card); str.Append(','); }); str.Append(':'); });
            return str.ToString();
        }

        static int PlayGame(List<long>[] playerDecks, bool recursiveCombat)
        {
            var stateRecord = new HashSet<string>();

            while (playerDecks.Where(playerDeck => playerDeck.Count > 0).Count() != 1)
            {
                var startState = SerializeState(playerDecks);
                if (recursiveCombat && stateRecord.Contains(startState))
                    return 0;

                var playedCards = playerDecks.Select((playerDeck, playerIndex) => KeyValuePair.Create(playerIndex, playerDeck.First())).ToList(); // select top cards
                playedCards.ForEach(playedCard => playerDecks[playedCard.Key].Remove(playedCard.Value)); // remove played from decks

                int winner = -1;
                if (recursiveCombat && playerDecks.Where((playerDeck, playerIndex) => playerDeck.Count >= playedCards[playerIndex].Value).Count() == playerDecks.Length)
                {
                    winner = PlayGame(playerDecks.Select((playerDeck, playerIndex) => new List<long>(playerDeck.Take((int)playedCards[playerIndex].Value))).ToArray(), true);
                    playedCards.Sort((lhsPair, rhsPair) => lhsPair.Key < winner ? 1 : -1); // sort according to winner
                }
                else
                {
                    playedCards.Sort((lhsPair, rhsPair) => lhsPair.Value < rhsPair.Value ? 1 : -1); // sort according to value to determine winner
                    winner = playedCards.First().Key;
                }

                playerDecks[winner].AddRange(playedCards.Select(card => card.Value)); // put on bottom in winner order (already sorted)
                stateRecord.Add(startState);
            }

            return playerDecks.Select((playerDeck, playerIndex) => KeyValuePair.Create(playerIndex, playerDeck)).Where(playerDeck => playerDeck.Value.Count > 0).First().Key;
        }

        static void Main(string[] args)
        {
            List<long>[] originalPlayerDecks = File.ReadAllText("input.txt").Split(Environment.NewLine + Environment.NewLine).Select(playerBlock => playerBlock.Split(Environment.NewLine).Skip(1).Select(intStr => long.Parse(intStr)).ToList()).ToArray();

            List<long>[] playerDecks = originalPlayerDecks.Select(deck => new List<long>(deck)).ToArray();
            int winner = PlayGame(playerDecks, false);
            Console.WriteLine("Winning score: {0}", playerDecks[winner].Select((card, cardIndex) => card * (playerDecks[winner].Count - cardIndex)).Aggregate((lhs, rhs) => lhs + rhs));

            playerDecks = originalPlayerDecks.Select(deck => new List<long>(deck)).ToArray();
            winner = PlayGame(playerDecks, true);
            Console.WriteLine("Winning score for recursive combat: {0}", playerDecks[winner].Select((card, cardIndex) => card * (playerDecks[winner].Count - cardIndex)).Aggregate((lhs, rhs) => lhs + rhs));
        }
    }
}
