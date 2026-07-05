using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace Benchmarks
{
    /// <summary>
    /// Produces deterministic, fully-populated sample instances of the StackExchange
    /// benchmark models via reflection, so the same data feeds both MessagePack and
    /// Serde. Recursion is bounded so the mutually-referential model graph terminates.
    /// </summary>
    internal static class ModelData
    {
        private const int SafetyDepth = 16;
        private static readonly ConcurrentDictionary<Type, object> _cache = new();

        public static object Sample(Type t)
            => _cache.GetOrAdd(t, static type =>
            {
                // Stable seed per type so sizes are reproducible across runs.
                var rng = new Random(StableSeed(type.FullName!));
                return Fill(type, rng, 0, new HashSet<Type>())!;
            });

        public static T Sample<T>() => (T)Sample(typeof(T));

        private static int StableSeed(string s)
        {
            int h = 17;
            foreach (char c in s)
                h = unchecked(h * 31 + c);
            return h;
        }

        private static object? Fill(Type t, Random r, int depth, HashSet<Type> path)
        {
            var underlying = Nullable.GetUnderlyingType(t);
            if (underlying != null)
                return Fill(underlying, r, depth, path);

            if (t == typeof(string))
                return RandomString(r);
            if (t == typeof(bool))
                return r.Next(2) == 0;
            if (t == typeof(byte))
                return (byte)r.Next(256);
            if (t == typeof(sbyte))
                return (sbyte)(r.Next(256) - 128);
            if (t == typeof(short))
                return (short)r.Next(short.MinValue, short.MaxValue);
            if (t == typeof(ushort))
                return (ushort)r.Next(0, ushort.MaxValue);
            if (t == typeof(int))
                return r.Next();
            if (t == typeof(uint))
                return (uint)r.Next();
            if (t == typeof(long))
                return (long)r.Next() << 16 | (uint)r.Next();
            if (t == typeof(ulong))
                return (ulong)((long)r.Next() << 16 | (uint)r.Next());
            if (t == typeof(float))
                return (float)r.NextDouble() * 1000f;
            if (t == typeof(double))
                return r.NextDouble() * 1_000_000;
            if (t == typeof(decimal))
                return (decimal)(r.NextDouble() * 10000);
            if (t == typeof(char))
                return (char)('a' + r.Next(26));
            if (t == typeof(DateTime))
                return new DateTime(2000 + r.Next(20), 1 + r.Next(12), 1 + r.Next(28),
                    r.Next(24), r.Next(60), r.Next(60), DateTimeKind.Utc);
            if (t == typeof(Guid))
            {
                var bytes = new byte[16];
                r.NextBytes(bytes);
                return new Guid(bytes);
            }
            if (t.IsEnum)
            {
                var vals = Enum.GetValues(t);
                return vals.GetValue(r.Next(vals.Length))!;
            }

            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>))
            {
                var elem = t.GetGenericArguments()[0];
                var list = (IList)Activator.CreateInstance(t)!;
                // Break recursive cycles at the collection boundary: if the element
                // type is already being constructed on this path (or we hit the
                // safety depth), leave the list empty rather than recursing forever.
                bool recurse = depth < SafetyDepth && !PathContains(elem, path);
                int count = recurse ? 2 : 0;
                for (int i = 0; i < count; i++)
                {
                    var item = Fill(elem, r, depth + 1, path);
                    if (item != null)
                        list.Add(item);
                }
                return list;
            }

            if (t.IsClass)
            {
                // Never return null for a nested object property (Serde's serializer
                // does not tolerate a null nested object); break cycles via lists above.
                if (path.Contains(t) || depth >= SafetyDepth)
                    return Activator.CreateInstance(t);

                path.Add(t);
                var obj = Activator.CreateInstance(t);
                foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (!p.CanWrite || !p.CanRead || p.GetIndexParameters().Length > 0)
                        continue;
                    if (p.SetMethod is null || !p.SetMethod.IsPublic)
                        continue;
                    p.SetValue(obj, Fill(p.PropertyType, r, depth + 1, path));
                }
                path.Remove(t);
                return obj;
            }

            return null;
        }

        private static bool PathContains(Type elem, HashSet<Type> path)
        {
            var underlying = Nullable.GetUnderlyingType(elem) ?? elem;
            return path.Contains(underlying);
        }

        private static string RandomString(Random r)
        {
            int len = 4 + r.Next(16);
            Span<char> chars = stackalloc char[len];
            for (int i = 0; i < len; i++)
                chars[i] = (char)('a' + r.Next(26));
            return new string(chars);
        }
    }
}
