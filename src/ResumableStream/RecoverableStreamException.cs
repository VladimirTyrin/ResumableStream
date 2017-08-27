// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using JetBrains.Annotations;

namespace ResumableStream
{
    [PublicAPI]
    public class RecoverableStreamException : Exception
    {
        public RecoverableStreamException() { }

        public RecoverableStreamException(string message) : base(message) { }

        public RecoverableStreamException(string message, Exception innerException) : base(message, innerException) { }
    }
}
