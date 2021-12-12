using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        // Should have done this with a tree but I can code this quicker
        var caveMap = new Dictionary<string, List<string>>();
        foreach (var connection in File.ReadAllLines("input").Select(o => o.Split('-')))
        {
            if (caveMap.TryGetValue(connection[0], out var exits))
            {
                exits.Add(connection[1]);
            }
            else
            {
                caveMap.Add(connection[0], new List<string>() { connection[1] });
            }

            if (caveMap.TryGetValue(connection[1], out exits))
            {
                exits.Add(connection[0]);
            }
            else
            {
                caveMap.Add(connection[1], new List<string>() { connection[0] });
            }
        }

        Console.WriteLine("Part1 = " + Part1(caveMap));
        Console.WriteLine("Part2 = " + Part2(caveMap));
    }

    static int Part1(Dictionary<string, List<string>> caveMap)
    {
        var path = new Stack<string>();
        var unique = new HashSet<string>();

        path.Push("start");
        Travel(caveMap, "start", path, unique, null);

        return unique.Count;
    }

    static int Part2(Dictionary<string, List<string>> caveMap)
    {
        var path = new Stack<string>();
        var unique = new HashSet<string>();

        path.Push("start");
        // Very slow/ugly approach but again code quicker, code re-use
        foreach (var small in caveMap.Keys.Where(k => k.ToString() == k.ToLower() && k != "start" && k != "end"))
        {
            Travel(caveMap, "start", path, unique, small);
        }

        return unique.Count;
    }

    private static void Travel(Dictionary<string, List<string>> caveMap, string current, Stack<string> path, HashSet<string> unique, string freeSmallCave)
    {
        foreach (var connection in caveMap[current])
        {
            if (connection == "end")
            {
                unique.Add(String.Join(',', path.Reverse().ToArray()) + ",end");
            }
            else
            {
                var isBigCave = char.IsUpper(connection[0]);
                var canVisitThisSmall = !isBigCave && connection == freeSmallCave && path.Where(p => p == connection).Count() < 2;
                if (isBigCave || !path.Contains(connection) || canVisitThisSmall)
                {
                    path.Push(connection);
                    Travel(caveMap, connection, path, unique, freeSmallCave);
                    path.Pop();
                }
            }
        }
    }
}