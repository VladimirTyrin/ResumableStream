// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ResumableStream.Tests.Mocks
{
	internal class NullStreamProvider : IStreamProvider
	{
		public Stream GetStream(long position) => null;

		public Task<Stream> GetStreamAsync(long position, CancellationToken cancellationToken) => Task.FromResult((Stream) null);
	}
}
