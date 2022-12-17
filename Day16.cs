using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day16
    {
        readonly static IEnumerable<string> test = new string[]
        {
            "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB",
            "Valve BB has flow rate=13; tunnels lead to valves CC, AA",
            "Valve CC has flow rate=2; tunnels lead to valves DD, BB",
            "Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE",
            "Valve EE has flow rate=3; tunnels lead to valves FF, DD",
            "Valve FF has flow rate=0; tunnels lead to valves EE, GG",
            "Valve GG has flow rate=0; tunnels lead to valves FF, HH",
            "Valve HH has flow rate=22; tunnel leads to valve GG",
            "Valve II has flow rate=0; tunnels lead to valves AA, JJ",
            "Valve JJ has flow rate=21; tunnel leads to valve II",
        };

        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        [DebuggerDisplay("{Location} @ {Rate}")]
        class Node
        {
            public string Location { get; init; }
            public int Rate { get; init; }

            public List<Node> Nodes { get; init; } = new ();
        }

        static int Traverse(List<Node> nodes, int current, IEnumerable<int> queue, int [,] times, int pressure, int time, int totalTime)
        {
            if (time >= totalTime) return pressure;

            int maxPressure = pressure;

            foreach(var node in queue)
            {
                var newTime = time + times[current, node] + 1; // time to move there, and activate

                if (newTime >= totalTime) continue;

                var newPressure = pressure + nodes[node].Rate * (totalTime - newTime);
                var newQueue = queue.Except(new[] { node }).ToList();

                var totalPressure = Traverse(nodes, node, newQueue, times, newPressure, newTime, totalTime);
                maxPressure = Math.Max(maxPressure, totalPressure);
            }
           
            return maxPressure;
        }

        static int Traverse2(int person, int personTime, int elephant, int elephantTime, int[] queue, int pressure, int totalTime, List<Node> nodes, int[,] times)
        {
            if (personTime >= totalTime || elephantTime >= totalTime) return pressure;

            int maxPressure = pressure;

            int thisTime = personTime;
            int thisNode = person;
            bool isElephant = false;

            if(elephantTime < personTime)
            {
                thisTime = elephantTime;
                thisNode = elephant;
                isElephant = true;
            }

            foreach (var node in queue)
            {
                var newTime = thisTime + times[thisNode, node] + 1; // time to move there, and activate

                if (newTime >= totalTime) continue;

                var newPressure = pressure + nodes[node].Rate * (totalTime - newTime);
                var newQueue = queue.Except(new[] { node }).ToArray();

                var totalPressure = isElephant ?
                    Traverse2(person, personTime, node, newTime, newQueue, newPressure, totalTime, nodes, times) :
                    Traverse2(node, newTime, elephant, elephantTime, newQueue, newPressure, totalTime, nodes, times);
                maxPressure = Math.Max(maxPressure, totalPressure);
            }

            return maxPressure;
        }

        static int[,] Distances(List<Node> nodes)
        {
            // Floyd-Warshall
            int[,] distances = new int[nodes.Count, nodes.Count];
            for(int x = 0; x < nodes.Count; x++)
            {
                for (int y = 0; y < nodes.Count; y++)
                {
                    distances[y, x] = 10000;
                }
            }

            foreach(var node in nodes)
            {
                int index = nodes.IndexOf(node);
                foreach(var neighbour in node.Nodes)
                {
                    int nIndex = nodes.IndexOf(neighbour);
                    distances[index, nIndex] = 1;
                    distances[nIndex, index] = 1;
                }

                distances[index, index] = 0;
            }

            for(int k = 0; k < nodes.Count; k++)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    for (int j = 0; j < nodes.Count; j++)
                    {
                        int dist = distances[i, k] + distances[k, j];
                        if (distances[i, j] > dist)
                        {                            
                            distances[i, j] = dist;
                        }
                    }
                }
            }

            return distances;
        }

        static List<Node> ParseInput(IEnumerable<string> values)
        {
            //Valve TU has flow rate = 0; tunnels lead to valves XG, ID
            var nodes = values.Select(line => new Node { Location = line[6..8], Rate = int.Parse(line[23..25].TrimEnd(';')) }).ToList();
            var links = values.Select(line => line[49..].Trim().Split(", ").Select(loc => nodes.First(node => node.Location == loc)));
            nodes.Zip(links).ToList().ForEach(zip => zip.First.Nodes.AddRange(zip.Second));
            return nodes;
        }

        [TestMethod]
        public void Problem1()
        {
            var nodes = ParseInput(values);            

            // 1. Find all distances between all nodes
            var distances = Distances(nodes);

            // Traverse only between points of interest; and remove them once visited, as no point going back. (Note, the path might lead back, but we're not stopping there)
            var usefulNodes = nodes.Select((n, i) => (node: n, index: i)).Where(n => n.node.Rate > 0).Select(n => n.index).ToList();
            var startNode = nodes.FirstIndex(n => n.Location == "AA");
            int result = Traverse(nodes, startNode, usefulNodes.ToArray(), distances, 0, 0, 30);

            Assert.AreEqual(result, 1659);
        }

        [TestMethod]
        public void Problem2()
        {
            var nodes = ParseInput(values);

            // 1. Find all distances between all nodes
            var distances = Distances(nodes);

            // Traverse only between points of interest; and remove them once visited, as no point going back. (Note, the path might lead back, but we're not stopping there)
            var usefulNodes = nodes.Select((n, i) => (node: n, index: i)).Where(n => n.node.Rate > 0).Select(n => n.index).ToList();
            var startNode = nodes.FirstIndex(n => n.Location == "AA");
            int result = Traverse2(startNode, 0, startNode, 0, usefulNodes.ToArray(), 0, 26, nodes, distances);

            Assert.AreEqual(result, 2382);
        }
    }
}
