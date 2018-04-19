using System;

namespace GR.Records.Core.Exceptions
{
    public class RecordException : Exception
    {
        public RecordException(string error)
            : base(error)
        {
        }
    }
}
