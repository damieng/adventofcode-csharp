using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        var target = File.ReadAllText("input")
                         .Substring("target area:".Length)
                         .Split(',')
                         .Select(r => r.Substring(3).Split("..").Select(v => int.Parse(v)).ToArray())
                         .Select(r => (low: r[0], high: r[1]))
                         .ToArray();

        var calculate = Part1(target[0], target[1]);
        Console.WriteLine("Part1 = " + calculate.maxY);
        Console.WriteLine("Part2 = " + calculate.hitCount);

        Console.ReadKey();
    }

    static (int maxY, int hitCount) Part1((int low, int high) tx, (int low, int high) ty)
    {
        int hitCount = 0;
        int maxY = 0;

        for (var dx = 1; dx <= tx.high; dx++)
        {
            // There is probably a decent algorithm for this... 2500 steps is fast enough to brute force today...
            for (var dy = -2500; dy < 2500; dy++)
            {
                var breaker = 1000;
                var adx = dx;
                var ady = dy;
                bool hit = false;
                int x = 0, y = 0, my = 0;
                while (!hit && breaker-- > 0)
                {
                    x += adx;
                    y += ady;

                    if (adx > 0) adx--;
                    if (adx < 0) adx++;
                    ady--;

                    if (y > my) my = y;

                    if (x >= tx.low && x <= tx.high && y >= ty.low && y <= ty.high)
                    {
                        hit = true;
                        if (my > maxY) maxY = my;
                    }
                }

                if (hit) hitCount++;
            }
        }

        return (maxY, hitCount);
    }
}