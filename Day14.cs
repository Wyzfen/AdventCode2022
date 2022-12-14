using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day14
    {
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        int minX, maxX, maxY;
        bool [,] cave;

        void ParseInput(IEnumerable<string> input, bool addFloor = false)
        {
            var polylines = input.Select(line => line.Split(" -> ").Select(Utils.ValueFromString<Vector2>).ToList()).ToList();
            var allPoints = polylines.SelectMany(line => line);
            
            minX = allPoints.Min(v => v.X);
            maxX = allPoints.Max(v => v.X);
            maxY = allPoints.Max(v => v.Y);

            if (addFloor)
            {
                polylines.Add(new() { new(minX - maxY, maxY + 2), new(maxX + maxY, maxY + 2) });
                minX -= maxY;
                maxX += maxY;
                maxY += 2;
            }

            cave = new bool[maxX - minX + 1, maxY + 1];

            foreach(var line in polylines)
            {
                var current = line[0];
                foreach(var next in line.Skip(1))
                {
                    if(current.X == next.X)
                    {
                        int x = current.X - minX;
                        int startY = Math.Min(current.Y, next.Y);
                        int endY = Math.Max(current.Y, next.Y);
                        for(int y = startY; y <= endY; y++)
                        {
                            cave[x, y] = true;
                        }
                    }
                    else if(current.Y == next.Y)
                    {
                        int y = current.Y;
                        int startX = Math.Min(current.X, next.X) - minX;
                        int endX = Math.Max(current.X, next.X) - minX;
                        for (int x = startX; x <= endX; x++)
                        {
                            cave[x, y] = true;
                        }
                    }
                    else
                    {

                    }

                    current = next;
                }
            }

        }

        // Returns true if it lands
        bool DropSand(Vector2 location)
        {
            location.X -= minX;

            if (!cave[location.X, location.Y])
            {
                while (location.Y < maxY && location.X >= 0 && location.X <= (maxX - minX))
                {
                    if (!cave[location.X, location.Y + 1])
                    {
                        location.Y += 1;
                    }
                    else if (!cave[location.X - 1, location.Y + 1])
                    {
                        location.X -= 1;
                        location.Y += 1;
                    }
                    else if (!cave[location.X + 1, location.Y + 1])
                    {
                        location.X += 1;
                        location.Y += 1;
                    }
                    else
                    {
                        cave[location.X, location.Y] = true;
                        return true;
                    }
                }
            }

            return false;
        }

        [TestMethod]
        public void Problem1()
        {
            ParseInput(values);

            int result = 0;

            while (DropSand(new(500, 0))) result++;

            Assert.AreEqual(result, 862);
        }

        [TestMethod]
        public void Problem2()
        {
            ParseInput(values, true);

            int result = 0;

            while (DropSand(new(500, 0))) result++;

            Assert.AreEqual(result, 28744);
        }
    }
}
