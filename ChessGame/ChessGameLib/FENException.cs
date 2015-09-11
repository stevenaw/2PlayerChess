using System;
using System.Collections.Generic;
using System.Text;

namespace ChessGameLib
{
    public class FENException : Exception
    {
        public int ErrorIndex { get; private set; }

        public FENException() : base("An unexpected symbol was encountered") { }
        public FENException(string message, int idx) : base(message) { ErrorIndex = idx; }
        public FENException(string message, int idx, Exception innerException) : base(message, innerException) { ErrorIndex = idx; }
    }
}
