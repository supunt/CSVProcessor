namespace FileAnalyzer.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// FileTypeException
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class FileTypeException : Exception
    {
        public FileTypeException(string message)
            : base(message)
        {
        }
    }
}
