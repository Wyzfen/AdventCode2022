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

        bool RunAutomata(List<Vector2> input, bool[,] map, Direction direction)
        {
            var output = new List<Vector2>();
            var targets = new Dictionary<Vector2, int>();
            var collisions = new HashSet<Vector2>();

            bool Neighbours(Vector2 target) => map[target.Y, target.X - 1] || map[target.Y - 1, target.X - 1] || map[target.Y + 1, target.X - 1] ||
                                               map[target.Y, target.X + 1] || map[target.Y - 1, target.X + 1] || map[target.Y + 1, target.X + 1] ||
                                               map[target.Y + 1, target.X] || map[target.Y - 1, target.X];

            for(int index = 0; index < input.Count; index++)
            {
                var elf = input[index];

                if (!Neighbours(elf)) continue;
                
                for (int i = 0; i < 4; i++)
                {
                    var dir = (Direction)(((int)direction + i) % 4);
                    Vector2 target = elf;
                    switch(dir)
                    {
                        case Direction.North:
                            if (map[elf.Y - 1, elf.X - 1] || map[elf.Y - 1, elf.X] || map[elf.Y - 1, elf.X + 1]) continue;
                            target = new Vector2(elf.X, elf.Y - 1);
                            break;
                        case Direction.South:
                            if (map[elf.Y + 1, elf.X - 1] || map[elf.Y + 1, elf.X] || map[elf.Y + 1, elf.X + 1]) continue;
                            target = new Vector2(elf.X, elf.Y + 1);
                            break;
                        case Direction.West:
                            if (map[elf.Y - 1, elf.X - 1] || map[elf.Y, elf.X - 1] || map[elf.Y + 1, elf.X - 1]) continue;
                            target = new Vector2(elf.X - 1, elf.Y);
                            break;
                        case Direction.East:
                            if (map[elf.Y - 1, elf.X + 1] || map[elf.Y, elf.X + 1] || map[elf.Y + 1, elf.X + 1]) continue;
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
                        targets[target] = index;                        
                    }

                    break;
                }
            }

            foreach(var (dest, index) in targets)
            {
                var source = input[index];
                input[index] = dest;

                map[source.Y, source.X] = false;
                map[dest.Y, dest.X] = true;
            }

            return targets.Count > 0 || collisions.Count > 0;
        }

        [TestMethod]
        public void Problem1()
        {
            var elves = ParseInput(values);
            var bounds = elves.Bounds();
            
            var map = new bool[bounds.Height * 3, bounds.Width * 3];
            
            elves = elves.Select(e => e + bounds.Size).ToList();
            elves.ForEach(e => map[e.Y, e.X] = true);

            Direction direction = Direction.North;
            int count = 0;
            while (RunAutomata(elves, map, direction++) && count++ < 9) ;

            bounds = elves.Bounds();

            int result = bounds.Width * bounds.Height - elves.Count;

            Assert.AreEqual(result, 3920);
        }

        [TestMethod]
        public void Problem2()
        {
            var elves = ParseInput(values);
            var bounds = elves.Bounds();

            var map = new bool[bounds.Height * 3, bounds.Width * 3];

            elves = elves.Select(e => e + bounds.Size).ToList();
            elves.ForEach(e => map[e.Y, e.X] = true);

            Direction direction = Direction.North;
            int result = 1;            
            while (RunAutomata(elves, map, direction++))
            {
                result++;
            }

            Assert.AreEqual(result, 889);
        }
    }
}
