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

        static Vector2 FindCharacter(string[] values, char c)
        {
            var line = values.FirstIndex(s => s.Contains(c));
            return new Vector2(values[line].IndexOf(c), line);
        }

        static void ReplaceCharacter(ref string[] values, Vector2 location, char c)
        {
            var sb = new System.Text.StringBuilder(values[location.Y]);
            sb[location.X] = c;
            values[location.Y] = sb.ToString();
        }

        static (Vector2 start, Vector2 end) ProcessInput(ref string [] values)
        {
            var start = FindCharacter(values, 'S');
            var end = FindCharacter(values, 'E');

            ReplaceCharacter(ref values, start, 'a');
            ReplaceCharacter(ref values, end, 'z');
            
            return (start, end);
        }

        static int[][] CalculateDistances(Vector2 start, string[] values, Func<Vector2, Vector2, bool> predicate)
        {  
            int maxY = values.Length;
            int maxX = values[0].Length;

            int[][] distances = values.Select(s => s.Select(_ => int.MaxValue).ToArray()).ToArray();

            distances[start.Y][start.X] = 0;

            Queue<Vector2> open = new(new[] { start });
            
            var directions = new Vector2[] { new(1, 0), new(0, 1), new(-1, 0), new(0, -1) };

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

            return distances;
        }

        [TestMethod]
        public void Problem1()
        {
            var (start, end) = ProcessInput(ref values);
            var distances = CalculateDistances(start, values, (a, b) => values[a.Y][a.X] - values[b.Y][b.X] <= 1);

            int result = distances[end.Y][end.X];

            Assert.AreEqual(result, 520);
        }

        [TestMethod]
        public void Problem2()
        {
            var (_, end) = ProcessInput(ref values);
            var distances = CalculateDistances(end, values, (a, b) => values[a.Y][a.X] - values[b.Y][b.X] >= -1);

            var lowPoints = values.SelectMany((s, y) => s.Select((c, x) => (c, x, y)).Where(p => p.c == 'a').Select(p => new Vector2(p.x, p.y)));

            var result = lowPoints.Select(p => distances[p.Y][p.X]).Min();
            Assert.AreEqual(result, 508);
        }
    }
}
