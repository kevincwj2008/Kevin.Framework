using System;

namespace Kevin.Framework.Infrastructure
{
    /// <summary>
    /// 随机数
    /// </summary>
    public static class RandomHelper
    {
        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="count">随机位数，最大九位</param>
        /// <returns></returns>
        public static int GenerateRandom(int count)
        {
            if (count <= 0)
            {
                return 0;
            }
            if (count > 9)
            {
                throw new Exception("随机数超出最大值");
            }
            Random r = new Random();
            return r.Next((int)Math.Pow(10, (count - 1)), (int)Math.Pow(10, count) - 1);
        }
    }
}
