using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day15
    {
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        [DebuggerDisplay("{Start} : {End}")]
        struct Range // Can't use Record, as need to validate on creation
        {
            public int Start { get; init; }
            public int End { get; init; }

            public Range(int start, int end)
            {
                Start = Math.Min(start, end);
                End = Math.Max(start, end);
            }

            public bool Overlaps(Range other) => other.Start <= this.End && other.End >= this.Start;

            public Range Combine(Range other) => new(Math.Min(this.Start, other.Start), Math.Max(this.End, other.End));

            public static Range Combine(Range a, Range b) => new Range(Math.Min(a.Start, b.Start), Math.Max(a.End, b.End));

            public int Length { get => End - Start + 1; } // inclusive
        }

        public List<(Vector2 scanner, Vector2 beacon)> ParseInput() => values.Select(v => v
                                                                                .Split(':')
                                                                                .Select(Utils.ValueFromString<Vector2>)
                                                                                .Chunk(2).Select(p => (p.First(), p.Last()))
                                                                                .First())
                                                                              .ToList();

        [TestMethod]
        public void Problem1()
        {
            var input = ParseInput();
            var beacons = input.Select(v => v.beacon).Distinct().ToList();
                        
            List<Range> ranges = new();

            foreach (var (scanner, beacon) in input)
            {
                int distance = Math.Abs(scanner.X - beacon.X) + Math.Abs(scanner.Y - beacon.Y);
                int offset = Math.Abs(scanner.Y - 2000000);
                if (offset <= distance)
                {
                    Range range = new(scanner.X - (distance - offset), scanner.X + (distance - offset));

                    var overlaps = ranges.Where(r => range.Overlaps(r)).ToList();                    
                    range = overlaps.Aggregate(range, Range.Combine);

                    ranges = ranges.Except(overlaps).ToList();
                    ranges.Add(range);
                }
            }

            int result = ranges.Sum(r => r.Length) - beacons.Count(b => b.Y == 2000000);

            Assert.AreEqual(result, 6275922);
        }

        [TestMethod]
        public void Problem2()
        {
            var input = ParseInput();

            const int size = 4000000;
            for (int y = 0; y < size; y++)
            {
                List<Range> ranges = new();

                foreach (var (scanner, beacon) in input)
                {
                    int distance = Math.Abs(scanner.X - beacon.X) + Math.Abs(scanner.Y - beacon.Y);
                    int offset = Math.Abs(scanner.Y - y);
                    if (offset <= distance)
                    {
                        Range range = new(Math.Max(0, scanner.X - (distance - offset)), Math.Min(size, scanner.X + (distance - offset)));
                        if (range.Start > size || range.End < 0) continue;
                       
                        ranges.Add(range);
                    }
                }

                int current = 0;
                foreach(var range in ranges.OrderBy(r => r.Start))
                {
                    if(range.Start - current > 1)
                    {
                        long result = (current + 1) * (long)size + y;
                        Assert.AreEqual(result, 11747175442119);
                        return;
                    }

                    current = Math.Max(current, range.End);
                }
            }


        }
    }
}
