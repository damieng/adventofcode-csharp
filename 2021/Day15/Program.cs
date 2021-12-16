using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    class Node
    {
        public int Risk { get; init; }
        public int BestRiskPath { get; set; } = int.MaxValue;
        public List<Node> Adjacent { get; } = new List<Node>();
        public bool Visited { get; set; }
    }

    static void Main()
    {
        var riskArray = File.ReadAllLines("input")
            .Select(l => l.Select(c => (byte)(c - 48)).ToArray())
            .ToArray();

        Console.WriteLine("Part1 = " + Part1(riskArray));
        Console.WriteLine("Part2 = " + Part2(riskArray));
    }

    static long Part1(byte[][] riskArray)
    {
        var (start, end) = BuildNodeGraph(riskArray);
        return CalculateLowestRisk(start, end);
    }

    static long Part2(byte[][] riskArray)
    {
        var (start, end) = BuildNodeGraph(ExtendArray(riskArray, 5));
        return CalculateLowestRisk(start, end);
    }

    static byte[][] ExtendArray(byte[][] riskArray, int multiplier)
    {
        var extendedRiskArray = new byte[riskArray.Length * multiplier][];

        var yl = riskArray.Length;
        var xl = riskArray[0].Length;

        for (var y = 0; y < yl; y++)
            for (var my = 0; my < multiplier; my++)
            {
                var newRow = new byte[xl * multiplier];
                extendedRiskArray[my * yl + y] = newRow;
                for (var x = 0; x < xl; x++)
                {
                    for (var mx = 0; mx < multiplier; mx++)
                    {
                        var newRisk = riskArray[y][x] + my + mx;
                        newRow[mx * xl + x] = (byte)(newRisk > 9 ? newRisk - 9 : newRisk);
                    }
                }
            }

        return extendedRiskArray;
    }

    static (Node start, Node end) BuildNodeGraph(byte[][] riskArray)
    {
        var my = riskArray.Length - 1;
        var mx = riskArray[0].Length - 1;

        var nodeArray = new Node[my + 1, mx + 1];
        for (var y = 0; y <= my; y++)
            for (var x = 0; x <= mx; x++)
                nodeArray[y, x] = new Node { Risk = riskArray[y][x] };

        for (var y = 0; y <= my; y++)
            for (var x = 0; x <= mx; x++)
            {
                var node = nodeArray[y, x];
                if (y > 0) node.Adjacent.Add(nodeArray[y - 1, x]);
                if (y < my) node.Adjacent.Add(nodeArray[y + 1, x]);
                if (x > 0) node.Adjacent.Add(nodeArray[y, x - 1]);
                if (x < my) node.Adjacent.Add(nodeArray[y, x + 1]);
            }

        return (nodeArray[0, 0], nodeArray[my, mx]);
    }

    static long CalculateLowestRisk(Node start, Node end)
    {
        var leastRiskQueue = new PriorityQueue<Node, int>();
        leastRiskQueue.Enqueue(start, start.BestRiskPath = 0);

        while (leastRiskQueue.TryDequeue(out var current, out var bestRiskPath))
        {
            foreach (var adjacent in current.Adjacent)
                if (!adjacent.Visited)
                {
                    adjacent.Visited = true;
                    var isLowerRisk = adjacent.Risk + bestRiskPath < adjacent.BestRiskPath;
                    if (isLowerRisk)
                        leastRiskQueue.Enqueue(adjacent, adjacent.BestRiskPath = bestRiskPath + adjacent.Risk);
                }
        }

        return end.BestRiskPath;
    }
}