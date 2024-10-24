//=====================================================
// - FileName: SRandom.cs
// - AuthorName: ZhanJianhua
// - CreateTime: 2024/10/21 14:55:47
// - Description: 自定义随机数
//======================================================

using System;

namespace GameLogic
{
    public class SRandom
    {
        public int count = 0;

        public ulong randSeed = 1;
        public SRandom(long seed)
        {
            count = 0;
            randSeed = Convert.ToUInt64(seed);
        }

        public uint Next()
        {
            randSeed = randSeed * 1103515245 + 12345;
            return (uint)(randSeed / 65536);
        }

        // range:[0 ~(max-1)]
        public uint Next(uint max)
        {
            if (max == 0)
                return 0;
            return Next() % max;
        }

        // range:[min~(max-1)]
        public uint Range(uint min, uint max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException("minValue", string.Format("'{0}' cannot be greater than {1}.", min, max));

            uint num = max - min;
            return Next(num) + min;
        }

        public int Next(int max)
        {
            if (max == 0)
                return 0;
            return (int)(Next() % max);
        }

        public int Range(int min, int max)
        {
            count++;

            if (min > max)
                throw new ArgumentOutOfRangeException("minValue", string.Format("'{0}' cannot be greater than {1}.", min, max));

            int num = max - min;

            return Next(num) + min;
        }
        
    }

}
