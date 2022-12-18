using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day17
    {
        readonly static string test = ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>";
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly string values = Utils.FromFile<string>($"{day}.txt").First();

        int[][] pieces = new[]
        {
            new int[] { 0b00111100 },
            new int[] { 0b00010000,
                        0b00111000,
                        0b00010000 },
            new int[] { 0b00001000,
                        0b00001000,
                        0b00111000 },
            new int[] { 0b00100000,
                        0b00100000,
                        0b00100000,
                        0b00100000 },
            new int[] { 0b00110000,
                        0b00110000 }
        };

        int floor = 0b111111111;
        int wall = 0b100000001;

        record State(int rock, int gust, int offset);

        List<State> history = new();
        
        long FallRocks(string gusts, long count, int gustIndex = 0, int currentRock = 0, int[] start = null)
        {
            List<int> levels = new() { floor };
            if(start != null)
            {
                levels.InsertRange(0, start);
            }

            for (long i = 0; i < count; i++)
            {
                int[] piece = (int[])pieces[currentRock].Clone();
                levels.InsertRange(0, Enumerable.Repeat(wall, 3 + piece.Length));

                int offset = 0;

                do
                {
                    switch (gusts[gustIndex])
                    {
                        case '<':
                            if (piece.Zip(levels.Skip(offset)).All(p => ((p.First << 1) & p.Second) == 0))
                            {
                                for(int p = 0; p < piece.Length; p++) { piece[p] <<= 1; }
                            }
                            break;
                        case '>':
                            if (piece.Zip(levels.Skip(offset)).All(p => ((p.First >> 1) & p.Second) == 0))
                            {
                                for (int p = 0; p < piece.Length; p++) { piece[p] >>= 1; }
                            }
                            break;
                    }

                    gustIndex = (gustIndex + 1) % gusts.Length;
                    offset++;
                } while (piece.Zip(levels.Skip(offset)).All(p => (p.First & p.Second) == 0));

                for (int k = 0; k < piece.Length; k++)
                {
                    levels[offset + k - 1] |= piece[k];
                }

                var state = new State(currentRock, gustIndex, offset);
                if(history.Contains(state))
                {
                    Console.WriteLine($"State ({currentRock}, {gustIndex}, {offset}) seen before at {history.IndexOf(state)}. CurrentIndex = {i}");
                }
                history.Add(state);

                levels.RemoveAll(x => x == wall);
                currentRock = (currentRock + 1) % 5;
            }


            return levels.Count - 1;
        }

        [TestMethod]
        public void Problem1()
        {
            long result = FallRocks(values, 2022);

            Assert.AreEqual(result, 3181);
        }

        [TestMethod]
        public void Problem2()
        {
            // Repeats after 1913 rows.
            // 1914th row is same as 188th row
            // height difference is 2709
            long result = FallRocks(values, 1913);
            
            // (10^12 - 1913) / (1913-188) = 579710143 copies
            result += 2709 * (long)579710143;
            
            // remainder is 1412. use previous gustIndex, rockIndex, and last 3 levels.
            // Sum is -3, to discard extra three levels added
            result += FallRocks(values, 1412, 1127, 3, new[] { 289, 297, 509 }) - 3; // will repeat from 188

            Assert.AreEqual(result, 1570434782634);
        }
    }
}
