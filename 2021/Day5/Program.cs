using System;
using System.IO;
using System.Linq;

class Program
{
    record Point(int X, int Y);
    record Line(Point Start, Point End);

    static Line MakeLine(string line)
    {
        var parts = line.Split(" -> ");
        var start = MakePoint(parts[0]);
        var end = MakePoint(parts[1]);
        return new Line(start, end);
    }

    static Point MakePoint(string pair)
    {
        var parts = pair.Split(',').Select(p => int.Parse(p)).ToArray();
        return new Point(parts[0], parts[1]);
    }

    static void Main()
    {
        var lines = File
            .ReadAllLines("input")
            .Select(text => MakeLine(text))
            .ToArray();

        Console.WriteLine("Part 1 = " + Part1(lines));
        Console.WriteLine("Part 2 = " + Part2(lines));
    }

    private static int Part1(Line[] lines)
    {
        int intersections = 0;
        var seabed = new short[1000, 1000];
        foreach (var line in lines)
        {
            // Vertical
            if (line.Start.X == line.End.X)
            {
                var start = Math.Min(line.Start.Y, line.End.Y);
                var end = Math.Max(line.Start.Y, line.End.Y);
                for (var y = start; y <= end; y++)
                {
                    if (seabed[line.Start.X, y]++ == 1)
                        intersections++;
                }
            }
            else
            {
                if (line.Start.Y == line.End.Y)
                {
                    var start = Math.Min(line.Start.X, line.End.X);
                    var end = Math.Max(line.Start.X, line.End.X);
                    for (var x = start; x <= end; x++)
                    {
                        if (seabed[x, line.Start.Y]++ == 1)
                            intersections++;
                    }
                }
            }
        }
        return intersections;
    }

    private static int Part2(Line[] lines)
    {
        int intersections = 0;
        var seabed = new short[1000, 1000];

        foreach (var line in lines)
        {
            var xStep = line.End.X == line.Start.X ? 0 : line.End.X > line.Start.X ? 1 : -1;
            var yStep = line.End.Y == line.Start.Y ? 0 : line.End.Y > line.Start.Y ? 1 : -1;

            var x = line.Start.X;
            var y = line.Start.Y;

            while (x != line.End.X || y != line.End.Y)
            {
                if (seabed[x, y]++ == 1)
                    intersections++;
                x += xStep;
                y += yStep;
            } 

            seabed[x, y]++;
        }

        return intersections;
    }
}