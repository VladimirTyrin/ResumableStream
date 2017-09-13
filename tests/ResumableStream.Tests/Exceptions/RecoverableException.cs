﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;

namespace ResumableStream.Tests.Exceptions
{
	internal class RecoverableException : Exception
	{
		public RecoverableException()
		{
			
		}

		public RecoverableException(string message) : base(message)
		{
			
		}
	}
}
