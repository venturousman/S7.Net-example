namespace HmiExample.Helpers
{
    public class CommonHelpers
    {
        /// <summary>
        /// Check if bit at pos position is 1 or 0
        /// </summary>
        /// <param name="n"></param>
        /// <param name="pos">position of bit need to check, must be greater than 0</param>
        /// <returns>true / false</returns>
        public static bool IsBitSet(byte n, int pos)
        {
            if (pos == 0) return false;
            return ((n >> (pos - 1)) & 1) != 0;

            //example:
            //        3210 <- standard pos
            //        4321 <- pos
            //n = 6 = 0110
            //pos 0 -> n >> -1 ???
            //pos 1 -> n >> 0 = 0110 & 0001 = 0000
            //pos 2 -> n >> 1 = 0011 & 0001 = 0001
            //pos 3 -> n >> 2 = 0001 & 0001 = 0001
            //pos 4 -> n >> 3 = 0000 & 0001 = 0000
        }
    }
}
