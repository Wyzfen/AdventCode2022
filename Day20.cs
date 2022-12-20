using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day20
    {
        readonly static List<int> test = new() {1, 2, -3, 3, -2, 0, 4};
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly List<int> values = Utils.FromFile<int>($"{day}.txt").ToList();

                
        static void Mix(List<long> values, List<int> indices)
        {
            checked
            {
                int length = values.Count;

                for (int i = 0; i < length; i++)
                {
                    int index = indices.IndexOf(i);
                    long value = values[index];
                    int newIndex = (int)((index + value % (length - 1) + (length - 1)) % (length - 1));

                    values.RemoveAt(index);
                    indices.RemoveAt(index);

                    values.Insert(newIndex, value);
                    indices.Insert(newIndex, i);
                }
            }
        }

        [TestMethod]
        public void Problem1()
        {
            var input = values.Select(a => (long)a).ToList();
            Mix(input, Enumerable.Range(0, input.Count).ToList());

            int index = input.IndexOf(0);
            long result = input[(index + 1000) % input.Count] +
                input[(index + 2000) % input.Count] +
                input[(index + 3000) % input.Count]; 
            
            Assert.AreEqual(result, 3346);

        }

        [TestMethod]
        public void Problem2()
        {
            var input = values.Select(a => (long)a * 811589153).ToList();
            var indices = Enumerable.Range(0, input.Count).ToList();
            for (int i = 0; i < 10; i++)
            {
                Mix(input, indices);
            }

            int index = input.IndexOf(0);
            long result = input[(index + 1000) % input.Count] +
                input[(index + 2000) % input.Count] +
                input[(index + 3000) % input.Count];
            
            Assert.AreEqual(result, 4265712588168);
        }
    }
}
