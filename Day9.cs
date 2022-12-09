using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day9
    {
        readonly static IEnumerable<string> test = new string[] { "R 4", "U 4", "L 3", "D 1", "R 4", "D 1", "L 5", "R 2" };
        readonly static IEnumerable<string> test2 = new string[] { "R 5 ", "U 8 ", "L 8 ", "D 3 ", "R 17", "D 10", "L 25", "U 20" };

        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        [TestMethod]
        public void Problem1()
        {
            var input = values.Select(l => (dir:l.First(), dist:int.Parse(l[2..])));
            
            var head = new Vector2();
            var previous = new Vector2();
            var tail = new Vector2();

            var visited = new HashSet<Vector2> { tail };

            foreach(var (dir, dist) in input)
            {                
                int xo = 0, yo = 0;
                switch(dir) 
                {
                    case 'L': 
                        xo = -1;
                        break;
                    case 'R':
                        xo = 1;
                        break;
                    case 'U':
                        yo = -1;
                        break;
                    case 'D':
                        yo = 1;
                        break;
                }

                for(int i = 0; i < dist; i++)
                {
                    previous = head;

                    head.X += xo;
                    head.Y += yo;

                    if(Math.Abs(tail.X - head.X) > 1 || Math.Abs(tail.Y - head.Y) > 1)
                    {
                        tail = previous;
                    }

                    visited.Add(tail);
                }
            }

            int result = visited.Count;

            Assert.AreEqual(result, 5513);
        }

        [TestMethod]
        public void Problem2()
        {
            var input = values.Select(l => (dir: l.First(), dist: int.Parse(l[2..])));

            var head = new Vector2();
            var tails = new Vector2[9];

            var visited = new HashSet<Vector2> { head };

            foreach (var (dir, dist) in input)
            {
                int xo = 0, yo = 0;
                switch (dir)
                {
                    case 'L':
                        xo = -1;
                        break;
                    case 'R':
                        xo = 1;
                        break;
                    case 'U':
                        yo = -1;
                        break;
                    case 'D':
                        yo = 1;
                        break;
                }

                for (int i = 0; i < dist; i++)
                {
                    head.X += xo;
                    head.Y += yo;

                    Vector2 previous = head;

                    for (int t = 0; t < tails.Length; t++)
                    {
                        Vector2 tail = tails[t];

                        if (Math.Abs(tail.X - previous.X) <= 1 && Math.Abs(tail.Y - previous.Y) <= 1)
                            break;

                        tails[t].X += Math.Sign(previous.X - tail.X);
                        tails[t].Y += Math.Sign(previous.Y - tail.Y);

                        previous = tails[t];
                    }

                    visited.Add(tails.Last());
                }
            }

            int result = visited.Count;

            Assert.AreEqual(result, 2427);
        }
    }
}
