using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode2022
{
    public static class Utils
    {
        public static IEnumerable<T> FromFile<T>(string filename) =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => ValueFromString<T>(s));

        public static IEnumerable<(T, U)> FromFile<T, U>(string filename, string split = " ") =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(s => FromString<T, U>(s, split));

        public static IEnumerable<T> FromString<T>(string str, params string[] split) =>
           SplitClean(str, split).Select(s => ValueFromString<T>(s));

        public static (T, U) FromString<T, U>(string mystring, string split = " ")
        {
            var p = SplitClean(mystring, split);
            return (ValueFromString<T>(p[0]), ValueFromString<U>(p[1]));
        }

        private static string[] SplitClean(string mystring, params string [] split) => mystring.Trim().Split(split, StringSplitOptions.RemoveEmptyEntries).ToArray();

        public static (T, U, V) FromString<T, U, V>(string mystring, params string[] split)
        {
            var p = SplitClean(mystring, split);
            return (ValueFromString<T>(p[0]), ValueFromString<U>(p[1]), ValueFromString<V>(p[2]));
        }

        public static (T, U, V, W) FromString<T, U, V, W>(string mystring, params string [] split)
        {
            var p = SplitClean(mystring, split);
            return (ValueFromString<T>(p[0]), ValueFromString<U>(p[1]), ValueFromString<V>(p[2]), ValueFromString<W>(p[3]));
        }

        public static (T, U, V, W, X) FromString<T, U, V, W, X>(string mystring, params string[] split)
        {
            var p = SplitClean(mystring, split);
            return (ValueFromString<T>(p[0]), ValueFromString<U>(p[1]), ValueFromString<V>(p[2]), ValueFromString<W>(p[3]), ValueFromString<X>(p[4]));
        }

        public static (T, U, V, W, X, Y) FromString<T, U, V, W, X, Y>(string mystring, params string[] split)
        {
            var p = SplitClean(mystring, split);
            return (ValueFromString<T>(p[0]), ValueFromString<U>(p[1]), ValueFromString<V>(p[2]), ValueFromString<W>(p[3]), ValueFromString<X>(p[4]), ValueFromString<Y>(p[5]));
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

        public static int ToInt(this IEnumerable<int> source) =>
             source.Select(d => Math.Abs(d) % 10).Aggregate(0, (t, n) => t * 10 + n);

        public static Dictionary<string, string> SplitFromFile(string filename, char split = ')') =>
            File.ReadAllLines(filename, Encoding.UTF8).Select(i => i.Split(split)).ToDictionary(s => s[1], s => s[0]);

        public static Dictionary<string, string> SplitFromString(string input, char split = ')') =>
            input.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).Select(i => i.Split(split)).ToDictionary(s => s[1], s => s[0]);

        // Group consecutive lines into array, splitting into new group on blank line
        public static List<List<string>> MergeLines(IEnumerable<string> lines)
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

        public static string FromAlphabet6(string[] input, char set = '#', char unset = '.')
        {
            if (input == null || input.Length != 6) return "";
            
            int length = input[0].Length;
            if (!input.Select(l => l.Length).All(l => l == input[0].Length)) return "";

            Dictionary<string, string> alphabet6 = new()
                        {
                            {".##.\n#..#\n#..#\n####\n#..#\n#..#", "A"},
                            {"###.\n#..#\n###.\n#..#\n#..#\n###.", "B"},
                            {".##.\n#..#\n#...\n#...\n#..#\n.##.", "C"},
                            {"####\n#...\n###.\n#...\n#...\n####", "E"},
                            {"####\n#...\n###.\n#...\n#...\n#...", "F"},
                            {".##.\n#..#\n#...\n#.##\n#..#\n.###", "G"},
                            {"#..#\n#..#\n####\n#..#\n#..#\n#..#", "H"},
                            {".###\n..#.\n..#.\n..#.\n..#.\n.###", "I"},
                            {"..##\n...#\n...#\n...#\n#..#\n.##.", "J"},
                            {"#..#\n#.#.\n##..\n#.#.\n#.#.\n#..#", "K"},
                            {"#...\n#...\n#...\n#...\n#...\n####", "L"},
                            {".##.\n#..#\n#..#\n#..#\n#..#\n.##.", "O"},
                            {"###.\n#..#\n#..#\n###.\n#...\n#...", "P"},
                            {"###.\n#..#\n#..#\n###.\n#.#.\n#..#", "R"},
                            {".###\n#...\n#...\n.##.\n...#\n###.", "S"},
                            {"#..#\n#..#\n#..#\n#..#\n#..#\n.##.", "U"},
                            {"#...\n#...\n.#.#\n..#.\n..#.\n..#.", "Y"},
                            {"####\n...#\n..#.\n.#..\n#...\n####", "Z"}
                        };

            string result = "";

            for(int x = 0; x < length; x += 5)
            {
                var character = string.Join('\n', input.Select(l => l.Substring(x, 4)));
                
                result += alphabet6.TryGetValue(character, out string output) ? output : "?";
            }

            return result;
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
    public record struct Vector2(int X, int Y)
    {
        public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);

        public static Vector2 One { get; } = new Vector2(1, 1);
        public static Vector2 Zero { get; } = new Vector2(0, 0);
    }

    public class Vector2Converter : TypeConverter
    { 
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(String);
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string str)
            {
                if (str.Contains('=')) // x = 1, y = 2
                {
                    (_, int x, _, int y) = Utils.FromString<string, int, string, int>(str, "=", ",");
                    return new Vector2(x, y);
                }
                else // 1, 2
                {
                    (int x, int y) = Utils.FromString<int, int>(str, ",");
                    return new Vector2(x, y);
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    [TypeConverter(typeof(Vector3Converter))]
    public record struct Vector3(int X, int Y, int Z)
    {
        public static Vector3 operator +(Vector3 a, Vector3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        
    }

    public class Vector3Converter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => sourceType == typeof(String);
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string str)
            {
                if (str.Contains('=')) // x = .., y = .., z = ..
                {
                    (_, int x, _, int y, _, int z) = Utils.FromString<string, int, string, int, string, int>(str, "=", ",");
                    return new Vector3(x, y, z);
                }
                else // 1, 2
                {
                    (int x, int y, int z) = Utils.FromString<int, int, int>(str, ",");
                    return new Vector3(x, y, z);
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    public record struct Rectangle(int X, int Y, int Width, int Height)
    {
        public static Rectangle Bounds(IEnumerable<Vector2> points)
        {
            int minX = points.Min(e => e.X);
            int maxX = points.Max(e => e.X);
            int minY = points.Min(e => e.Y);
            int maxY = points.Max(e => e.Y);

            return new Rectangle(minX, minY, (maxX - minX + 1), (maxY - minY + 1));
        }

        public Vector2 Size => new (Width, Height);
    }

    public static class VectorExtensions
    {
        public static Rectangle Bounds(this IEnumerable<Vector2> points) => Rectangle.Bounds(points);
    }

    public static class LinqExtensions
    {        
        public static IEnumerable<IEnumerable<T>> Sliding<T>(this IEnumerable<T> input, int length) => Enumerable.Range(length, input.Count() - length).Select(i => input.Skip(i - length).Take(length));

        public static int FirstIndex<T>(this IEnumerable<T> input, Func<T, bool> predicate) => input.Select((value, index) => (value, index)).Where(p => predicate(p.value)).Select(p => p.index).First();


        /// <summary>
        /// Uses factorial notation to give all permutations of input set.
        /// if input set is in lexigraphical order, the results will be too.
        /// ie, pass in 012 (the lowest combination of 0,1 and 2) and the next one will be 021
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public static IEnumerable<List<int>> Permutations(this IEnumerable<int> set)
        {
            int count = set.Count();
            ulong number = Utils.Factorial(count);
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
        public static IEnumerable<List<T>> Combinations<T>(this IEnumerable<T> input, int choose)
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

        public static IEnumerable<T> Generate<T>(T value, Func<T, T> func)
        {
            while (true)
            {
                yield return value;
                value = func(value);
            }
        }
    }
}
