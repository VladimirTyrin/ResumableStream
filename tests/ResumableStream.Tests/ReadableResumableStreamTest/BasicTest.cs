// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResumableStream.Tests.Exceptions;
using ResumableStream.Tests.Mocks;
using ResumableStream.Tests.Streams;

namespace ResumableStream.Tests.ReadableResumableStreamTest
{
	[TestClass]
	public class BasicTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ReadableResumableStream_ShouldNotBeCreatedWithNoProvider()
		{
			// ReSharper disable once ObjectCreationAsStatement
			new ReadableResumableStream(null);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ReadableResumableStream_ShouldNotBeCreatedIfFailedToAcquireStream()
		{
			// ReSharper disable once ObjectCreationAsStatement
			new ReadableResumableStream(new NullStreamProvider());
		}

		[TestMethod]
		public void ReadableResumableStream_ShouldRecoverFromReadErrorAndRaiseEvent()
		{
			var streamProvider = CreateFailingStreamProvider();

			var resumableStream = new ReadableResumableStream(streamProvider);
			var errorEvents = 0;

			resumableStream.RecoverableError += (sender, args) =>
			{
				++errorEvents;
				Assert.IsNotNull(args);
				Assert.AreEqual(OperationType.Read, args.OperationType);
				Assert.AreEqual(typeof(RecoverableException), args.Exception.GetType());
			};

			var buffer = new byte[64];
			var read = resumableStream.Read(buffer, 0, buffer.Length);
			Assert.AreEqual(buffer.Length, read);
			Assert.AreEqual(2, errorEvents);
		}

		[TestMethod]
		public async Task ReadableResumableStream_ShouldRecoverFromAsyncReadErrorAndRaiseEvent()
		{
			var streamProvider = CreateFailingStreamProvider();

			var resumableStream = new ReadableResumableStream(streamProvider);
			var errorEvents = 0;

			resumableStream.RecoverableError += (sender, args) =>
			{
				++errorEvents;
				Assert.IsNotNull(args);
				Assert.AreEqual(OperationType.Read, args.OperationType);
				Assert.AreEqual(typeof(RecoverableException), args.Exception.GetType());
			};

			var buffer = new byte[64];
			var read = await resumableStream.ReadAsync(buffer, 0, buffer.Length);
			Assert.AreEqual(buffer.Length, read);
			Assert.AreEqual(2, errorEvents);
		}

		private static IStreamProvider CreateFailingStreamProvider()
		{
			var memoryStream = new MemoryStream(new byte[256]);
			var failingStream = new StartFailingStream(memoryStream, 2);
			return new SingleStreamProvider(failingStream);
		}
	}
}
