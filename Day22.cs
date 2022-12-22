using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventCode2022
{
    [TestClass]
    public class Day22
    {
        readonly static IEnumerable<string> test = new[]
                    {
                        "        ...#    ",
                        "        .#..    ",
                        "        #...    ",
                        "        ....    ",
                        "...#.......#    ",
                        "........#...    ",
                        "..#....#....    ",
                        "..........#.    ",
                        "        ...#....",
                        "        .....#..",
                        "        .#......",
                        "        ......#.",
                        "",
                        "10R5L5R10L4R5L5",
                    };
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");


        (string[] map, string instructions) ParseInput(IEnumerable<string> values)
        {
            var mapValues = values.TakeWhile(v => !string.IsNullOrWhiteSpace(v));
            var instrValues = values.Last();

            var length = mapValues.Max(v => v.Length);
            var map = mapValues.Select(v => v.PadRight(length, ' ')).ToArray();

            return (map, instrValues);
        }

        enum Facing : int
        {
            Right = 0,
            Down = 1,
            Left = 2,
            Up = 3
        }

        (Vector2 location, Facing facing) TraverseMap(string[] map, Vector2 start, Facing facing, string instructions, FindNext findNext)
        {
            var x = start.X;
            var y = start.Y;

            int index = 0;
            while (index < instructions.Length)
            {
                switch (instructions[index])
                {
                    case char c when char.IsDigit(c):
                        var valueString = new String(instructions.Skip(index).TakeWhile(char.IsDigit).ToArray());
                        int distance = int.Parse(valueString);
                        index += valueString.Length - 1; // -1 so can use index++ below

                        int testx = x;
                        int testy = y;
                        var testFacing = facing;

                        while (distance > 0)
                        {
                            findNext(ref testFacing, ref testx, ref testy);

                            if (map[testy][testx] == '#')
                            {
                                break;
                            }

                            x = testx;
                            y = testy;
                            facing = testFacing;

                            distance--;
                        }
                        break;
                    case 'R':
                        facing = (Facing)(((int)facing + 1) % 4);
                        break;
                    case 'L':
                        facing = (Facing)(((int)facing - 1 + 4) % 4);
                        break;
                }
                index++;
            }

            return (new Vector2(x, y), facing);
        }

        delegate void FindNext(ref Facing facing, ref int testx, ref int testy);

        [TestMethod]
        public void Problem1()
        {
            var (map, instructions) = ParseInput(values);

            var start = new Vector2(map[0].FirstIndex(c => c == '.'), 0);

            void process(ref Facing facing, ref int testx, ref int testy)
            {
                do
                {
                    switch (facing)
                    {
                        case Facing.Left:
                            testx = (testx - 1 + map[0].Length) % map[0].Length;
                            break;
                        case Facing.Right:
                            testx = (testx + 1) % map[0].Length;
                            break;
                        case Facing.Up:
                            testy = (testy - 1 + map.Length) % map.Length;
                            break;
                        case Facing.Down:
                            testy = (testy + 1) % map.Length;
                            break;
                    }
                } while (map[testy][testx] == ' ');
            }

            var (location, facing) = TraverseMap(map, start, Facing.Right, instructions, process);

            int result = 1000 * (location.Y + 1) + 4 * (location.X + 1) + (int)facing;

            Assert.AreEqual(result, 57350);
        }


        [TestMethod]
        public void Problem2()
        {
            var (map, instructions) = ParseInput(values);

            var start = new Vector2(map[0].FirstIndex(c => c == '.'), 0);

            void process(ref Facing facing, ref int testx, ref int testy)
            {
                switch (facing, testx, testy)
                {
                    case (Facing.Left, 50, >= 0 and < 50):
                        testx = 0;
                        testy = 149 - testy;
                        facing = Facing.Right;
                        break;
                    case (Facing.Left, 0, >= 100 and < 150):
                        testx = 50;
                        testy = 149 - testy;
                        facing = Facing.Right;
                        break;

                    case (Facing.Up, >= 50 and < 100, 0):
                        testy = testx + 100;
                        testx = 0;
                        facing = Facing.Right;
                        break;
                    case (Facing.Left, 0, >= 150 and < 200):
                        testx = testy - 100;
                        testy = 0;
                        facing = Facing.Down;
                        break;

                    case (Facing.Left, 50, >= 50 and < 100):
                        testx = testy - 50;
                        testy = 100;
                        facing = Facing.Down;
                        break;
                    case (Facing.Up, >= 0 and < 50, 100):
                        testy = testx + 50;
                        testx = 50;
                        facing = Facing.Right;
                        break;

                    case (Facing.Down, >= 100 and < 150, 49):
                        testy = testx - 50;
                        testx = 99;
                        facing = Facing.Left;
                        break;
                    case (Facing.Right, 99, >= 50 and < 100):
                        testx = testy + 50;
                        testy = 49;
                        facing = Facing.Up;
                        break;

                    case (Facing.Up, >= 100 and < 150, 0):
                        testx = testx - 100;
                        testy = 199;
                        facing = Facing.Up;
                        break;
                    case (Facing.Down, >= 0 and < 50, 199):
                        testx = testx + 100;
                        testy = 0;
                        facing = Facing.Down;
                        break;

                    case (Facing.Right, 149, >= 0 and < 50):
                        testy = 149 - testy;
                        testx = 99;
                        facing = Facing.Left;
                        break;
                    case (Facing.Right, 99, >= 100 and < 150):
                        testy = 149 - testy;
                        testx = 149;
                        facing = Facing.Left;
                        break;

                    case (Facing.Down, >= 50 and < 100, 149):
                        testy = testx + 100;
                        testx = 49;
                        facing = Facing.Left;
                        break;
                    case (Facing.Right, 49, >= 150 and < 200):
                        testx = testy - 100;
                        testy = 149;
                        facing = Facing.Up;
                        break;

                    case (Facing.Left, _, _):
                        testx--;
                        break;
                    case (Facing.Right, _, _):
                        testx++;
                        break;
                    case (Facing.Up, _, _):
                        testy--;
                        break;
                    case (Facing.Down, _, _):
                        testy++;
                        break;
                }
            }

            var (location, facing) = TraverseMap(map, start, Facing.Right, instructions, process);

            int result = 1000 * (location.Y + 1) + 4 * (location.X + 1) + (int)facing;

            Assert.AreEqual(result, 104385);
        }
    }
}
