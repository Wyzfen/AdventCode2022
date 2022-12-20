using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day19
    {
        readonly static string [] test = new string[]
        {
            "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.",
            "Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian."
        };

        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        record struct Blueprint(int oreRobotOre, int clayRobotOre, int obsidianRobotOre, int obsidianRobotClay, int geodeRobotOre, int geodeRobotObsidian);
        record struct State(int ore, int clay, int obsidian, int geodes, int oreRobots, int clayRobots, int obsidianRobots, int geodeRobots, int minutes);

        List<Blueprint> ParseInput(IEnumerable<string> inputs)
        {
            List<Blueprint> blueprints = new();

            foreach(var value in inputs)
            {
                var strings = value.Split('.', ':');
                int oreRobotOre = int.Parse(strings[1][22..23]);
                int clayRobotOre = int.Parse(strings[2][23..24]);
                int obsidianRobotOre = int.Parse(strings[3][27..28]);
                int obsidianRobotClay = int.Parse(strings[3][37..39]);
                int geodeRobotOre = int.Parse(strings[4][24..25]);
                int geodeRobotClay = int.Parse(strings[4][34..36]);

                blueprints.Add(new Blueprint(oreRobotOre, clayRobotOre, obsidianRobotOre, obsidianRobotClay, geodeRobotOre, geodeRobotClay));
            }

            return blueprints;
        }

        int CalculateTime(int current, int cost, int perTick)
        {
            int missing = cost - current;
            if (missing <= 0) return 1; // build time

            if (perTick == 0) return int.MaxValue;

            return 1 + (missing / perTick) + ((missing % perTick) != 0 ? 1 : 0);
        }

        State RunBlueprint(Blueprint blueprint, State state, int maxMinutes)
        {
            var maxState = state;

            if (state.minutes >= maxMinutes) return state;

            // each step, there's 4 robots that could be built, so test each
            // There's no point building more of a robot, than the maximum that can be used in one turn
            #region Geode
            int time = Math.Max(CalculateTime(state.ore, blueprint.geodeRobotOre, state.oreRobots),
                                CalculateTime(state.obsidian, blueprint.geodeRobotObsidian, state.obsidianRobots));
            if (time != int.MaxValue && state.minutes + time < maxMinutes)
            {
                var newState = RunBlueprint(blueprint, state with
                {
                    minutes = state.minutes + time,
                    ore = state.ore + time * state.oreRobots - blueprint.geodeRobotOre,
                    clay = state.clay + time * state.clayRobots,
                    obsidian = state.obsidian + time * state.obsidianRobots - blueprint.geodeRobotObsidian,
                    geodeRobots = state.geodeRobots + 1,
                    geodes = state.geodes + (maxMinutes - state.minutes - time) // generate all the geodes now
                }, maxMinutes);

                if (newState.geodes > maxState.geodes)
                {
                    maxState = newState;
                }
            }
            #endregion

            #region Obsidian
            if (state.obsidianRobots < blueprint.geodeRobotObsidian)
            {
                time = Math.Max(CalculateTime(state.ore, blueprint.obsidianRobotOre, state.oreRobots),
                                CalculateTime(state.clay, blueprint.obsidianRobotClay, state.clayRobots));
                if (time != int.MaxValue && state.minutes + time < maxMinutes)
                {
                    var newState = RunBlueprint(blueprint, state with
                    {
                        minutes = state.minutes + time,
                        ore = state.ore + time * state.oreRobots - blueprint.obsidianRobotOre,
                        clay = state.clay + time * state.clayRobots - blueprint.obsidianRobotClay,
                        obsidian = state.obsidian + time * state.obsidianRobots,
                        obsidianRobots = state.obsidianRobots + 1
                    },  maxMinutes);

                    if (newState.geodes > maxState.geodes)
                    {
                        maxState = newState;
                    }
                }
            }
            #endregion

            #region Clay
            if (state.clayRobots < blueprint.obsidianRobotClay)
            {
                time = CalculateTime(state.ore, blueprint.clayRobotOre, state.oreRobots);
                if (time != int.MaxValue && state.minutes + time < maxMinutes)
                {
                    var newState = RunBlueprint(blueprint, state with
                    {
                        minutes = state.minutes + time,
                        ore = state.ore + time * state.oreRobots - blueprint.clayRobotOre,
                        clay = state.clay + time * state.clayRobots,
                        obsidian = state.obsidian + time * state.obsidianRobots,
                        clayRobots = state.clayRobots + 1
                    },  maxMinutes);

                    if (newState.geodes > maxState.geodes)
                    {
                        maxState = newState;
                    }
                }
            }
            #endregion

            #region Ore
            if (state.oreRobots < blueprint.oreRobotOre || state.oreRobots < blueprint.clayRobotOre || state.oreRobots < blueprint.obsidianRobotOre || state.oreRobots < blueprint.geodeRobotOre)
            {
                time = CalculateTime(state.ore, blueprint.oreRobotOre, state.oreRobots);
                if (time != int.MaxValue && state.minutes + time < maxMinutes)
                {
                    var newState = RunBlueprint(blueprint, state with
                    {
                        minutes = state.minutes + time,
                        ore = state.ore + time * state.oreRobots - blueprint.oreRobotOre,
                        clay = state.clay + time * state.clayRobots,
                        oreRobots = state.oreRobots + 1
                    }, maxMinutes);

                    if (newState.geodes > maxState.geodes)
                    {
                        maxState = newState;
                    }
                }
            }
            #endregion

            return maxState;
        }

        [TestMethod]
        public void Problem1()
        {
            var blueprints = ParseInput(values);
            var results = blueprints.Select(b => RunBlueprint(b, new State(0, 0, 0, 0, 1, 0, 0, 0, 0), 24));
            int result = results.Select((g, i) => (i + 1) * g.geodes).Sum();

            Assert.AreEqual(result, 1528);
        }

        [TestMethod]
        public void Problem2()
        {
            var blueprints = ParseInput(values).Take(3).ToList();
            var results =  blueprints.AsParallel().Select(b => RunBlueprint(b, new State(0, 0, 0, 0, 1, 0, 0, 0, 0), 32)).ToList();

            int result = results.Aggregate(1, (a, b) => a * b.geodes);

            Assert.AreEqual(result, 16926);
        }
    }
}
