using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day21
    {
        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        class Monad
        {
            public string Name { get; init; }

            string Function { get; init; }

            public bool IsConstant => char.IsDigit(Function[0]);

            public string Left => Function[..4];
            public string Right => Function[7..];

            public Monad(string value)
            {
                (Name, Function, _) = value.Split(": ");
            }

            public long Eval(Dictionary<string, Monad> monads)
            {
                if (IsConstant)
                {
                    return int.Parse(Function);
                }

                checked
                {
                    var left = monads[Left].Eval(monads);
                    var right = monads[Right].Eval(monads);

                    return Function[5] switch
                    {
                        '+' => left + right,
                        '-' => left - right,
                        '*' => left * right,
                        '/' => left / right,
                        _ => 0
                    };
                }
            }

            public long EvalReverse(Dictionary<string, Monad> monads, string name = "")
            {
                var parent = monads.Values.Where(v => !v.IsConstant).FirstOrDefault(m => m.Left == Name || m.Right == Name);
                
                if(parent == null) // At root
                {                    
                    return Left == name ? monads[Right].Eval(monads) : monads[Left].Eval(monads);                    
                }

                checked
                {
                    var result = parent.EvalReverse(monads, Name);
                    if(IsConstant)
                    {
                        return result;
                    }
                    else if (Left == name) // root = name ? Right -> root ^? Right = name
                    {
                        var right = monads[Right].Eval(monads);
                        return Function[5] switch
                        {
                            '+' => result - right,
                            '-' => result + right,
                            '*' => result / right,
                            '/' => result * right,
                            _ => 0
                        };
                    }
                    else // root = Left ? name
                    {
                        var left = monads[Left].Eval(monads);
                        return Function[5] switch
                        {
                            '+' => result - left,
                            '-' => left - result,
                            '*' => result / left,
                            '/' => left / result,
                            _ => 0
                        };
                    }
                }
            }
        }


        [TestMethod]
        public void Problem1()
        {
            var monads = values.Select(x => new Monad(x)).ToDictionary(x => x.Name, x => x);
            long result = monads["root"].Eval(monads);

            Assert.AreEqual(result, 282285213953670);
        }

        [TestMethod]
        public void Problem2()
        {
            var monads = values.Select(x => new Monad(x)).ToDictionary(x => x.Name, x => x);
            var root = monads["root"];
            var humn = monads["humn"];

            long result = humn.EvalReverse(monads);

            Assert.AreEqual(result, 3699945358564);
        }
    }
}
