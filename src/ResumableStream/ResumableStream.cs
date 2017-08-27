// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.IO;
using JetBrains.Annotations;

namespace ResumableStream
{
    /// <summary>
    ///     Represents an abstraction of a stream that can be transparently resumed in case of some (e.g. IO) errors
    /// </summary>
    [PublicAPI]
    public abstract class ResumableStream : Stream
    {
        #region public

        public event EventHandler<StreamErrorEventArgs> RecoverableError; 

        #endregion

        #region override
        public abstract override void Flush();

        public abstract override int Read(byte[] buffer, int offset, int count);

        public abstract override long Seek(long offset, SeekOrigin origin);

        public override void SetLength(long value)
        {
            throw new InvalidOperationException($"Cannot set length of {nameof(ResumableStream)}");
        }

        public abstract override void Write(byte[] buffer, int offset, int count);

        public abstract override bool CanRead { get; }
        public abstract override bool CanSeek { get; }
        public abstract override bool CanWrite { get; }
        public abstract override long Length { get; }
        public abstract override long Position { get; set; }
        #endregion

        #region protected

        protected ResumableStream(StreamProvider streamProvider)
        {
            if (streamProvider == null)
                throw new ArgumentNullException();

            StreamProvider = streamProvider;
            // ReSharper disable once VirtualMemberCallInConstructor
            SetUnderlyingStream();
        }

        protected override void Dispose(bool unused) => TryDisposeUnderlyingStream();

        protected void TryDisposeUnderlyingStream()
        {
            try
            {
                UnderlyingStream?.Dispose();
            }
            catch (Exception)
            {
                // Well, at least we tried
            }
        }

        protected virtual void SetUnderlyingStream()
        {
            TryDisposeUnderlyingStream();
            UnderlyingStream = StreamProvider(Position);

            if (UnderlyingStream == null)
                throw new InvalidOperationException("Failed to retrieve underlying stream");
        }

        protected bool ErrorIsRecoverable(Exception exception) => true;

        protected void OnRecoverableError(StreamErrorEventArgs e)
        {
            RecoverableError?.Invoke(this, e);
        }

        protected void Recover(OperationType operationType, int requestedCount)
        {
            try
            {
                SetUnderlyingStream();
            }
            catch (Exception exception)
            {
                if (ErrorIsRecoverable(exception))
                {
                    OnRecoverableError(new StreamErrorEventArgs(operationType, Position, requestedCount, exception));
                    Recover(operationType, requestedCount);
                }
                throw new RecoverableStreamException("Failed to recover stream", exception);
            }
        }

        protected readonly StreamProvider StreamProvider;
        protected Stream UnderlyingStream;

        #endregion
    }
}
