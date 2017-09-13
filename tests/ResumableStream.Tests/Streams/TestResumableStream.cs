// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using ResumableStream.Tests.Exceptions;
using ResumableStream.Tests.Mocks;

namespace ResumableStream.Tests.Streams
{
	internal class TestResumableStream : ReadableResumableStream
	{
		public TestResumableStream(IStreamProvider streamProvider) : base(streamProvider)
		{
		}

		protected override bool ErrorIsRecoverable(Exception exception) => exception is RecoverableException;
	}
}
