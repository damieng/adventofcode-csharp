using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

class Program
{
    const int packetHeaderSize = 6;

    record Packet(PacketType PacketType, long Value);

    enum PacketType
    {
        Sum,
        Product,
        Minimum,
        Maximum,
        Literal,
        GreaterThan,
        LessThan,
        EqualTo
    }

    static int versionTotal = 0;

    static void Main()
    {
        var result = Process(File.ReadAllText("input"));

        Console.WriteLine("Part1 = " + versionTotal);
        Console.WriteLine("Part2 = " + result.Value);

        Console.ReadKey();
    }

    static Packet Process(string input)
    {
        int bitIndex = 0;
        int bitLength = input.Length * 4;

        return CreatePacketsByBitCount(bitLength)[0];

        List<Packet> CreatePacketsByBitCount(int bitCount)
        {
            int bitEnd = bitIndex + bitCount;
            var packets = new List<Packet>();
            while (bitIndex < bitEnd && bitIndex + packetHeaderSize < bitLength)
                packets.Add(CreatePacket());
            return packets;
        }

        List<Packet> CreatePacketsByPacketCount(int packetCount)
        {
            var packets = new List<Packet>();
            while (packetCount-- > 0)
                packets.Add(CreatePacket());
            return packets;
        }

        List<Packet> CreatePackets(int lengthTypeId)
        {
            return lengthTypeId switch
            {
                0 => CreatePacketsByBitCount(GetInt(15)),
                1 => CreatePacketsByPacketCount(GetInt(11)),
                _ => throw new NotSupportedException(),
            };
        }

        Packet CreatePacket()
        {
            versionTotal += GetInt(3);
            var packetType = (PacketType)GetInt(3);

            if (packetType == PacketType.Literal)
            {
                var literal = GetLiteral();
                return new Packet(packetType, literal);
            }

            return new Packet(packetType, Calculate(packetType, CreatePackets(GetInt(1))));
        }

        long Calculate(PacketType packetType, List<Packet> packets)
        {
            return packetType switch
            {
                PacketType.Sum => packets.Sum(p => p.Value),
                PacketType.Product => packets.Count == 1 ? packets[0].Value : packets.Select(p => p.Value).Aggregate((a, b) => a * b),
                PacketType.Minimum => packets.Min(p => p.Value),
                PacketType.Maximum => packets.Max(p => p.Value),
                PacketType.GreaterThan => packets[0].Value > packets[1].Value ? 1 : 0,
                PacketType.LessThan => packets[0].Value < packets[1].Value ? 1 : 0,
                PacketType.EqualTo => packets[0].Value == packets[1].Value ? 1 : 0,
                _ => throw new NotImplementedException($"Unknown packet type {packetType}")
            };
        }

        int GetInt(int bitCount)
        {
            int result = 0;
            for (var i = 0; i < bitCount; i++)
            {
                var chrIdx = (bitIndex + i) / 4;
                var c = input[chrIdx..(chrIdx + 1)];
                var binary = int.Parse(c, NumberStyles.HexNumber);
                var mask = 8 >> ((bitIndex + i) % 4); // 4-bit nibble not 8-bit byte
                var set = (binary & mask) == 0 ? 0 : 1;
                result <<= 1;
                result += set;
            }
            bitIndex += bitCount;
            return result;
        }

        long GetLiteral()
        {
            bool isMore = false;
            long literal = 0;
            do
            {
                var b = GetInt(5);
                isMore = (b & 16) == 16;
                literal <<= 4;
                literal |= (long)b & 15;
            } while (isMore);
            return literal;
        }
    }
}