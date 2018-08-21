using System;

namespace OOPatterns.Core.Utils.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    class OOPatternsException : Exception
    {
        /// <summary>
        /// Message of exception
        /// </summary>
        public new string Message { get; }
        //public string Location { get; }

        public OOPatternsException(string message)
        {
            Message = message;

            // Get stack trace for the exception with source file information
            //var st = new StackTrace(this, true);
            // Get the top stack frame
            //var frame = st.GetFrame(0);
            //Location = $"Line: {frame?.GetFileLineNumber()}  Column: {frame?.GetFileColumnNumber()} in {frame?.GetFileName()}";
        }

        public override string ToString()
        {
            return $"{Message}";
        }
    }
}
