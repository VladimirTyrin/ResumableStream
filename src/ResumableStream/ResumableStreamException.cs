// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using JetBrains.Annotations;

namespace ResumableStream
{
	[PublicAPI]
	public class ResumableStreamException : Exception
	{
		public ResumableStreamException()
		{
		}

		public ResumableStreamException(string message) : base(message)
		{
		}

		public ResumableStreamException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
