// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.IO;

namespace ResumableStream.Tests.Streams
{
	internal abstract class TestStreamBase : Stream
	{
		public override void Flush()
		{
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return 0;
		}

		public override void SetLength(long value)
		{
			if (value < 0)
				throw new ArgumentOutOfRangeException(nameof(value), "Length cannot be negative");

			_length = value;
		}

		public override bool CanRead => false;
		public override bool CanSeek => false;
		public override bool CanWrite => false;
		public override long Length => _length;
		public override long Position { get; set; }

		protected long _length;
	}
}
