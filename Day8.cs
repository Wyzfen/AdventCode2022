using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day8
    {
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<String> values = Utils.FromFile<String>($"{day}.txt");

        [TestMethod]
        public void Problem1()
        {
            var input = values.Select(row => row.Select(char.ToString).Select(int.Parse).ToList()).ToList();
            var visible = new bool [input.Count, input[0].Count]; // Y, X

            for(int y = 0; y < input.Count; y++)
            {
                int a = -1, b = -1, c = -1, d = -1;
                for(int x = 0, n = input[y].Count - 1; x < input[y].Count; x++, n--)
                {
                    if (input[y][x] > a)
                    {
                        a = input[y][x];
                        visible[y, x] = true;
                    }

                    if (input[y][n] > b)
                    {
                        b = input[y][n];
                        visible[y, n] = true;
                    }

                    if (input[x][y] > c)
                    {
                        c = input[x][y];
                        visible[x, y] = true;
                    }

                    if (input[n][y] > d)
                    {
                        d = input[n][y];
                        visible[n, y] = true;
                    }
                }
            }

            int result = visible.Cast<bool>().Count(b => b);

            Assert.AreEqual(result, 1825);
        }

        [TestMethod]
        public void Problem2()
        {

            var input = values.Select(row => row.Select(char.ToString).Select(int.Parse).ToList()).ToList();
            int result = 0;

            for (int y = 0; y < input.Count; y++)
            {
                for (int x = 0; x < input[y].Count; x++)
                {
                    int height = input[y][x];

                    int left = 0;
                    for(int i = x - 1; i >= 0; i--)
                    {
                        left++;
                        if (input[y][i] >= height) break;
                    }

                    int right = 0;
                    for (int i = x + 1; i < input[y].Count; i++)
                    {
                        right++;
                        if (input[y][i] >= height) break;
                    }

                    int up = 0;
                    for (int i = y - 1; i >= 0; i--)
                    {
                        up++;
                        if (input[i][x] >= height) break;
                    }

                    int down = 0;
                    for (int i = y + 1; i < input.Count; i++)
                    {
                        down++;
                        if (input[i][x] >= height) break;
                    }

                    int score = left * right * up * down;
                    result = Math.Max(score, result);
                }
            }

            Assert.AreEqual(result, 235200);
        }
    }
}
