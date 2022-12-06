using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day4
    {
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        private int [] Value(string value) => value.Split(',', '-').Select(int.Parse).ToArray();

        [TestMethod]
        public void Problem1()
        {           
            int result = values.Select(Value).Count(a => (a[0] >= a[2] && a[1] <= a[3]) || (a[0] <= a[2] && a[1] >= a[3]));

            Assert.AreEqual(result, 576);
        }

        [TestMethod]
        public void Problem2()
        {
            int result = values.Select(Value).Count(a => a[0] <= a[3] && a[1] >= a[2]); 

            Assert.AreEqual(result, 905);
        }
    }
}
