
using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Serde.IO;

internal interface IByteReader
{
    public const short EndOfStream = -1;

    /// <summary>
    /// Reads a byte without advancing the stream, or returns <see cref="EndOfStream"/> if the end
    /// of the stream has been reached.
    /// </summary>
    short Peek();

    /// <summary>
    /// Reads a byte and advances the stream, or returns <see cref="EndOfStream"/> if the end of the
    /// stream has been reached.
    /// </summary>
    short Next();

    /// <summary>
    /// Advances the stream by <paramref name="count"/> bytes. The caller should ensure there is
    /// enough remaining data in the stream.
    /// </summary>
    void Advance(int count = 1);
}