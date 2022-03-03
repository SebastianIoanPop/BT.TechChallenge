using System;
using System.Collections.Generic;

namespace BT.TechTest.OTP.Generator.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// Using the Fisher Yates Shuffle.
        /// An advanced and more secure version would be replacing Random with RNGCryptoServiceProvider.
        /// (Which is why I opted for copying it as an extension instead of finding a package that does this for me)
        /// 
        /// https://en.wikipedia.org/wiki/Fisher–Yates_shuffle
        /// https://www.dotnetperls.com/fisher-yates-shuffle
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void FisherYatesShuffle<T>(this IList<T> list)
        {
            Random random = new Random();

            int n = list.Count;
            for (int i = 0; i < (n - 1); i++)
            {
                int r = i + random.Next(n - i);
                T t = list[r];
                list[r] = list[i];
                list[i] = t;
            }
        }
    }
}
