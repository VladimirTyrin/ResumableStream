// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using JetBrains.Annotations;

namespace ResumableStream
{
	[PublicAPI]
	public class StreamErrorEventArgs
	{
		public readonly long Position;
		public readonly int RequestedCount;
		public readonly OperationType OperationType;
		public readonly Exception Exception;

		public StreamErrorEventArgs(OperationType operationType, long position, int requestedCount, Exception exception)
		{
			OperationType = operationType;
			Position = position;
			RequestedCount = requestedCount;
			Exception = exception;
		}
	}
}
