// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ResumableStream
{
    [PublicAPI]
    public class ReadableResumableStream : ResumableStream
    {
        #region public

        public ReadableResumableStream(IStreamProvider streamProvider) : base(streamProvider)
        {
        }

        #endregion

        #region override

        public override void Flush()
        {
            // Nothing to do
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            try
            {
                var readCount = await UnderlyingStream.ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
                _position += readCount;
                return readCount;
            }
            catch (Exception exception)
            {
                if (!ErrorIsRecoverable(exception))
                    throw new RecoverableStreamException("Unrecoverable read error occured");
                await RecoverAsync(OperationType.Read, count, cancellationToken).ConfigureAwait(false);
                return await ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
            }
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
            if (!UnderlyingStream.CanRead)
                throw new InvalidOperationException("Underlying stream is not readable");
        }

        protected override async Task SetUnderlyingStreamAsync(CancellationToken cancellationToken)
        {
            await base.SetUnderlyingStreamAsync(cancellationToken).ConfigureAwait(false);
            if (!UnderlyingStream.CanRead)
                throw new InvalidOperationException("Underlying stream is not readable");
        }

        #endregion

        #region private

        private long _position;

        #endregion
    }
}
