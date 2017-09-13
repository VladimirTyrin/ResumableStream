// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ResumableStream.Tests.Exceptions;

namespace ResumableStream.Tests.Mocks
{
	/// <summary>
	///		Fails after first return
	/// </summary>
	public class FailingStreamProvider : IStreamProvider
	{
		public FailingStreamProvider(Stream stream, bool recoverable)
		{
			_stream = stream;
			_recoverable = recoverable;
		}

		public Stream GetStream(long position)
		{
			if (_shouldFail)
				Fail();

			_shouldFail = true;
			return _stream;
		}

		public Task<Stream> GetStreamAsync(long position, CancellationToken cancellationToken)
		{
			if (_shouldFail)
				Fail();

			_shouldFail = true;
			return Task.FromResult(_stream);
		}

		private void Fail()
		{
			if (_recoverable)
				throw new RecoverableException();
			throw new UnrecoverableException();
		}

		private bool _shouldFail;
		private readonly Stream _stream;
		private readonly bool _recoverable;
	}
}
