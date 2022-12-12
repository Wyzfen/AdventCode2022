using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day12
    {
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        string [] values = Utils.FromFile<string>($"{day}.txt").Where(s =>!string.IsNullOrWhiteSpace(s)).ToArray();

        Vector2 start;
        Vector2 end;

        int[][] distances;

        void ParseInput()
        {
            Vector2 FindCharacter(char c)
            {
                var line = values.FirstIndex(s => s.Contains(c));
                return new Vector2(values[line].IndexOf(c), line);
            }

            void ReplaceCharacter(Vector2 location, char c)
            {
                var sb = new System.Text.StringBuilder(values[location.Y]);
                sb[location.X] = c;
                values[location.Y] = sb.ToString();
            }

            start = FindCharacter('S');
            end = FindCharacter('E');
            ReplaceCharacter(start, 'a');
            ReplaceCharacter(end, 'z');

            distances = values.Select(s => Enumerable.Range(0, s.Length).Select(_ => int.MaxValue).ToArray()).ToArray();
        }

        void CalculateDistances(Vector2 start, Func<Vector2, Vector2, bool> predicate)
        {
            var directions = new Vector2[] { new(1, 0), new(0, 1), new(-1, 0), new(0, -1) };
            int maxY = distances.Length;
            int maxX = distances[0].Length;

            distances[start.Y][start.X] = 0;
            Queue<Vector2> open = new(new[] { start });

            while (open.TryDequeue(out var item))
            {
                var itemDistance = distances[item.Y][item.X];
                foreach (var test in directions.Select(d => d + item).Where(d => d.X >= 0 && d.Y >= 0 && d.X < maxX && d.Y < maxY))
                {
                    if (predicate(test, item) &&
                        distances[test.Y][test.X] > itemDistance + 1)
                    {
                        distances[test.Y][test.X] = itemDistance + 1;
                        open.Enqueue(test);
                    }
                }
            }
        }

        [TestMethod]
        public void Problem1()
        {
            ParseInput();
            CalculateDistances(start, (a, b) => values[a.Y][a.X] - values[b.Y][b.X] <= 1);

            int result = distances[end.Y][end.X];

            Assert.AreEqual(result, 520);
        }

        [TestMethod]
        public void Problem2()
        {
            ParseInput();
            CalculateDistances(end, (a, b) => values[a.Y][a.X] - values[b.Y][b.X] >= -1);

            var lowPoints = values.SelectMany((s, y) => s.Select((c, x) => (c, x, y)).Where(p => p.c == 'a').Select(p => new Vector2(p.x, p.y)));

            var result = lowPoints.Select(p => distances[p.Y][p.X]).Min();
            Assert.AreEqual(result, 508);
        }
    }
}
