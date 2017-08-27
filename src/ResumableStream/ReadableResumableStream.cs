// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.IO;
using JetBrains.Annotations;

namespace ResumableStream
{
    [PublicAPI]
    public class ReadableResumableStream : ResumableStream
    {
        public ReadableResumableStream(StreamProvider streamProvider) : base(streamProvider)
        {
        }

        #region override

        public override void Flush()
        {
            // Nothing to do
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new InvalidOperationException("Seek is not supported");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException("Cannot write to readable stream");
        }

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => 0;
        public override long Position
        {
            get => 0;
            set => throw new InvalidOperationException("Seek is not supported");
        }

        #endregion
    }
}
