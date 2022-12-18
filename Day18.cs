using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day18
    {
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly List<Vector3> values = Utils.FromFile<string>($"{day}.txt").Select(Utils.ValueFromString<Vector3>).ToList();

        [TestMethod]
        public void Problem1()
        {
            int maxx = values.Max(v => v.X);
            int maxy = values.Max(v => v.Y);
            int maxz = values.Max(v => v.Z);

            int[,,] grid = new int[maxx + 3, maxy + 3, maxz + 3];
            values.ForEach(v => grid[v.X + 1, v.Y + 1, v.Z + 1] = 1);
            
            int result = values.Sum(v => 6 -  grid[v.X    , v.Y + 1, v.Z + 1] 
                                       - grid[v.X + 2, v.Y + 1, v.Z + 1]
                                       - grid[v.X + 1, v.Y    , v.Z + 1]
                                       - grid[v.X + 1, v.Y + 2, v.Z + 1]
                                       - grid[v.X + 1, v.Y + 1, v.Z    ]
                                       - grid[v.X + 1, v.Y + 1, v.Z + 2]);

            Assert.AreEqual(result, 4192);
        }

        [TestMethod]
        public void Problem2()
        {
            int maxx = values.Max(v => v.X) + 3;
            int maxy = values.Max(v => v.Y) + 3;
            int maxz = values.Max(v => v.Z) + 3;

            int[,,] grid = new int[maxx, maxy, maxz];
            values.ForEach(v => grid[v.X + 1, v.Y + 1, v.Z + 1] = 1);

            void Iterate(Action<int, int, int> func)
            {
                for (int x = 0; x < maxx; x++)
                {
                    for (int y = 0; y < maxy; y++)
                    {
                        for (int z = 0; z < maxz; z++)
                        {
                            func(x, y, z);
                        }
                    }
                }
            }

            Iterate((x, y, z) => { if (grid[x, y, z] == 0) grid[x, y, z] = -1; });

            void FloodFill(int x, int y, int z, int to, int from)
            {
                if (x < 0 || x >= maxx || y < 0 || y >= maxy || z < 0 || z >= maxz) return;
                if (grid[x, y, z] != from) return;

                grid[x, y, z] = to;

                FloodFill(x - 1, y, z, to, from);
                FloodFill(x + 1, y, z, to, from);
                FloodFill(x, y - 1, z, to, from);
                FloodFill(x, y + 1, z, to, from);
                FloodFill(x, y, z - 1, to, from);
                FloodFill(x, y, z + 1, to, from);
            }

            FloodFill(0, 0, 0, 0, -1);

            Iterate((x, y, z) => { if (grid[x, y, z] == -1) grid[x, y, z] = 1; });

            int result = values.Sum(v => 6 - grid[v.X, v.Y + 1, v.Z + 1]
                                - grid[v.X + 2, v.Y + 1, v.Z + 1]
                                - grid[v.X + 1, v.Y, v.Z + 1]
                                - grid[v.X + 1, v.Y + 2, v.Z + 1]
                                - grid[v.X + 1, v.Y + 1, v.Z]
                                - grid[v.X + 1, v.Y + 1, v.Z + 2]);

            Assert.AreEqual(result, 2520);
        }
    }
}
