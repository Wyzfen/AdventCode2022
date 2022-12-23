using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day23
    {
        readonly static string[] test = new[]
        {
            "..............",
            "..............",
            ".......#......",
            ".....###.#....",
            "...#...#.#....",
            "....#...##....",
            "...#.###......",
            "...##.#.##....",
            "....#..#......",
            "..............",
            "..............",
            ".............."
        };
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        List<Vector2> ParseInput(IEnumerable<string> input) => input.SelectMany((s, y) => 
                            s.Select((c, x) => (c, x))
                             .Where(p => p.c == '#')
                             .Select(p => new Vector2(p.x, y))
                            ).ToList();

        enum Direction : int
        {
            North, South, West, East
        }

        string[] Map(IEnumerable<Vector2> input)
        {
            int minX = input.Min(e => e.X);
            int maxX = input.Max(e => e.X);
            int minY = input.Min(e => e.Y);
            int maxY = input.Max(e => e.Y);

            char[][] output = Enumerable.Range(0, maxY - minY + 1).Select(_ => Enumerable.Repeat<char>('.', maxX - minX + 1).ToArray()).ToArray();

            foreach(var (x, y) in input)
            {
                output[y - minY][x - minX] = '#';
            }

            return output.Select(s => new String(s)).ToArray();
        }

        bool RunAutomata(List<Vector2> input, Direction direction)
        {
            var output = new List<Vector2>();
            var targets = new Dictionary<Vector2, Vector2>();
            var collisions = new HashSet<Vector2>();

            int Neighbours(Vector2 target) => input.Where(i => Math.Abs(i.X - target.X) <= 1 && Math.Abs(i.Y - target.Y) <= 1).Count() - 1; // -1 to remove self
            
            foreach(var elf in input)
            {
                if (Neighbours(elf) == 0) continue;
                
                for (int i = 0; i < 4; i++)
                {
                    var dir = (Direction)(((int)direction + i) % 4);
                    Vector2 target = elf;
                    switch(dir)
                    {
                        case Direction.North:
                            if (input.Any(i => Math.Abs(i.X - elf.X) <= 1 && i.Y == elf.Y - 1)) continue;
                            target = new Vector2(elf.X, elf.Y - 1);
                            break;
                        case Direction.South:
                            if (input.Any(i => Math.Abs(i.X - elf.X) <= 1 && i.Y == elf.Y + 1)) continue;
                            target = new Vector2(elf.X, elf.Y + 1);
                            break;
                        case Direction.West:
                            if (input.Any(i => Math.Abs(i.Y - elf.Y) <= 1 && i.X == elf.X - 1)) continue;
                            target = new Vector2(elf.X - 1, elf.Y);
                            break;
                        case Direction.East:
                            if (input.Any(i => Math.Abs(i.Y - elf.Y) <= 1 && i.X == elf.X + 1)) continue;
                            target = new Vector2(elf.X + 1, elf.Y);
                            break;
                    }

                    if(collisions.Contains(target))
                    {
                        // Do nothing
                    }
                    else if(targets.ContainsKey(target))
                    {
                        collisions.Add(target);
                        targets.Remove(target);
                    }
                    else
                    {
                        targets[target] = elf;                        
                    }

                    break;
                }
            }

            foreach(var (dest, source) in targets)
            {
                input.Remove(source);
                input.Add(dest);
            }

            return targets.Count > 0 || collisions.Count > 0;
        }

        [TestMethod]
        public void Problem1()
        {
            var elves = ParseInput(values);

            Direction direction = Direction.North;
            int count = 0;
            while (RunAutomata(elves, direction++) && count++ < 9) ;
            //{
            //    Console.WriteLine($"Pass #{count}");
            //    foreach (var line in Map(elves))
            //    {
            //        Console.WriteLine(line);
            //    }
            //    Console.WriteLine();
            //}


            int minX = elves.Min(e => e.X);
            int maxX = elves.Max(e => e.X);
            int minY = elves.Min(e => e.Y);
            int maxY = elves.Max(e => e.Y);

            int result = (maxX - minX + 1) * (maxY - minY + 1) - elves.Count;

            Assert.AreEqual(result, 3920);
        }

        [TestMethod]
        public void Problem2()
        {
            var elves = ParseInput(values);

            Direction direction = Direction.North;
            int count = 1;
            while (RunAutomata(elves, direction++))
            {
                count++;
            }
            
            int result = count;

            Assert.AreEqual(result, 889);
        }
    }
}
