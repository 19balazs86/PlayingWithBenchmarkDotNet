using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PlayingWithBenchmarkDotNet.Code;

public static class CollectionExtensions
{
    public static bool ContainsDuplicates<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable.GetType() == typeof(T[]))
            return ContainsDuplicates(Unsafe.As<T[]>(enumerable));

        if (enumerable.GetType() == typeof(List<T>))
            return ContainsDuplicates<T>(CollectionsMarshal.AsSpan(Unsafe.As<List<T>>(enumerable)));

        var knownElements = new HashSet<T>();

        foreach (T element in enumerable)
        {
            if (!knownElements.Add(element))
                return true;
        }

        return false;
    }

    public static bool ContainsDuplicates<T>(this ReadOnlySpan<T> span)
    {
        var knownElements = new HashSet<T>();

        foreach (ref readonly T element in span)
        {
            if (!knownElements.Add(element))
            {
                return true;
            }
        }

        return false;
    }
}
