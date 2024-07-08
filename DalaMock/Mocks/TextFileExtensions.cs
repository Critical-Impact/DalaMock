namespace DalaMock.Core.Mocks;

using System;
using System.Runtime.CompilerServices;
using Dalamud.Memory;
using Lumina.Data.Files;

public static class TextFileExtensions
{
    /// <summary>Determines if the given data is possibly a <see cref="TexFile"/>.</summary>
    /// <param name="data">The data.</param>
    /// <returns><c>true</c> if it should be attempted to be interpreted as a <see cref="TexFile"/>.</returns>
    public static unsafe bool IsPossiblyTexFile2D(this ReadOnlySpan<byte> data)
    {
        if (data.Length < Unsafe.SizeOf<TexFile.TexHeader>())
        {
            return false;
        }

        fixed (byte* ptr = data)
        {
            ref readonly var texHeader = ref MemoryHelper.Cast<TexFile.TexHeader>((nint)ptr);
            if ((texHeader.Type & TexFile.Attribute.TextureTypeMask) != TexFile.Attribute.TextureType2D)
            {
                return false;
            }

            if (!Enum.IsDefined(texHeader.Format))
            {
                return false;
            }

            if (texHeader.Width == 0 || texHeader.Height == 0)
            {
                return false;
            }
        }

        return true;
    }
}