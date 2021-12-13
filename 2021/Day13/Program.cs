using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

class Program
{
    record Point(int X, int Y);
    record Fold(char Axis, int At);

    class Paper
    {
        public bool[,] Dots
        {
            get; init;
        }

        public int SizeX { get; set; }
        public int SizeY { get; set; }

        public Paper(bool[,] Dots, int SizeX, int SizeY)
        {
            this.Dots = Dots;
            this.SizeX = SizeX;
            this.SizeY = SizeY;
        }
    }

    const string foldPrefix = "fold along ";

    static void Main()
    {
        var lines = File.ReadAllLines("input");

        var points = new List<Point>();
        var folds = new List<Fold>();

        foreach (var line in lines)
        {
            if (line.Contains(','))
            {
                var parts = line.Split(',');
                points.Add(new Point(int.Parse(parts[0]), int.Parse(parts[1])));
            }

            if (line.StartsWith(foldPrefix))
            {
                var axis = line[foldPrefix.Length];
                var parts = line.Split('=');
                folds.Add(new Fold(axis, int.Parse(parts[1])));
            }
        }

        var paper = CreatePaperWithPoints(points);

        Console.WriteLine("Part1 = " + Part1(paper, folds));
        Console.WriteLine("Part2 = \n" + Part2(paper, folds));
    }

    private static Paper CreatePaperWithPoints(List<Point> points)
    {
        var sizeX = points.Max(p => p.X);
        var sizeY = points.Max(p => p.Y);

        var dots = new bool[sizeX + 1, sizeY + 1];
        foreach (var point in points)
            dots[point.X, point.Y] = true;

        return new Paper(dots, sizeX, sizeY);
    }

    static int Part1(Paper paper, List<Fold> folds)
    {
        FoldPaper(paper, folds[0]);
        return CountDots(paper);
    }

    static string Part2(Paper paper, List<Fold> folds)
    {
        foreach (var fold in folds.Skip(1))
            FoldPaper(paper, fold);
        return PrintPaper(paper);
    }

    static string PrintPaper(Paper paper)
    {
        var sb = new StringBuilder();
        for (var y = 0; y <= paper.SizeY; y++)
        {
            var line = new char[paper.SizeX + 1];
            for (var x = 0; x <= paper.SizeX; x++)
                line[x] = paper.Dots[x, y] ? '#' : '.';
            sb.AppendLine(new string(line));
        }
        return sb.ToString();
    }

    private static void FoldPaper(Paper paper, Fold fold)
    {
        switch (fold.Axis)
        {
            case 'y': FoldPaperAlongY(paper, fold); break;
            case 'x': FoldPaperAlongX(paper, fold); break;
            default: throw new InvalidDataException($"Unknown fold axis {fold.Axis}");
        }
    }

    private static void FoldPaperAlongX(Paper paper, Fold fold)
    {
        for (var y = 0; y <= paper.SizeY; y++)
            for (var x = fold.At; x <= paper.SizeX; x++)
                if (paper.Dots[x, y])
                    paper.Dots[paper.SizeX - x, y] = true;
        paper.SizeX = fold.At - 1;
    }

    private static void FoldPaperAlongY(Paper paper, Fold fold)
    {
        for (var x = 0; x <= paper.SizeX; x++)
            for (var y = fold.At; y <= paper.SizeY; y++)
                if (paper.Dots[x, y])
                    paper.Dots[x, paper.SizeY - y] = true;
        paper.SizeY = fold.At - 1;
    }

    private static int CountDots(Paper paper)
    {
        int pointCount = 0;

        for (var x = 0; x <= paper.SizeX; x++)
            for (var y = 0; y <= paper.SizeY; y++)
                if (paper.Dots[x, y]) pointCount++;

        return pointCount;
    }
}