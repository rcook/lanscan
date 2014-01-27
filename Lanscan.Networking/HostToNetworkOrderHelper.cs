namespace Lanscan.Networking
{
    using System;

    public static class HostToNetworkOrderHelper
    {
        public static short Convert(short value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            var result = BitConverter.ToInt16(bytes, 0);
            return result;
        }

        public static int Convert(int value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            var result = BitConverter.ToInt32(bytes, 0);
            return result;
        }

        public static long Convert(long value)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            var result = BitConverter.ToInt64(bytes, 0);
            return result;
        }
    }
}
