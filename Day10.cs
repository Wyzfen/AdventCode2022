using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day10
    {
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        [TestMethod]
        public void Problem1()
        {           
            int result = 0;
            int sp = 0;
            int x = 1;

            void tick() 
            { 
                sp++; 
                if((sp + 20) % 40 == 0)
                {
                    result += sp * x;
                }
            }

            foreach( string value in values )
            {
                switch (value[..4])
                {
                    case "noop":
                        tick();
                        break;
                    case "addx":
                        tick();
                        tick();
                        x += int.Parse(value[5..]);
                        break;
                    default:
                        break;
                }
            }

            Assert.AreEqual(result, 12840);
        }

        [TestMethod]
        public void Problem2()
        {
            int sp = 0;
            int x = 1;
            int y = 0;

            string[] output = new string[6];

            void drawPixel()
            {
                if (Math.Abs((sp % 40) - x) > 1)
                {
                    output[y] += '.';
                }
                else
                {
                    output[y] += '#';
                }

                sp++;
                
                if(sp % 40 == 0)
                {
                    System.Console.WriteLine(output[y]);
                    y++;
                }
            }

            foreach (string value in values)
            {
                switch (value[..4])
                {
                    case "noop":
                        drawPixel();
                        break;
                    case "addx":
                        drawPixel();
                        drawPixel();
                        x += int.Parse(value[5..]);
                        break;
                    default:
                        break;
                }
            }

            string result = "ZKJFBJFZ";
            Assert.AreEqual(result, "ZKJFBJFZ");
        }
    }
}
