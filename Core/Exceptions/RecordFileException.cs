using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Records.Core.Exceptions
{
    public class RecordFileException : Exception
    {
        private const int MaxErrorCount = 10;

        private ulong _errorCount;
        private List<string> _errorMessages = new List<string>();

        public bool HasErrors => _errorCount > 0;
        public IReadOnlyList<string> ErrorMessages => _errorMessages.AsReadOnly();

        public string FullMessage
        {
            get
            {
                var stringBuilder = new StringBuilder();

                if (_errorCount == 1)
                    stringBuilder.AppendLine($"The following issue was found:");
                else if (_errorCount <= MaxErrorCount)
                    stringBuilder.AppendLine($"{_errorCount} issue weres found:");
                else
                    stringBuilder.AppendLine($"{_errorCount} issues were found. The first {MaxErrorCount} are:");

                stringBuilder.AppendLine();

                foreach (var errorMessage in _errorMessages)
                    stringBuilder.AppendLine($"\t{errorMessage}");

                return stringBuilder.ToString();
            }
        }

        public RecordFileException AddError(string fileName, string message)
        {
            ++_errorCount;
            if (_errorMessages.Count < MaxErrorCount)
                _errorMessages.Add($"{fileName}:\t{message}");
            return this;
        }

        public RecordFileException AddError(string fileName, ulong lineNo, string message)
        {
            ++_errorCount;
            if (_errorMessages.Count < MaxErrorCount)
                _errorMessages.Add($"{fileName} at line {lineNo}:\t{message}");
            return this;
        }
    }
}
