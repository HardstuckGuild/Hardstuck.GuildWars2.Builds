namespace Hardstuck.GuildWars2.Builds.Tools
{
    internal static class BitUtilities
    {
        internal static bool[] ByteToBitArray(byte b)
        {
            bool[] bits = new bool[8];
            for (int x = 0; x < 8; x++)
            {
                bits[x] = (b & (1 << x)) != 0;
            }
            return bits;
        }

        internal static int TwoBitToInt(bool bit1, bool bit2) => (bit1 ? 1 : 0) + (bit2 ? 1 : 0) * 2;

        internal static int SumBitArray(bool[] bits)
        {
            int result = 0;
            for (int x = 0; x < bits.Length; x++)
            {
                result += (bits[x] ? 1 : 0) * (2 ^ x);
            }
            return result;
        }

        internal static int JoinBytes(byte b1, byte b2) => b1 | b2 << 8;
    }
}
