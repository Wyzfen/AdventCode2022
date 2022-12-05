using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day5
    {
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        List<char>[] stacks = new List<char>[9].Select(_ => new List<char>()).ToArray();
        List<int[]> moves = new();

        private void ProcessInput()
        {
            var inputQueue = new Queue<string>(values);
            string line = inputQueue.Dequeue();

            while(!line.Contains('1'))
            {
                for(int i = 0; i < 9; i++)
                {
                    char c = line[i * 4 + 1];
                    if(c != ' ')
                    {
                        stacks[i].Add(c);
                    }
                }

                line = inputQueue.Dequeue();
            }

            inputQueue.Dequeue();

            while(inputQueue.Count > 0)
            {
                line = inputQueue.Dequeue().Substring(5);
                line = line.Replace("from", ",").Replace("to", ",");
                moves.Add(line.Split(',').Select(int.Parse).ToArray());
            }

        }

        [TestMethod]
        public void Problem1()
        {
            ProcessInput();

            foreach(var move in moves)
            {
                int from = move[1] - 1;
                int to = move[2] - 1;
                int count = move[0];

                for (int i = 0; i < count; i++)
                {
                    char c = stacks[from][0];
                    stacks[from].RemoveAt(0);
                    stacks[to].Insert(0, c);
                }
            }

            string result = new String(stacks.Select(list => list[0]).ToArray());
            Assert.AreEqual(result, "QNNTGTPFN");
        }

        [TestMethod]
        public void Problem2()
        {
            ProcessInput();

            foreach (var move in moves)
            {
                int from = move[1] - 1;
                int to = move[2] - 1;
                int count = move[0];

                var items = stacks[from].GetRange(0, count);
                stacks[from].RemoveRange(0, count);
                stacks[to].InsertRange(0, items);
            }

            string result = new String(stacks.Select(list => list[0]).ToArray());
            Assert.AreEqual(result, "GGNPJBTTR");
        }
    }
}
