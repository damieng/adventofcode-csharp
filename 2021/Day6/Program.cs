using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        var startingData = File
            .ReadAllLines("input")[0]
            .Split(',')
            .Select(i => int.Parse(i))
            .ToArray();

        Console.WriteLine("Part1 = " + SimulateFish(startingData, 80));
        Console.WriteLine("Part2 = " + SimulateFish(startingData, 256));
    }

    private static long SimulateFish(int[] startingData, int days)
    {
        var fishByDay = new long[9];

        foreach (var fish in startingData)
            fishByDay[fish]++;

        for (int day = 1; day <= days; day++)
        {
            var fishSpawning = fishByDay[0];
            fishByDay[0] = fishByDay[1];
            fishByDay[1] = fishByDay[2];
            fishByDay[2] = fishByDay[3];
            fishByDay[3] = fishByDay[4];
            fishByDay[4] = fishByDay[5];
            fishByDay[5] = fishByDay[6];
            fishByDay[6] = fishSpawning + fishByDay[7];
            fishByDay[7] = fishByDay[8];
            fishByDay[8] = fishSpawning;
        }

        return fishByDay.Sum();
    }
}