// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ResumableStream
{
    [PublicAPI]
    public interface IStreamProvider
    {
        Stream GetStream(long position);
        Task<Stream> GetStreamAsync(long position, CancellationToken cancellationToken);
    }
}
