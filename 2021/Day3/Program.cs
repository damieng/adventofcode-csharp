using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    enum Direction { forward, up, down };

    struct Movement
    {
        public Direction Direction;
        public int Quantity;
    }

    static void Main()
    {
        var input = File
            .ReadAllLines("input");

        Console.WriteLine("Part 1 = " + Part1(input));
        Console.WriteLine("Part 2 = " + Part2(input));
    }

    static ulong Part1(string[] input)
    {
        var zeroBits = new int[12];
        var oneBits = new int[12];

        foreach (var line in input)
        {
            for (int i = 0; i < 12; i++)
            {
                if (line[i] == '0')
                    zeroBits[i]++;
                else
                    oneBits[i]++;
            }
        }

        ulong gamma = 0;
        ulong epsilon = 0;

        for (int i = 0; i < 12; i++)
        {
            gamma <<= 1;
            epsilon <<= 1;

            if (zeroBits[i] > oneBits[i])
            {
                epsilon++;
            }
            else
            {
                gamma++;
            }
        }

        return gamma * epsilon;
    }

    static ulong Part2(string[] input)
    {
        var (co2, oxygen) = SplitLists(input, 0);

        int position = 1;
        while (oxygen.Count > 1)
        {
            (_, oxygen) = SplitLists(oxygen, position++);
        }

        position = 1;
        while (co2.Count > 1)
        {
            (co2, _) = SplitLists(co2, position++);
        }

        var co2Value = Convert.ToUInt32(co2[0],2);
        var oxygenValue = Convert.ToUInt32(oxygen[0], 2);

        return co2Value * oxygenValue;
    }

    private static (List<string>, List<string>) SplitLists(IEnumerable<string> input, int index)
    {
        var (zeroes, ones) = SplitListByDigit(input, index); 
        
        return zeroes.Count > ones.Count ? (zeroes, ones) : (ones, zeroes);
    }

    private static (List<string>, List<string>) SplitListByDigit(IEnumerable<string> input, int index)
    {
        var zeroStart = new List<string>();
        var oneStart = new List<string>();

        foreach (var line in input)
        {
            if (line[index] == '0')
                zeroStart.Add(line);
            else
                oneStart.Add(line);
        }

        return (zeroStart, oneStart);
    }
}
