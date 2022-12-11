using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day11
    {
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        [DebuggerDisplay("{id} : {Count}")]
        class Monkey
        {
            public int id;
            public Queue<int> items;
            public int Count;
            public char Operation;
            public int constant;

            public int Test;
            public int TrueMonkey;
            public int FalseMonkey;

            public Monkey(string [] input)
            {
                id = input[0][7] - '0';
                items = new Queue<int>(input[1][18..].Split(',').Select(int.Parse));
                Count = 0;
                Operation = input[2][23];
                constant = input[2][25] == 'o' ? 1 : int.Parse(input[2][25..]);
                Test = int.Parse(input[3][21..]);
                TrueMonkey = int.Parse(input[4][29..]);
                FalseMonkey = int.Parse(input[5][30..]);

                if (input[2][25] == 'o') Operation = '^';
            }

            public void Process(Monkey[] monkeys, int divisor = 0, int modulo = 0)
            {
                checked
                {
                    while (items.TryDequeue(out int item))
                    {
                        Count++;
                        long value = item;

                        switch (Operation)
                        {
                            case '*':
                                value *= constant;
                                break;
                            case '+':
                                value += constant;
                                break;
                            case '^':
                                value *= value;
                                break;
                        }

                        if(divisor > 0) value /= divisor;

                        if(modulo > 0) value %= modulo;

                        var monkey = (value % Test == 0) ? TrueMonkey : FalseMonkey;
                        monkeys[monkey].items.Enqueue((int)value);
                    }
                }
            }
        }

        [TestMethod]
        public void Problem1()
        {           
            var monkeys = values.Chunk(7).Select(c => new Monkey(c.ToArray())).ToArray();

            for(int i = 0; i < 20; i++)
            {
                foreach(var monkey in monkeys)
                {
                    monkey.Process(monkeys, divisor:3);
                }
            }

            int result = monkeys.Select(m => m.Count).OrderByDescending(c => c).Take(2).Aggregate(1, (a, b) => a * b);

            Assert.AreEqual(result, 57348);
        }

        [TestMethod]
        public void Problem2()
        {
            var monkeys = values.Chunk(7).Select(c => new Monkey(c.ToArray())).ToArray();

            for (int i = 0; i < 10000; i++)
            {
                foreach (var monkey in monkeys)
                {
                    monkey.Process(monkeys, modulo:(2 * 3 * 5 * 7 * 11 * 13 * 17 * 19));
                }
            }

            checked
            {
                long result = monkeys.Select(m => (long)m.Count).OrderByDescending(c => c).Take(2).Aggregate((long)1, (a, b) => a * b);

                Assert.AreEqual(result, 14106266886);
            }
        }
    }
}
