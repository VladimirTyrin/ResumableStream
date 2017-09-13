// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ResumableStream
{
	/// <summary>
	///		Alwars returns stream passed to constuctor
	/// </summary>
	[PublicAPI]
    public class SingleStreamProvider : IStreamProvider
    {
		public SingleStreamProvider(Stream stream)
	    {
			_stream = stream;
		}

	    public Stream GetStream(long position)
	    {
			if (_stream.CanSeek)
				_stream.Seek(position, SeekOrigin.Begin);
		    return _stream;
	    }

	    public Task<Stream> GetStreamAsync(long position, CancellationToken cancellationToken)
	    {
			cancellationToken.ThrowIfCancellationRequested();

			if (_stream.CanSeek)
				_stream.Seek(position, SeekOrigin.Begin);

		    return Task.FromResult(_stream);
	    }

	    private readonly Stream _stream;
	}
}
