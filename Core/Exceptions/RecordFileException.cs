using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Records.Core.Exceptions
{
    /// <summary>
    /// Used to return list of errors to callers
    /// </summary>
    public class RecordFileException : Exception
    {
        private const int DefaultMaxErrors = 10;

        private int _maxErrors;
        private int _errorCount;
        private List<string> _errorMessages = new List<string>();

        public bool HasErrors => _errorCount > 0;
        public IReadOnlyList<string> ErrorMessages => _errorMessages.AsReadOnly();

        /// <summary>
        /// Basic constructor taking option max error count
        /// </summary>
        /// <param name="maxErrors">Maximum number of error messages to keep in the exception</param>
        public RecordFileException(int maxErrors = DefaultMaxErrors)
        {
            _maxErrors = maxErrors;
        }

        /// <summary>
        /// Formats the exception in a friendly manner
        /// </summary>
        public string FullMessage
        {
            get
            {
                var stringBuilder = new StringBuilder();

                if (_errorCount == 1)
                    stringBuilder.AppendLine($"The following issue was found:");
                else if (_errorCount <= _maxErrors)
                    stringBuilder.AppendLine($"{_errorCount} issue weres found:");
                else
                    stringBuilder.AppendLine($"{_errorCount} issues were found. The first {_maxErrors} are:");

                stringBuilder.AppendLine();

                foreach (var errorMessage in _errorMessages)
                    stringBuilder.AppendLine($"\t{errorMessage}");

                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// Adds error
        /// </summary>
        /// <param name="fileName">File the error occurred in</param>
        /// <param name="message">Error messdage</param>
        /// <returns>
        /// Reference to the exception so it can be used in a fluent-api like manner
        /// </returns>
        public RecordFileException AddError(string fileName, string message)
        {
            ++_errorCount;
            if (_errorMessages.Count < _maxErrors)
                _errorMessages.Add($"{fileName}:\t{message}");
            return this;
        }

        /// <summary>
        /// Adds error
        /// </summary>
        /// <param name="fileName">File the error occurred in</param>
        /// <param name="lineNo">Line number the error occurred on</param>
        /// <param name="message">Error messdage</param>
        /// <returns>
        /// Reference to the exception so it can be used in a fluent-api like manner
        /// </returns>
        public RecordFileException AddError(string fileName, ulong lineNo, string message)
        {
            ++_errorCount;
            if (_errorMessages.Count < _maxErrors)
                _errorMessages.Add($"{fileName} at line {lineNo}:\t{message}");
            return this;
        }
    }
}
