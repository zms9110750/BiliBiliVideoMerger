using System.Collections.Immutable;
using System.IO;
using System.Runtime.CompilerServices;

namespace Canalot.Utils;

public static class UtilExtension
{

    private static readonly ImmutableHashSet<char> _invalidFileNameChars = Path.GetInvalidFileNameChars().ToImmutableHashSet();

    public static string ToSafeFileName(this string s, char replacement = '_')
    {
        if (string.IsNullOrEmpty(s))
            return s;

        return string.Create(s.Length, (s, replacement), static (span, state) =>
        {
            state.s.CopyTo(span);
            foreach (ref var item in span)
            {
                if (_invalidFileNameChars.Contains(item))
                {
                    item = state.replacement;
                }
            }
        });
    }

    public static string ToString<T>(this IEnumerable<T> values, string separator = ", ")
    { 
        return separator switch
        {
            "" or null => string.Concat(values),
            { Length: 1 } => string.Join(separator[0], values),
            _ => string.Join(separator, values),
        };
    } 
 
}
