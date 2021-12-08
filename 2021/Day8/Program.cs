using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

record Display(string[] signals, string[] output);

class Program
{
    static void Main()
    {
        var displays = File
            .ReadAllLines("input")
            .Select(l => l.Split('|'))
            .Select(p => new Display(p[0].Split(' ', StringSplitOptions.RemoveEmptyEntries), p[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)))
            .ToArray();

        Console.WriteLine("Part1 = " + Part1(displays));
        Console.WriteLine("Part2 = " + Part2(displays));
    }

    private static readonly int[] segmentCounts = new int[] { 6, 2, 5, 5, 4, 5, 6, 3, 7, 6 };

    private static long Part1(Display[] displays)
    {
        int count = 0;
        var uniqueSegments = segmentCounts.GroupBy(s => s).Where(g => g.Count() == 1).Select(g => g.Key).ToList();
        foreach (Display display in displays)
        {
            count += display.output.Count(d => uniqueSegments.Contains(d.Length));
        }

        return count;
    }

    private static long Part2(Display[] displays)
    {
        int count = 0;
        foreach (Display display in displays)
        {
            var bySegmentCount = display.signals.GroupBy(g => g.Length).ToDictionary(g => g.Key, v => v.ToList());

            // Unique counts
            var one = bySegmentCount[2][0];
            var four = bySegmentCount[4][0];
            var seven = bySegmentCount[3][0];
            var eight = bySegmentCount[7][0];

            // Nine contains same segments as 4 with two extra
            var fourParts = four.ToCharArray();
            var nine = display.signals.Single(s => s.ToCharArray().Intersect(fourParts).Count() == fourParts.Length && s.ToCharArray().Except(fourParts).Count() == 2);

            // Middle segment is in four and nine but not seven and should appear 7 times
            var nineParts = nine.ToCharArray();
            var sevenParts = seven.ToCharArray();
            var segment3 = fourParts.Intersect(nineParts).Except(seven).Where(p => display.signals.Count(s => s.Contains(p)) == 7).Single();

            // Zero has 6 segments with no middle segment
            var zero = display.signals.Single(s => s.Length == 6 && !s.Contains(segment3));

            // Six is the only remaining six segment 
            var six = display.signals.Single(s => s.Length == 6 && s != nine && s != zero);

            // Five intersects six with one missing
            var sixParts = six.ToCharArray();
            var five = display.signals.Single(s => s.Length == 5 && s.ToCharArray().Intersect(sixParts).Count() == 5);

            // Three intersects nine with one missing
            var three = display.signals.Single(s => s.Length == 5 && s != five && s.ToCharArray().Intersect(nineParts).Count() == 5);

            // Two is the last remaining
            var two = display.signals.Single(s => s != one && s != three && s != four && s != five && s != six && s != seven && s != eight && s != nine && s != zero);

            var digitMap = new Dictionary<string, int>
            {
                { SortString(one), 1 },
                { SortString(two), 2 },
                { SortString(three), 3 },
                { SortString(four), 4 },
                { SortString(five), 5 },
                { SortString(six), 6 },
                { SortString(seven), 7 },
                { SortString(eight), 8 },
                { SortString(nine), 9 },
                { SortString(zero), 0 }
            };

            // What is the output?
            int result = 0;
            foreach(var digit in display.output) {
                result *= 10;
                result += digitMap[SortString(digit)];
            }
            count += result;
        }

        return count;
    }

    private static string SortString(string input) {
        var sorted = input.ToCharArray();
        Array.Sort(sorted);
        return new string(sorted);
    }
}