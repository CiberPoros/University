using System;
using System.Runtime.InteropServices;

namespace Generators.Programs.Gen
{
    public static class ArrayExtension
    {
        public static T[] GetRow<T>(this T[,] array, int row)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (!typeof(T).IsPrimitive)
                throw new InvalidOperationException("Not supported for managed types.");

            int cols = array.GetUpperBound(1) + 1;
            T[] result = new T[cols];

            int size;

            if (typeof(T) == typeof(bool))
                size = 1;
            else if (typeof(T) == typeof(char))
                size = 2;
            else
                size = Marshal.SizeOf<T>();

            Buffer.BlockCopy(array, row * cols * size, result, 0, cols * size);

            return result;
        }
    }
}
