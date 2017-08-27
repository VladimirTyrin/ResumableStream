// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.IO;

namespace ResumableStream
{
    /// <summary>
    ///     Method used to reacquire stream in repeatable streams
    /// </summary>
    /// <param name="position">Position in underlying stream that should be set before return</param>
    /// <returns>Non-null stream</returns>
    public delegate Stream StreamProvider(long position);
}
