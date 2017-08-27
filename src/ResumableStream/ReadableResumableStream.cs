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
        #region public

        public ReadableResumableStream(StreamProvider streamProvider) : base(streamProvider)
        {
        }

        #endregion

        #region override

        public override void Flush()
        {
            // Nothing to do
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            try
            {
                var readCount = UnderlyingStream.Read(buffer, offset, count);
                _position += readCount;
                return readCount;
            }
            catch (Exception exception)
            {
                if (!ErrorIsRecoverable(exception))
                    throw new RecoverableStreamException("Unrecoverable read error occured");
                Recover(OperationType.Read, count);
                return Read(buffer, offset, count);
            }
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new InvalidOperationException("Seek is not supported");

        public override void Write(byte[] buffer, int offset, int count) => throw new InvalidOperationException("Cannot write to readable stream");

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => 0;
        public override long Position
        {
            get { return _position; }
            set { throw new InvalidOperationException("Seek is not supported"); }
        }

        #endregion

        #region protected

        protected override void SetUnderlyingStream()
        {
            base.SetUnderlyingStream();
            if (! UnderlyingStream.CanRead)
                throw new InvalidOperationException("Underlying stream is not readable");
        }

        #endregion

        #region private

        private long _position;

        #endregion
    }
}
