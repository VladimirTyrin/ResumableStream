// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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

		protected ResumableStream(IStreamProvider streamProvider)
		{
			StreamProvider = streamProvider ?? throw new ArgumentNullException();
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
			UnderlyingStream = StreamProvider.GetStream(Position);

			if (UnderlyingStream == null)
				throw new InvalidOperationException("Failed to retrieve underlying stream");
		}

		protected virtual async Task SetUnderlyingStreamAsync(CancellationToken cancellationToken)
		{
			TryDisposeUnderlyingStream();
			UnderlyingStream = await StreamProvider.GetStreamAsync(Position, cancellationToken).ConfigureAwait(false);

			if (UnderlyingStream == null)
				throw new InvalidOperationException("Failed to retrieve underlying stream");
		}

		protected virtual bool ErrorIsRecoverable(Exception exception) => true;

		protected void OnRecoverableError(StreamErrorEventArgs e)
		{
			RecoverableError?.Invoke(this, e);
		}

		protected void Recover(int requestedCount)
		{
			try
			{
				SetUnderlyingStream();
			}
			catch (Exception exception)
			{
				if (!ErrorIsRecoverable(exception))
					throw new ResumableStreamException("Failed to recover stream", exception);

				OnRecoverableError(new StreamErrorEventArgs(OperationType.Recovery, Position, requestedCount, exception));
				Recover(requestedCount);
			}
		}

		protected async Task RecoverAsync(int requestedCount, CancellationToken cancellationToken)
		{
			try
			{
				await SetUnderlyingStreamAsync(cancellationToken).ConfigureAwait(false);
			}
			catch (Exception exception)
			{
				if (ErrorIsRecoverable(exception))
				{
					OnRecoverableError(new StreamErrorEventArgs(OperationType.Recovery, Position, requestedCount, exception));
					await RecoverAsync(requestedCount, cancellationToken).ConfigureAwait(false);
					return;
				}
				throw new ResumableStreamException("Failed to recover stream", exception);
			}
		}

		protected readonly IStreamProvider StreamProvider;
		protected Stream UnderlyingStream;

		#endregion
	}
}
