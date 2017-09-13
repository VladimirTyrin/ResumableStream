// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System.IO;
using ResumableStream.Tests.Exceptions;
using ResumableStream.Tests.Mocks;

namespace ResumableStream.Tests.Streams
{
	public class StartFailingStream : Stream
	{
		public StartFailingStream(Stream underlyingStream, int failsLeft = 1)
		{
			_underlyingStream = underlyingStream;
			_failsLeft = failsLeft;
		}

		#region Stream

		public override void Flush() => _underlyingStream.Flush();
		public override long Seek(long offset, SeekOrigin origin) => _underlyingStream.Seek(offset, origin);
		public override void SetLength(long value) => _underlyingStream.SetLength(value);

		public override int Read(byte[] buffer, int offset, int count)
		{
			FailIfSomeFailsLeft();

			return _underlyingStream.Read(buffer, offset, count);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			FailIfSomeFailsLeft();

			_underlyingStream.Write(buffer, offset, count);
		}

		public override bool CanRead => _underlyingStream.CanRead;
		public override bool CanSeek => _underlyingStream.CanSeek;
		public override bool CanWrite => _underlyingStream.CanWrite;
		public override long Length => _underlyingStream.Length;
		public override long Position
		{
			get => _underlyingStream.Position;
			set => _underlyingStream.Position = value;
		}

		#endregion

		#region private

		private void FailIfSomeFailsLeft()
		{
			--_failsLeft;

			if (_failsLeft >= 0)
				throw new RecoverableException("FAIL");
		}

		private readonly Stream _underlyingStream;
		private int _failsLeft;

		#endregion
	}
}
