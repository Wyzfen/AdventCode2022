using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day3
    {
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        private int Value(char c) => (c - 'A' + 26) % 58 + 1;

        [TestMethod]
        public void Problem1()
        {
            int result = values.Select(v => v.Take(v.Length / 2).Intersect(v.TakeLast(v.Length / 2)).First())
                               .Sum(Value);

            Assert.AreEqual(result, 7831);
        }

        [TestMethod]
        public void Problem2()
        {
            int result = values.Chunk(3).Select(b => b.ToList()) // Group into sets of 3 and turn into list so can index (could just use .skip & .take)
                            .Select(b => b[0].Intersect(b[1]).Intersect(b[2]).First()) // Find common value
                            .Sum(Value);

            Assert.AreEqual(result, 2683);
        }
    }
}
