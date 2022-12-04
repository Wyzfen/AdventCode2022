using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day1
    {
        readonly IEnumerable<String> values = Utils.StringsFromFile("day1.txt");

        [TestMethod]
        public void Problem1()
        {
            int result = Utils.MergeLines(values).Select(a => a.Sum(int.Parse)).Max();

            Assert.AreEqual(result, 67633);
        }

        [TestMethod]
        public void Problem2()
        {
            int result = Utils.MergeLines(values).Select(a => a.Sum(int.Parse)).OrderBy(a => a).TakeLast(3).Sum();

            Assert.AreEqual(result, 199628);
        }
    }
}
