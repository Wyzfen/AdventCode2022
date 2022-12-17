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

        long FallRocks(string gusts, long count)
        {
            List<int> levels = new() { floor };
            
            int currentRock = 0;
            int gustIndex = 0;
            int height = 0;

            for (long i = 0; i < count; i++)
            {
                int[] piece = (int[])pieces[currentRock].Clone();
                levels.InsertRange(0, Enumerable.Repeat(wall, 3 + piece.Length));

                int offset = 0;

                do
                {
                    switch (gusts[gustIndex++ % gusts.Length])
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

                    offset++;
                } while (piece.Zip(levels.Skip(offset)).All(p => (p.First & p.Second) == 0));

                for (int k = 0; k < piece.Length; k++)
                {
                    levels[offset + k - 1] |= piece[k];
                }

                levels.RemoveAll(x => x == wall);
                currentRock = (currentRock + 1) % 5;
            }


            return levels.Count - 1 + height;
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
            long result = FallRocks(test, 1000000000000);


            Assert.AreEqual(result, 4267809);
        }
    }
}
