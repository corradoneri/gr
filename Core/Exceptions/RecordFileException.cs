using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Records.Core.Exceptions
{
    public class RecordFileException : Exception
    {
        private const int MaxErrorCount = 10;

        private string _fileName;
        private int _errorCount;
        private List<string> _errors = new List<string>();

        public string FileName => _fileName;
        public int ErrorCount => _errorCount;
        public IReadOnlyList<string> Errors => _errors.AsReadOnly();

        public RecordFileException(string fileName)
        {
            _fileName = fileName;
        }

        public RecordFileException(string fileName, string error)
        {
            _fileName = fileName;
            AddError(error);
        }

        public RecordFileException AddError(string error)
        {
            ++_errorCount;
            if (_errors.Count < MaxErrorCount)
                _errors.Add(error);
            return this;
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            if (_errorCount == 1)
                stringBuilder.AppendLine($"{_errorCount} issue was found in file {_fileName}:");
            else if (_errorCount <= MaxErrorCount)
                stringBuilder.AppendLine($"{_errorCount} issue were found in file {_fileName}:");
            else
                stringBuilder.AppendLine($"{_errorCount} issues were found in file {_fileName}. The first {MaxErrorCount} are:");

            stringBuilder.AppendLine();

            foreach (var error in _errors)
                stringBuilder.AppendLine($"\t{error}");

            return stringBuilder.ToString();
        }
    }
}
