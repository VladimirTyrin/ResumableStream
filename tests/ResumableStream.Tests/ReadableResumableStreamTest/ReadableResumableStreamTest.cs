// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ResumableStream.Tests
{
	[TestClass]
	public class ReadableResumableStreamTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ReadableResumableStream_ShouldNotBeCreatedWithNoProvider()
		{
			// ReSharper disable once ObjectCreationAsStatement
			new ReadableResumableStream(null);
		}
	}
}
