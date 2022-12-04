using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day2
    {
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        readonly Dictionary<string, int> scoreLookup = new Dictionary<string, int> { {"A X", 4},
                                                                                    {"A Y", 8},
                                                                                    {"A Z", 3},
                                                                                    {"B X", 1},
                                                                                    {"B Y", 5},
                                                                                    {"B Z", 9},
                                                                                    {"C X", 7},
                                                                                    {"C Y", 2},
                                                                                    {"C Z", 6},
                                                                                  };

        [TestMethod]
        public void Problem1()
        {           
            int result = values.Sum(v => scoreLookup[v]);

            Assert.AreEqual(result, 8933);
        }

        // Transform strategy into play from part 1
        readonly Dictionary<string, string> playLookup = new Dictionary<string, string> { {"A X", "A Z"},
                                                                                    {"A Y", "A X"},
                                                                                    {"A Z", "A Y"},
                                                                                    {"B X", "B X"},
                                                                                    {"B Y", "B Y"},
                                                                                    {"B Z", "B Z"},
                                                                                    {"C X", "C Y"},
                                                                                    {"C Y", "C Z"},
                                                                                    {"C Z", "C X"},
                                                                                  };

        [TestMethod]
        public void Problem2()
        {
            int result = values.Select(v => playLookup[v]).Sum(v => scoreLookup[v]);

            Assert.AreEqual(result, 11998);
        }
    }
}
