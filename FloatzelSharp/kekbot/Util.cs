using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSharpPlus.Entities;

namespace FloatzelSharp.kekbot
{
    static class Util
    {

        internal static readonly Random Rng = new Random();

        internal static T RandomElement<T>(this IEnumerable<T> list) => list.ElementAt(Rng.Next(list.Count()));

        /// <summary>
        /// Appends copies of the specified strings followed by the default line terminator
        /// to the end of the current System.Text.StringBuilder object.
        /// </summary>
        /// <param name="s">The current System.Text.StringBuilder object/param>
        /// <param name="lines">The strings to append.</param>
        /// <returns>A reference to this instance after the append operation has completed.</returns>
        internal static StringBuilder AppendLines(this StringBuilder s, IEnumerable<string> lines)
        {
            foreach (var line in lines) s.AppendLine(line);
            return s;
        }

        internal static T ToNullableClass<T>(this Optional<T> opt)
            where T : class => opt.HasValue ? opt.Value : null;

        internal static T? ToNullable<T>(this Optional<T> opt)
            where T : struct => opt.HasValue ? (T?)opt.Value : null;

        internal static IEnumerable<int> Range(int start = 0, int end = int.MaxValue, int step = 1)
        {
            for (int n = start; n < end; n += step)
            {
                yield return n;
            }
        }

        internal static int ParseInt(string intStr, int fallback) => int.TryParse(intStr, out var n) ? n : fallback;

        internal static void Panic(string msg = "")
        {
            Console.Error.WriteLine(msg);
            Environment.Exit(1);
        }

        // stolen from newer release of kekbotSharp: shuffle shit for the music player
        public static void Shuffle<T>(this IList<T> list, Randumb? rng = null) {
            rng ??= Randumb.Instance;
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static String PrintTimeSpan(TimeSpan time) {
            return $"{(time.Days > 0 ? $"{time.Days} Days, " : "")}{(time.Hours > 0 ? $"{time.Hours} Hours, " : "")}{time.Minutes} Minute{(time.Minutes > 1 ? "s" : "")}, and {time.Seconds} Second{(time.Seconds > 1 ? "s" : "")}.";
        }

    }
}