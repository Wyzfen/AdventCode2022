using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day13
    {        
        readonly static string[] test = new[]
                                        {
                                                "[1,1,3,1,1]",
                                                "[1,1,5,1,1]",
                                                "[[1],[2,3,4]]",
                                                "[[1],4]",
                                                "[9]",
                                                "[[8,7,6]]",
                                                "[[4,4],4,4]",
                                                "[[4,4],4,4,4]",
                                                "[7,7,7,7]",
                                                "[7,7,7]",
                                                "[]",
                                                "[3]",
                                                "[[[]]]",
                                                "[[]]",
                                                "[1,[2,[3,[4,[5,6,7]]]],8,9]",
                                                "[1,[2,[3,[4,[5,6,0]]]],8,9]",
                                        };
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt").Where(s => !string.IsNullOrEmpty(s));

        static ArrayList Parse(List<char> line)
        {
            var output = new ArrayList();
            line.RemoveAt(0); // remove [

            while (line.Count > 0)
            {
                switch (line[0])
                {
                    case '[':
                        output.Add(Parse(line));
                        break;
                    case ']':
                        line.RemoveAt(0);
                        return output;
                    case ',':
                        line.RemoveAt(0);
                        break;
                    default: // digits
                        var digits = String.Join(null, line.TakeWhile(char.IsDigit));
                        line.RemoveRange(0, digits.Length);

                        output.Add(int.Parse(digits));
                        break;
                }
            }

            return output;
        }

        static bool? ComparePair(ArrayList left, ArrayList right)
        {
            for(int i = 0; i < left.Count; i++)
            {
                if (right.Count <= i) break;

                switch(left[i], right[i])
                {
                    case (ValueType ao, ValueType bo) when ao is int a && bo is int b:
                        if (a > b) return false;
                        if (a < b) return true;
                        break;
                    case (ValueType ao, ArrayList b) when ao is int a:
                        {
                            if (ComparePair(new ArrayList { a }, b) is bool result) return result;
                        }
                        break;
                    case (ArrayList a, ValueType bo) when bo is int b:
                        {
                            if (ComparePair(a, new ArrayList { b }) is bool result) return result;
                        }
                        break;
                    case (ArrayList a, ArrayList b):
                        {
                            if (ComparePair(a, b) is bool result) return result;
                        }
                        break;
                }
            }

            if (left.Count == right.Count) return null;

            return left.Count < right.Count;
        }

        [TestMethod]
        public void Problem1()
        {
            var input = values.Select(s => Parse(s.ToList()));
            List<(ArrayList left, ArrayList right)> output = new();
            foreach (var pair in input.Chunk(2).ToList())
            {
                output.Add((pair[0], pair[1]));
            }

            int result = output.Select((p, i) => (p.left, p.right, index:i + 1)).Where(t => ComparePair(t.left, t.right).Value).Sum(t => t.index);

            Assert.AreEqual(result, 4821);
        }

        [TestMethod]
        public void Problem2()
        {
            const string divA = "[[2]]";
            const string divB = "[[6]]";

            var input = values.Union(new[] { divA, divB } ).Select(s => Parse(s.ToList())).ToList();

            input.Sort((a, b) => ComparePair(a, b).Value ? -1 : 1);

            var indexA = 1 + input.FindIndex(v => v.Count == 1 && v[0] is ArrayList a && a.Count == 1 && a[0] is int i && i == 2);
            var indexB = 1 + input.FindIndex(v => v.Count == 1 && v[0] is ArrayList a && a.Count == 1 && a[0] is int i && i == 6);

            int result = indexA * indexB;

            Assert.AreEqual(result, 21890);
        }
    }
}
