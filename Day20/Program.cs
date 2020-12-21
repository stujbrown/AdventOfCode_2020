using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Day20
{
    class Program
    {
        class Tile
        {
            public long Id;
            public string[] Image;
            public int Orientation = 0;

            public string[] Edges()
            {
                return new string[4]
                {
                    new string( Enumerable.Range(0, Image.Length).Select(index => GetPixel( index, 0 ) ).ToArray() ),
                    new string( Enumerable.Range(0, Image.Length).Select(index => GetPixel( Image.Length-1, index) ).ToArray() ),
                    new string( Enumerable.Range(0, Image.Length).Select(index => GetPixel( index, Image.Length-1 ) ).ToArray() ),
                    new string( Enumerable.Range(0, Image.Length).Select(index => GetPixel( 0, index ) ).ToArray() ),
                };
            }

            public char GetPixel(int x, int y)
            {
                switch (Orientation)
                {
                    case 0: return Image[x][y];
                    case 1: return Image[(Image.Length - 1) - x][y];
                    case 2: return Image[y][x];
                    case 3: return Image[y][(Image.Length - 1) - x];
                    case 4: return Image[(Image.Length - 1) - y][x];
                    case 5: return Image[x][(Image.Length - 1) - y];
                    case 6: return Image[(Image.Length - 1) - y][(Image.Length - 1) - x];
                    case 7: return Image[(Image.Length - 1) - x][(Image.Length - 1) - y];
                    default:
                        return '0';
                }
            }
        };

        struct TileMatch
        {
            public Tile Tile;
            public int MatchedEdge; // 0=N, 1=E, etc
        };


        static List<TileMatch> Match(List<Tile> tiles, Tile matchTile)
        {
            var matches = new List<TileMatch>();
            foreach (var otherTile in tiles)
            {
                Func<int> checkOtherTile = () =>
                {
                    for (int otherOrientation = 0; otherOrientation < 8; ++otherOrientation)
                    {
                        if (otherTile != matchTile)
                        {
                            otherTile.Orientation = otherOrientation; // bodge it til it works

                            var edges = matchTile.Edges();
                            var otherEdges = otherTile.Edges();
                            for (int edgeIndex = 0; edgeIndex < 4; ++edgeIndex)
                            {
                                if (edges[edgeIndex].Equals(otherEdges[(edgeIndex + 2) % 4]))
                                {
                                    return (edgeIndex + 2) % 4;
                                }
                            }
                        }
                    }

                    return -1;
                };

                int dir = checkOtherTile();
                if (dir != -1)
                    matches.Add(new TileMatch() { Tile = otherTile, MatchedEdge = dir });


            }


            return matches;
        }

        static void Main(string[] args)
        {
            var tiles = new List<Tile>();

            var blocks = File.ReadAllText("input.txt").Split(Environment.NewLine + Environment.NewLine).Select(block => block.Split(Environment.NewLine)).ToArray();
            Array.ForEach(blocks, block =>
            {
                var image = new string[block.Length - 1];
                new ArraySegment<string>(block, 1, image.Length).CopyTo(image);
                tiles.Add(new Tile() { Id = long.Parse(block[0].Split(' ')[1].Split(':')[0]), Image = image, Orientation = 0 });
            });

            var categorisedTiles = new List<Tile>[3] { new List<Tile>(), new List<Tile>(), new List<Tile>() }; //0 = corners, 1 = edges, 3 centres
            foreach (var tile in tiles)
            {
                int numEdgeMatches = Match(tiles, tile).Count;


                if (numEdgeMatches == 2)
                    categorisedTiles[0].Add(tile);
                else if (numEdgeMatches == 3)
                    categorisedTiles[1].Add(tile);
                else
                    categorisedTiles[2].Add(tile);
            }

            Console.WriteLine("Corners: {0}", categorisedTiles[0].Select(tile => tile.Id).Aggregate((lhs, rhs) => lhs * rhs));

            // Part 2
            var tileMap = new Tile[(int)Math.Sqrt(tiles.Count), (int)Math.Sqrt(tiles.Count)];


            var remainingCorners = new List<Tile>(categorisedTiles[0]);
            var remainingEdges = new List<Tile>(categorisedTiles[1]);
            var remainingCentres = new List<Tile>(categorisedTiles[2]);

            // Get first tile oriented right
            categorisedTiles[0][0].Orientation = 0;
            var firstMatches = Match(tiles, categorisedTiles[0][0]);
            var orientations = firstMatches.Select(match => match.MatchedEdge);
            while (orientations.Contains(3) == false || orientations.Contains(0) == false)
            {
                categorisedTiles[0][0].Orientation += 1;
                firstMatches = Match(tiles, categorisedTiles[0][0]);
                orientations = firstMatches.Select(match => match.MatchedEdge);
            }

            tileMap[0, 0] = categorisedTiles[0][0];
            remainingCorners.Remove(categorisedTiles[0][0]);

            Action<bool, int, int> fillEdge = (bool verticalAxis, int expectedOrientation, int axisIndex) =>
            {
                for (int i = 1; i < tileMap.GetLength(1); ++i)
                {
                    int x = i;
                    int y = axisIndex;
                    if (verticalAxis)
                        (x, y) = (y, x);

                    var list = remainingEdges;
                    if (i == tileMap.GetLength(1) - 1)
                        list = remainingCorners;

                    if (tileMap[x, y] == null)
                    {
                        var matches = Match(list, tileMap[verticalAxis ? x : x - 1, verticalAxis ? y - 1 : y]).Where(match => match.MatchedEdge == expectedOrientation);
                        if (matches.Count() != 1) throw new Exception();
                        tileMap[x, y] = matches.First().Tile;
                        list.Remove(matches.First().Tile);
                    }

                }

            };

            fillEdge(false, 3, 0);
            fillEdge(true, 0, 0);
            fillEdge(false, 3, tileMap.GetLength(1) - 1);
            fillEdge(true, 0, tileMap.GetLength(0) - 1);

            for (int y = 0; y < tileMap.GetLength(1); ++y)
            {
                for (int x = 0; x < tileMap.GetLength(0); ++x)
                {
                    if (tileMap[x, y] == null)
                    {
                        var matchesX = Match(remainingCentres, tileMap[x - 1, y]).Where(match => match.MatchedEdge == 3).Select(match => match.Tile);
                        var matchesY = Match(remainingCentres, tileMap[x, y - 1]).Where(match => match.MatchedEdge == 0).Select(match => match.Tile);
                        var commonMatches = matchesX.Intersect(matchesY);
                        if (commonMatches.Intersect(matchesY).Count() != 1) throw new Exception();
                        tileMap[x, y] = commonMatches.First();
                        remainingCentres.Remove(commonMatches.First());
                    }
                }
            }

            var fullImage = Merge(tileMap);
            var fullTile = new Tile() { Image = fullImage };

            var seaMonster = new string[3] {    "                  #",
                                                "#    ##    ##    ###",
                                                " #  #  #  #  #  #" };


            int totalWaves = 0;
            Array.ForEach(fullTile.Image, str => totalWaves += str.Where(c => c == '#').Count());

            for (int orientation = 0; orientation < 8; ++orientation)
            {
                fullTile.Orientation = orientation;

                int roughness = totalWaves;
                for (int y = 0; y < fullTile.Image.Length - 2; ++y)
                {
                    for (int x = 0; x < fullTile.Image[0].Length - seaMonster.Max(str => str.Length); ++x)
                    {
                        var line = new StringBuilder();
                        Array.ForEach(Enumerable.Range(0, seaMonster[0].Length).ToArray(), offset => line.Append(fullTile.GetPixel(x + offset, y)));
                        if (Regex.Match(line.ToString(), "^" + seaMonster[0].Replace(' ', '.')).Success)
                        {
                            line.Clear();
                            Array.ForEach(Enumerable.Range(0, seaMonster[1].Length).ToArray(), offset => line.Append(fullTile.GetPixel(x + offset, y + 1)));
                            if (Regex.Match(line.ToString(), "^" + seaMonster[1].Replace(' ', '.')).Success)
                            {

                                line.Clear();
                                Array.ForEach(Enumerable.Range(0, seaMonster[2].Length).ToArray(), offset => line.Append(fullTile.GetPixel(x + offset, y + 2)));
                                if (Regex.Match(line.ToString(), "^" + seaMonster[2].Replace(' ', '.')).Success)
                                {
                                    roughness -= 15;
                                }
                            }
                        }
                    }
                }

                if (roughness != 0 && roughness != totalWaves)
                    Console.WriteLine("roughness: {0}", roughness);
            }
        }

        static string[] Merge(Tile[,] map)
        {
            int tileDimension = map[0, 0].Image.Length;
            int dimension = map.GetLength(0) * tileDimension; // it's all square anyway...
            var image = new string[map.GetLength(0) * (tileDimension - 2)];
            for (int y = 0, mergedY = 0; y < dimension; ++y)
            {
                if (y % tileDimension == 0 || y % tileDimension == tileDimension - 1) continue;

                var newRow = new StringBuilder();
                for (int x = 0; x < dimension; ++x)
                {
                    if (x % tileDimension == 0 || x % tileDimension == tileDimension - 1) continue;

                    if (map[x / tileDimension, y / tileDimension] != null)
                    {
                        newRow.Append(map[x / tileDimension, y / tileDimension].GetPixel(x % tileDimension, y % tileDimension));
                    }
                }
                image[mergedY] = newRow.ToString();
                mergedY++;
            }

            return image;
        }
    }
}
