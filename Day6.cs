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
            int result = Enumerable.Range(4, values.Length - 4).First(i => values.Substring(i - 4, 4).Distinct().Count() == 4);

            Assert.AreEqual(result, 1929);
        }

        [TestMethod]
        public void Problem2()
        {
            int result = Enumerable.Range(14, values.Length - 14).First(i => values.Substring(i - 14, 14).Distinct().Count() == 14);


            Assert.AreEqual(result, 3298);
        }
    }
}
