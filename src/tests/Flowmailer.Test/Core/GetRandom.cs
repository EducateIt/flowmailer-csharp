using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Flowmailer.Test.Core
{
    [SuppressMessage("ReSharper", "MethodOverloadWithOptionalParameter")]
    public static class GetRandom
    {
        private const double EPSILON = .0000001;

        private static int _nextId = 1000000000;

        private static string _stringPrefix = "¿";

        public static Random Random { get; set; } = new Random();

        public static bool Bool()
        {
            var value = Random.Next();

            return value % 2 == 0;
        }

        public static int Int32()
        {
            return Random.Next();
        }

        public static int Int32(int maxValue)
        {
            return Random.Next(maxValue);
        }

        public static int Int32(int minValue, int maxValue)
        {
            return Random.Next(minValue, maxValue);
        }

        public static int Int32(int minValue = 0, int maxValue = int.MaxValue, int? exclude = null)
        {
            int result;

            do
            {
                result = Random.Next(minValue, maxValue);
            } while (exclude.HasValue && result == exclude);

            return result;
        }

        public static long Int64(long maxValue)
        {
            return Int64(0, maxValue, null);
        }

        public static long Int64(long minValue, long maxValue)
        {
            return Int64(minValue, maxValue, null);
        }

        public static long Int64(long minValue = 0, long maxValue = long.MaxValue, long? exclude = null)
        {
            long result;

            do
            {
                result = minValue + Convert.ToInt64(Convert.ToDouble(maxValue - minValue) * Double());
            } while (exclude.HasValue && result == exclude);

            return result;
        }

        public static double Double()
        {
            return Random.NextDouble();
        }

        public static double Double(double maxValue)
        {
            return Double(0, maxValue, null);
        }

        public static double Double(double minValue, double maxValue)
        {
            return Double(minValue, maxValue, null);
        }

        public static double Double(double minValue = 0, double maxValue = double.MaxValue, double? exclude = null)
        {
            double result;

            do
            {
                result = minValue + (maxValue - minValue) * Double();
            } while (exclude.HasValue && Math.Abs(exclude.Value - result) < EPSILON);

            return result;
        }

        /// <summary>Returns a random <see cref="System.DateTime" /> value less than the specified maximum value.</summary>
        /// <param name="maxValue">The maximum desired value.</param>
        /// <returns>A <see cref="System.DateTime" /> value less than <paramref name="maxValue" />.</returns>
        public static DateTime DateTime(DateTime maxValue)
        {
            return DateTime(System.DateTime.MinValue, maxValue);
        }

        /// <summary>Returns a random <see cref="System.DateTime" /> value within the specified range.</summary>
        /// <param name="minValue">The minimum desired value.</param>
        /// <param name="maxValue">The maximum desired value.</param>
        /// <returns>
        ///     A <see cref="System.DateTime" /> value greater than or equal to <paramref name="minValue" />, less than
        ///     <paramref name="maxValue" /> .
        /// </returns>
        public static DateTime DateTime(DateTime minValue, DateTime maxValue)
        {
            return DateTime(minValue, maxValue, null);
        }

        /// <summary>Returns a random <see cref="System.DateTime" /> value within the specified range.</summary>
        /// <param name="minValue">The minimum desired value.</param>
        /// <param name="maxValue">The maximum desired value.</param>
        /// <param name="exclude">
        ///     A value that should not be returned, used when you want to get a value other than one you already
        ///     have.
        /// </param>
        /// <returns>
        ///     A <see cref="System.DateTime" /> value greater than or equal to <paramref name="minValue" />, less than
        ///     <paramref name="maxValue" /> , and not equal to <paramref name="exclude" />.
        /// </returns>
        public static DateTime DateTime(DateTime minValue, DateTime maxValue, DateTime? exclude = null)
        {
            DateTime result;
            do
            {
                result = new DateTime(Int64(minValue.Ticks, maxValue.Ticks));
            } while (exclude.HasValue && result == exclude);

            return result;
        }

        /// <summary>
        ///     Returns a random date from <paramref name="daysInThePast" /> days ago to <paramref name="daysInTheFuture" />
        ///     from now.
        /// </summary>
        /// <param name="daysInThePast">The maximum number of days in the past.</param>
        /// <param name="daysInTheFuture">The maximum number of days in the future.</param>
        /// <returns>A random date from <paramref name="daysInThePast" /> days ago to <paramref name="daysInTheFuture" /> from now.</returns>
        public static DateTime DateTime(int daysInThePast, int daysInTheFuture)
        {
            return System.DateTime.Now.AddDays(Int32(-daysInThePast, daysInTheFuture));
        }

        public static string String(int minLength = 1, int maxLength = 50, string exclude = null)
        {
            string result;

            do
            {
                var length = Int32(minLength, maxLength);
                var prefix = GetStringPrefix();
                var prefixLength = prefix?.Length ?? 0;

                var sb = new StringBuilder(prefix ?? string.Empty, length);
                for (var index = 0; index < length - prefixLength; index++)
                {
                    sb.Append(Convert.ToChar(Int32(65, 90)));
                }

                result = sb.ToString(0, length);
            } while (!string.IsNullOrEmpty(exclude) && result == exclude);

            return result;
        }

        public static string GetStringPrefix()
        {
            return _stringPrefix;
        }
    }
}