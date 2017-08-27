// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using JetBrains.Annotations;

namespace ResumableStream
{
    [PublicAPI]
    public enum OperationType
    {
        Undefined,
        Read,
        Write
    }
}
