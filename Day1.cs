using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2021
{
    [TestClass]
    public class Day1
    {
        readonly IEnumerable<String> values = Utils.StringsFromFile("day1.txt");

        [TestMethod]
        public void Problem1()
        {
            int result = 0;
            int sum = 0;

            foreach (var value in values)
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    sum += int.Parse(value);
                    continue;
                }
                else
                {
                    result = Math.Max(result, sum);
                    sum = 0;
                }
            }

            Assert.AreEqual(result, 67633);
        }

        [TestMethod]
        public void Problem2()
        {
            int result = 0;

            int sum = 0;
            var sums = new List<int>();

            foreach (var value in values)
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    sum += int.Parse(value);
                    continue;
                }
                else
                {
                    sums.Add(sum);
                    sum = 0;
                }
            }

            sums.Sort();

            result = sums.TakeLast(3).Sum();

            Assert.AreEqual(result, 199628);
        }
    }
}
