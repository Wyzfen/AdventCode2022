using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2021
{
    public static class Utils
    {
        public static IEnumerable<T> FromFile<T>(string filename) =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => ValueFromString<T>(s));

        public static IEnumerable<(T, U)> FromFile<T, U>(string filename, string split = " ") =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => FromString<T, U>(s, split));

        public static IEnumerable<T> FromString<T>(string str, string split = " ") =>
           SplitClean(str, split).Select(s => ValueFromString<T>(s));

        public static (T, U) FromString<T, U>(string mystring, string split = " ")
        {
            var p = SplitClean(mystring, split);
            return (ValueFromString<T>(p[0]), ValueFromString<U>(p[1]));
        }

        private static string[] SplitClean(string mystring, string split = " ") => mystring.Trim().Split(new[] { split }, StringSplitOptions.RemoveEmptyEntries).ToArray();

        public static (T, U, V) FromString<T, U, V>(string mystring, string split = " ")
        {
            var p = SplitClean(mystring, split);
            return (ValueFromString<T>(p[0]), ValueFromString<U>(p[1]), ValueFromString<V>(p[2]));
        }

        public static T ValueFromString<T>(string mystring)
        {
            var foo = TypeDescriptor.GetConverter(typeof(T));
            return (T)(foo.ConvertFromInvariantString(mystring));
        }

        public static IEnumerable<int> IntsFromFile(string filename) =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => int.Parse(s));

        public static IEnumerable<long> LongsFromFile(string filename) =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => long.Parse(s));

        public static IEnumerable<int> IntsFromString(string input) =>
            input.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Select(s => int.Parse(s));

        public static string [] StringsFromFile(string filename) =>
            File.ReadAllLines(filename, Encoding.UTF8);

        public static IEnumerable<String> StringsFromString(string input) =>
            input.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

        // Returns an array of arrays from a CSV file
        public static T[][] FromCSVFile<T>(string filename, string split=",") =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => FromString<T>(s, split).ToArray()).ToArray();


        // Returns an array of arrays from a CSV file
        public static String[][] StringsFromCSVFile(string filename) =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => s.Split(',')).ToArray();

        // Returns an array of arrays from a CSV file
        public static int[][] IntsFromCSVFile(string filename) =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => s.Split(',').Select(i => int.Parse(i)).ToArray()).ToArray();

        public static long[][] LongsFromCSVFile(string filename) =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => s.Split(',').Select(i => long.Parse(i)).ToArray()).ToArray();

        // Returns an array of arrays from a CSV file
        public static String[][] StringsFromCSVString(string input) =>
            input.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Select(s => s.Split(',')).ToArray();

        public static IEnumerable<T> Generate<T>(T value, Func<T, T> func)
        {
            while (true)
            {
                yield return value;
                value = func(value);
            }
        }

        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int N)
        {
            return source.Skip(Math.Max(0, source.Count() - N));
        }

        public static int ToInt(this IEnumerable<int> source) =>
             source.Select(d => Math.Abs(d) % 10).Aggregate(0, (t, n) => t * 10 + n);

        public static Dictionary<string, string> SplitFromFile(string filename, char split = ')') =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(i => i.Split(split)).ToDictionary(s => s[1], s => s[0]);

        public static Dictionary<string, string> SplitFromString(string input, char split = ')') =>
            input.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Select(i => i.Split(split)).ToDictionary(s => s[1], s => s[0]);

        // Group consecutive lines into array, splitting into new group on blank line
        public static List<List<string>> MergeLines(string[] lines)
        {
            List<List<string>> output = new List<List<string>>();

            List<string> current = new List<string>();
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    output.Add(current);
                    current = new List<string>();
                }
                else
                {
                    current.Add(line.TrimEnd());
                }
            }

            if (current.Count() > 0) output.Add(current);

            return output;
        }

        /// <summary>
        /// Uses factorial notation to give all permutations of input set.
        /// if input set is in lexigraphical order, the results will be too.
        /// ie, pass in 012 (the lowest combination of 0,1 and 2) and the next one will be 021
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public static IEnumerable<List<int>> Permutations(IEnumerable<int> set)
        {
            int count = set.Count();
            ulong number = Factorial(count);
            int[] factors = new int[count];

            for (ulong n = 0; n < number; n++)
            {
                List<int> workingSet = new List<int>(set);
                List<int> result = new List<int>();

                for (int i = count - 1; i >= 0; i--)
                {
                    int j = factors[i];
                    result.Add(workingSet[j]);
                    workingSet.RemoveAt(j);
                }

                yield return result;

                for (int index = 1; index < count; index++)
                {
                    factors[index]++;
                    if (factors[index] <= index) break;

                    factors[index] = 0;
                }
            }
        }

        /// <summary>
        /// Select (choose) from (set) where order doesnt matter
        /// </summary>
        /// <param name="set"></param>
        /// <param name="choose"></param>
        /// <returns></returns>
        public static IEnumerable<List<T>> Combinations<T>(IEnumerable<T> input, int choose)
        {
            List<T> set = new List<T>(input);
            for (int i = 0; i < set.Count(); i++)
            {
                // only want 1 character, just return this one
                if (choose == 1)
                    yield return new List<T>(new T[] { set[i] });

                // want more than one character, return this one plus all combinations one shorter
                // only use characters after the current one for the rest of the combinations
                else
                    foreach (List<T> next in Combinations(set.GetRange(i + 1, set.Count - (i + 1)), choose - 1))
                    {
                        next.Add(set[i]);
                        yield return next;
                    }
            }
        }

        public static ulong Factorial(int n)
        {
            ulong value = 1;
            for (int i = 2; i <= n; i++)
            {
                value *= (ulong)i;
            }

            return value;
        }

        public static ulong GreatestCommonFactor(ulong a, ulong b)
        {
            while (b != 0)
            {
                ulong temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static ulong GCD(ulong a, ulong b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                {
                    a %= b;
                }
                else
                {
                    b %= a;
                }
            }

            return a == 0 ? b : a;
        }

        public static ulong LeastCommonMultiple(ulong a, ulong b) => (a / GreatestCommonFactor(a, b)) * b;

        public static ulong LeastCommonMultiple(params ulong[] f) => f.Aggregate((a, b) => LeastCommonMultiple(a, b));


        // Split string to tuples
        public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default;
            rest = list.Skip(1).ToList();
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default;
            second = list.Count > 1 ? list[1] : default;
            rest = list.Skip(2).ToList();
        }
    }

    public class MultiMap<V> : Dictionary<string, List<V>>
    {
        public void Add(string key, V value)
        {
            // Add a key.
            if (TryGetValue(key, out List<V> list))
            {
                list.Add(value);
            }
            else
            {
                Add(key, new List<V> { value });
            }
        }
    }

    [TypeConverter(typeof(Vector2Converter))]
    public record struct Vector2(int X, int Y);

    public class Vector2Converter : TypeConverter
    { 
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(String);
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string str)
            {
                (int x, int y) = Utils.FromString<int, int>(str, ",");
                return new Vector2(x, y);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

}
