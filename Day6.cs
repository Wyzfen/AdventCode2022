using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day6
    {
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly string values = Utils.FromFile<string>($"{day}.txt").First();

        [TestMethod]
        public void Problem1()
        {
            int result = values.Sliding(4).FirstIndex(v => v.Distinct().Count() == v.Count()) + 4;

            Assert.AreEqual(result, 1929);
        }

        [TestMethod]
        public void Problem2()
        {
            int result = values.Sliding(14).FirstIndex(v => v.Distinct().Count() == v.Count()) + 14;

            Assert.AreEqual(result, 3298);
        }
    }
}
