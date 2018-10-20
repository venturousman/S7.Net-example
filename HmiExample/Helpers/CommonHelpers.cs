namespace HmiExample.Helpers
{
    public class CommonHelpers
    {
        public static bool IsBitSet(byte n, int pos)
        {
            // return (b & (1 << pos)) != 0;
            return ((n >> (pos - 1)) & 1) != 0;
        }
    }
}
